﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetCoreIdentity.Application;
using DotNetCoreIdentity.Application.BlogServices;
using DotNetCoreIdentity.Application.BlogServices.Dtos;
using DotNetCoreIdentity.Domain.BlogEntries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;

namespace DotNetCoreIdentity.Web.CMS.Controllers
{
    [Authorize(Roles = "Admin,Editor")]
    [Area("CMS")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;
        private readonly IFileProvider _fileProvider;
        private readonly IWebHostEnvironment _env;

        public PostController(IPostService postService, ICategoryService categoryService, IFileProvider fileProvider, IWebHostEnvironment env)
        {
            _postService = postService;
            _categoryService = categoryService;
            _fileProvider = fileProvider;
            _env = env;
        }

        // Liste
        public async Task<IActionResult> Index()
        {
            var getAllService = await _postService.GetAll();
            List<PostDto> model = getAllService.Result;
            return View(model);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            ApplicationResult<PostDto> data = await _postService.Get(id);
            return View(data.Result);
        }

        public async Task<IActionResult> Create()
        {
            var categoryList = await _categoryService.GetAll();
            ViewBag.CategoryDDL = categoryList.Result.Select(x => new SelectListItem
            {
                Selected = false,
                Text = x.Name,
                Value = x.Id.ToString()

            }).ToList();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreatePostInput model)
        {
            if (ModelState.IsValid)
            {
                // createdById alanını doldur
                model.CreatedBy = User.FindFirst(ClaimTypes.Name).Value;
                // createService gönder
                var createService = await _postService.Create(model);
                // hata yoksa Index'e redirect et
                if (createService.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                // hata varsa hatayı ModelState' e ekle
                ModelState.AddModelError(string.Empty, createService.ErrorMessage);
            }

            var categoryList = await _categoryService.GetAll();
            ViewBag.CategoryDDL = categoryList.Result.Select(x => new SelectListItem
            {
                Selected = false,
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
            return View(model);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var post = await _postService.Get(id);
            var categoryList = await _categoryService.GetAll();
            ViewBag.CategoryDDL = categoryList.Result.Select(c => new SelectListItem
            {
                Selected = false,
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            UpdatePostInput model = new UpdatePostInput
            {
                Id = post.Result.Id,
                CategoryId = post.Result.CategoryId,
                Content = post.Result.Content,
                Title = post.Result.Title,
                UrlName = post.Result.UrlName,
                CreatedById = post.Result.CreatedById
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, UpdatePostInput model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id)
                {
                    ModelState.AddModelError(string.Empty, "Forma müdahele etme!");
                }
                else
                {
                    model.ModifiedBy = User.FindFirst(ClaimTypes.Name).Value;

                    var updatePost = await _postService.Update(model);
                    if (updatePost.Succeeded)
                    {
                        return RedirectToAction("Details", new { id = model.Id });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Bir hata oluştu:\n" + updatePost.ErrorMessage);
                    }
                }
            }
            var categoryList = await _categoryService.GetAll();
            ViewBag.CategoryDDL = categoryList.Result.Select(c => new SelectListItem
            {
                Selected = false,
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await _postService.Get(id);
            return View(model.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, PostDto input)
        {
            if (ModelState.IsValid && id == input.Id)
            {
                var delete = await _postService.Delete(id);
                if (delete.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Bir hata oluştu");
            return View(input);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteApi([FromBody] PostDeleteDto model)
        {
            var delete = await _postService.Delete(model.Id);
            return Json(delete);
        }

        public async Task<IActionResult> UploadImage(Guid id)
        {
            ApplicationResult<PostDto> data = await _postService.Get(id);
            return View(data.Result);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(Guid id, IFormFile file)
        {
            if (file != null || file.Length != 0)
            {
                // Resmi al değişkene ata
                FileInfo fi = new FileInfo(file.FileName);
                // Bir dosya adı belirle
                var newfileName = id.ToString() + "_" + String.Format("{0:d}", (DateTime.Now.Ticks / 10) % 100000000) + fi.Extension;
                // Resmi belirtilen path'e yükle
                var webPath = _env.WebRootPath;
                var path = Path.Combine("", webPath + @"\images\" + newfileName);
                var pathToSave = @"/images/" + newfileName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Yükleme tamamlandıktan sonra resmin yolunu db'ye yükle (imageUrl alanını oluştur)
                var updateUrl = await _postService.UpdateImageUrl(id, pathToSave);
                if (updateUrl.Succeeded)
                {
                    return RedirectToAction("UploadImage", new { id });
                }
            }
            return RedirectToAction("Error", "Home");
        }
    }
}

