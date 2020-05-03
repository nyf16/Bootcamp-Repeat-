using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StartEFCore.Entityframework;
using StartEFCore.Models;

namespace StartEFCore.Controllers
{
    public class TeamController : Controller
    {
        private readonly StartEFCoreDbContext _context;
        public TeamController(StartEFCoreDbContext context)
        {
            _context = context;
        }
        // TODO: Takımları listelemek (List)
        public IActionResult Index()
        {
            List<Team> list = _context.Teams.ToList();
            return View(list);
        }

        //TODO: Yeni takım oluşturmak (Create)
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Team model)
        {
            if (ModelState.IsValid)
            {
                _context.Teams.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //TODO: Id'si eşit olan takımın bilgileri (Detail)
        public IActionResult Detail(int id)
        {
            Team model = _context.Teams.
                Where(x => x.Id == id)
                .Include(x => x.Players).
                FirstOrDefault();
            return View(model);
        }

        //TODO: Id'si eşit olan takımın bilgilerini güncelle (Update)

        //TODO: Id'si eşit olan takımı sil (Delete)
        //TODO: Delete yapılırken takıma ait oyuncu var mı kontrol et
        // yoksa sil
        // varsa silme

    }
}