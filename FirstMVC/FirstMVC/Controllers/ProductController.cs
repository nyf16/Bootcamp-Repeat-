using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstMVC.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            List<Product> productList = Product.GetSampleData();

            return View(productList);
        }
    }
}