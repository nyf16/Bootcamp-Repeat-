using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreIdentity.Domain.Identity;
using DotNetCoreIdentity.Web.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreIdentity.Web.Controllers
{
    public class AccountController : Controller
    {
        // Kullanici kaydetmek için veya kullanici bilgilerinde değişiklik yapmak için kullanılan servis.
        private readonly UserManager<ApplicationUser> _userManager;

        // Kullanicinin uygulamaya giriş - çıkış işlemlerini yönettiğimiz servis.
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Gelen modeli doğrula
            if (ModelState.IsValid)
            {
                // Model Doğruysa
                // Kullaniciyi kontrol et var mi ?
                var existUser = await _userManager.FindByEmailAsync(model.Username);
                // Yoksa hata dön
                if (existUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Bu email ile kayıtlı bir kullanıcı bulunamadı!");
                    return View(model);
                }

                // Ana Sayfaya yönlendir (şimdilik)
                return RedirectToAction("Index", "Home");
            }
            // Başarılı değilse hata dön
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // gelen modeli valide et
            if (ModelState.IsValid)
            {
                // Validse kaydet

                // ApplicationUser Oluştur
                var newUser = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                    NationalIdNumber = model.NationalIdNumber
                };

                var registerUser = await _userManager.CreateAsync(newUser, model.Password);
                if (registerUser.Succeeded)
                {
                    await _signInManager.SignInAsync(newUser, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                // Kaydetme başarısızsa hatalari modelstate'e ekle
                AddErrors(registerUser);
            }
            // Değilse hatalari dön
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError(string.Empty, err.Description);
            }
        }
    }
}