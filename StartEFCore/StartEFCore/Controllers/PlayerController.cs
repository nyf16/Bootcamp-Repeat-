using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StartEFCore.Entityframework;
using StartEFCore.Models;

namespace StartEFCore.Controllers
{
    public class PlayerController : Controller
    {
        private readonly StartEFCoreDbContext _context;
        public PlayerController(StartEFCoreDbContext context)
        {
            _context = context;
        }
        // TODo: Tüm oyuncuları getirecek ındex actioni yap
        public IActionResult Index()
        {
            return View();
        }
        // TeamId değerine eşit gelecek id parametresi alır
        // TODO: Takımın oyuncularını listele (List)
        public IActionResult TeamPlayers(int id)
        {
            List<Player> list = _context.Players.Where(x => x.TeamId ==
            id).ToList();
            ViewData["TeamId"] = id;
            return View(list);
        }

        //TODO: Yeni oyuncu oluşturmak belirtilen takım için (Create)
        //[Route("create/team/player/{id}")]
        public IActionResult CreatePlayerToTeam(int TeamId)
        {
            Player model = new Player();
            model.TeamId = TeamId;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePlayerToTeam(Player model)
        {
            // model doğruysa 
            if (ModelState.IsValid)
            {

                // modeli contexte ekle
                _context.Players.Add(model);
                // contexteki tüm değişiklikleri kaydet
                _context.SaveChanges();
            }
            return View(model);
        }

        //TODO: Id'si eşit olan oyuncunun bilgileri (Detail)

        //TODO: Id'si eşit olan oyuncunun bilgilerini güncelle (Update)

        //TODO: Id'si eşit olan oyuncuyu sil (Delete)
    }
}