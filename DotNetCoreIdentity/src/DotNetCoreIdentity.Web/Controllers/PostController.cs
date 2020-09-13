﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetCoreIdentity.Application;
using DotNetCoreIdentity.Application.BlogServices;
using DotNetCoreIdentity.Application.BlogServices.Dtos;
using DotNetCoreIdentity.Domain.BlogEntries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DotNetCoreIdentity.Web.Controllers
{
    //[Authorize(Roles = "Admin,Editor")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;

        public PostController(IPostService postService, ICategoryService categoryService)
        {
            _postService = postService;
            _categoryService = categoryService;
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
                model.CreatedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
                    model.ModifiedById = User.FindFirst(ClaimTypes.NameIdentifier).Value;
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
    }
}

