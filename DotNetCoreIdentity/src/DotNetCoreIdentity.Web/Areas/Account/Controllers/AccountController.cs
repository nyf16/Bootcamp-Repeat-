﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreIdentity.Domain.Identity;
using DotNetCoreIdentity.Web.ViewModels.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreIdentity.Web.Areas.Account.Controllers
{
    [Area("Account")]
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

        [Route("Account/Login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [Route("Account/Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
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
                // Kullanıcı Adı ve Şifre eşleşiyor mu ?
                var login = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
                // Eşleşmiyorsa hata dön
                if (!login.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Bu email ve şifre ile uyumlu bir kullanıcı bulunamadı! Şifrenizi veya Mail adresinizi kontrol edin!");
                    return View(model);
                }

                // Ana Sayfaya yönlendir (şimdilik)
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }
            // Başarılı değilse hata dön
            return View(model);
        }

        [Route("Account/Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("Account/Register")]
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

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccesDenied(string returnUrl)
        {
            return View();
        }
    }
}