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

        public IActionResult Detail(int id)
        {
            List<Product> produtList = Product.GetSampleData();
            // örnek data icerisindeki parametreden gelen id' ye sahip urunu getir!

            // Where kosulu tum dataList icerisindeki itemlarin kosullandirilmasidir.
            Product item = produtList.Where(x => x.Id == id).FirstOrDefault();

            return View(item);
        }
    }
}