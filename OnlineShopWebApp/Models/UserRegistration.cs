using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class UserRegistration
    {
        [Required(ErrorMessage = "Не указан логин")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Login { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Длина пароля должна быть от 8")]
        [Required(ErrorMessage = "Не указан пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указан повторный пароль")]
        [Compare("Password", ErrorMessage = "Проверочный пароль должен совпадать с паролем")]
        public string PasswordConfirm { get; set; }

    }
}
