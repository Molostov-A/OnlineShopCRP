﻿using System;
using System.Collections.Generic;
using System.Linq;
using OnlineShop.Common;
using OnlineShop.Common.Interface;
using OnlineShop.Db.Interfase;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Interfase;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp
{
    public class UsersManager : IUsersManager
    {
        private readonly ICartsRepository _cartRepository;
        private readonly IRoleManager _roleManager;

        private readonly List<User> users;

        private UserAutorized userAutorized { get; set; }

        private const string nameSave = "users";
        private IWorkWithData JsonStorage { get; } = new JsonWorkWithData(nameSave);
        private IWorkWithData AutorizedData { get; } = new JsonWorkWithData("autorization");
        public UsersManager(IRoleManager roleManager, ICartsRepository cartsRepository)
        {
            users = JsonStorage.Read<List<User>>();
            userAutorized = AutorizedData.Read<UserAutorized>();
            _roleManager = roleManager;
            _cartRepository = cartsRepository;
        }
        public string GetLoginAuthorizedUser()
        {
            return userAutorized?.Login;
        }
        public void Authorized(UserAutorized user)
        {
            userAutorized = user;
            AutorizedData.Write(userAutorized);
        }

        public bool CheckingForAuthorization()
        {
            if (userAutorized == null) return false;
            var user = Find(userAutorized.Login);
            if (user == null)
            {
                Exit();
                return false;
            }
            return true;
        }

        public void AssignRole(string userLogin, Guid roleId)
        {
            var user = Find(userLogin);
            SetRole(user, roleId);
            Save();
        }
        public void SetRole(User user, Guid roleId)
        {
            user.RoleId = roleId;
            user.RoleName = _roleManager.Find(roleId).Name;
            if (roleId == MyConstant.RoleDefaultId)
            {
                _cartRepository.CreateCartBuyer(user.Login);
            }
        }

        public List<User> GetAll()
        {
            return users;
        }

        public User Find(string userLogin)
        {
            return users.FirstOrDefault(x => x.Login == userLogin);
        }

        public void Add(UserRegistration userInput)
        {
            User newUser = new User()
            {
                Login = userInput.Login,
                Password = userInput.Password
            };
            users.Add(newUser);
            AssignRole(userInput.Login, MyConstant.RoleDefaultId); //Покупатель
            Save();
        }
        public void Remove(string login)
        {
            users?.RemoveAll(x => x.Login == login);
            Save();
        }

        public void ChangePassword(string login, string password)
        {
            var user = Find(login);
            user.Password = password;
            Save();
        }

        public void ChangeInfo(UserInfo userInfo)
        {
            var user = Find(userInfo.Login);
            user.Firstname ??= userInfo.Firstname;
            user.Secondname ??= userInfo.Secondname;
            user.Surname ??= userInfo.Surname;
            user.Surname ??= userInfo.Surname;
            if (userInfo.Age != 0) user.Age = userInfo.Age;
            user.Phone ??= userInfo.Phone;
            user.Email ??= userInfo.Email;
            Save();
        }

        public void Exit()
        {
            userAutorized = null;
            AutorizedData.Write(userAutorized);
        }

        private void Save()
        {
            JsonStorage.Write(users);
        }
    }
}
