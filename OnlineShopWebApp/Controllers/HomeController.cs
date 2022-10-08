using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Common.Interface;
using OnlineShop.Common;
using OnlineShop.Db.Interfase;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWorkWithData _productRepositoryJson = new JsonWorkWithData("projects_for_sale");
        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var productsDb = _productRepository.GetAll();
            if (productsDb.Count == 0)
            {
                AddListProductsDefoult(productsDb);
            }
            var productsViewModels = MappingToViewModels(productsDb);
            return View(productsViewModels);
        }

        private void AddListProductsDefoult(List<Product> productsDb)
        {
            var productsJson = _productRepositoryJson.Read<List<Product>>();
            foreach (var productJson in productsJson)
            {
                _productRepository.AddNew(productJson);
            }
            productsDb = _productRepository.GetAll();
        }

        private List<Product_ViewModel> MappingToViewModels(List<Product> productsDb)
        {
            var productsViewModels = new List<Product_ViewModel>();
            foreach (var product in productsDb)
            {
                var productViewModels = new Product_ViewModel
                {
                    Id = product.Id,
                    CodeNumber = product.CodeNumber,
                    Cost = product.Cost,
                    Description = product.Description,
                    Images = product.Images,
                    Length = product.Length,
                    Square = product.Square,
                    Width = product.Width
                };
                productsViewModels.Add(productViewModels);
            }
            return productsViewModels;
        }
    }
}
