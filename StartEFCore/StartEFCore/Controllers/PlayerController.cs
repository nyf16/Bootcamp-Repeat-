﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        // CreatedDate, ModifiedDate
        public IActionResult Index()
        {
            List<Player> list = _context.Players.ToList();
            return View(list);
        }
        // TeamId değerine eşit gelecek id parametresi alır
        // TODO: Takımın oyuncularını listele (List)
        // CreatedDate, ModifiedDate
        public IActionResult TeamPlayers(int id)
        {
            List<Player> list = _context.Players.Where(x => x.TeamId ==
            id).ToList();
            ViewData["TeamId"] = id;
            return View(list);
        }

        //TODO: Yeni oyuncu oluşturmak belirtilen takım için (Create)
        //[Route("create/team/player/{id}")]
        public IActionResult CreatePlayerToTeam(int teamId)
        {
            Player model = new Player();
            model.TeamId = teamId;
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

                return RedirectToAction("TeamPlayers", new { id = model.TeamId });
            }
            return View(model);
        }

        //TODO: Id'si eşit olan oyuncunun bilgileri (Detail)
        public IActionResult Details(int id)
        {
            Player model = _context.Players.Find(id);
            return View(model);
        }

        //TODO: Id'si eşit olan oyuncunun bilgilerini güncelle (Update)
        // set modifiedDate 
        public IActionResult Edit(int id)
        {
            Player model = _context.Players.Find(id);
            ViewBag.TeamsDDL = _context.Teams.Select(u => new SelectListItem
            {
                Selected = false,
                Text = u.Name,
                Value = u.Id.ToString()
            }).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // set modifiedDate 
        public IActionResult Edit(int id, Player model)
        {
            if (id != model.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                // context ile güncelleme
                try
                {
                    //_context.Players.Update(model);
                    //_context.SaveChanges();

                    TryToUpdatePlayer(model);
                    //return RedirectToAction("Edit", new { id = id });
                    return RedirectToAction("Edit", new { id });
                }
                catch (DBConcurrencyException ex)
                {
                    if (_context.Players.Find(id) == null)
                        return NotFound();
                    throw (ex);
                }
            }
            return View(model);

        }

        // void method hic birsey dönmez
        // set modifiedDate 
        private void TryToUpdatePlayer(Player model)
        {
            Player willUpdate = _context.Players.Find(model.Id);

            willUpdate.HiddenValue = model.HiddenValue;
            willUpdate.ImageUrl = model.ImageUrl;
            willUpdate.LongName = model.LongName;
            willUpdate.Number = model.Number;
            willUpdate.Position = model.Position;
            willUpdate.TeamId = model.TeamId;
            willUpdate.Age = model.Age;
            willUpdate.ModifiedDate = DateTime.UtcNow;
            //_context.Players.Update(model);
            _context.SaveChanges();
        }
        //TODO: Id'si eşit olan oyuncuyu sil (Delete)
        public IActionResult Delete(int id, string returnUrl = "")
        {
            ViewBag.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? "" : returnUrl;
            Player model = _context.Players.Find(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Player model, string returnUrl = "")
        {
            if (_context.Players.Find(model.Id) == null || id != model.Id)
            {
                return NotFound();
            }
            try
            {
                Player player = _context.Players.Find(model.Id);
                _context.Players.Remove(player);
                _context.SaveChanges();

                //return RedirectToAction("TeamPlayers",
                //    new { id = player.TeamId });

                if (!string.IsNullOrEmpty(returnUrl)
                    && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index");

            }
            catch (DBConcurrencyException ex)
            {

                throw (ex);
            }
        }
    }
}