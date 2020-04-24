using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StartEFCore.Controllers
{
    public class TeamController : Controller
    {
        // TODO: Takımları listelemek (List)
        public IActionResult Index()
        {
            return View();
        }

        //TODO: Yeni takım oluşturmak (Create)

        //TODO: Id'si eşit olan takımın bilgileri (Detail)

        //TODO: Id'si eşit olan takımın bilgilerini güncelle (Update)

        //TODO: Id'si eşit olan takımı sil (Delete)

    }
}