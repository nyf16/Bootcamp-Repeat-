using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StartEFCore.Controllers
{
    public class PlayerController : Controller
    {
        // TeamId değerine eşit gelecek id parametresi alır
        // TODO: Takımın oyuncularını listele (List)
        public IActionResult Index(int id)
        {
            return View();
        }

        //TODO: Yeni oyuncu oluşturmak belirtilen takım için (Create)

        //TODO: Id'si eşit olan oyuncunun bilgileri (Detail)

        //TODO: Id'si eşit olan oyuncunun bilgilerini güncelle (Update)

        //TODO: Id'si eşit olan oyuncuyu sil (Delete)
    }
}