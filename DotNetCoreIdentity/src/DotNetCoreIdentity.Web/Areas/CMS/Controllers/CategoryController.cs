﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DotNetCoreIdentity.Application;
using DotNetCoreIdentity.Application.CategoryServices.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace DotNetCoreIdentity.Web.Areas.CMS.Controllers
{
    [Authorize(Roles = "Admin, Editor")]
    [Area("CMS")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var getAllService = await _categoryService.GetAll();
            return View(getAllService.Result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var getService = await _categoryService.Get(id);
            return View(getService.Result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInput model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedBy = User.FindFirst(ClaimTypes.Name).Value;

                var result = await _categoryService.Create(model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var getService = await _categoryService.Get(id);
            return View(getService.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CategoryDto model)
        {
            if (ModelState.IsValid)
            {
                var deleteService = await _categoryService.Delete(id);
                if (deleteService.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, deleteService.ErrorMessage);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            var getService = await _categoryService.Get(id);
            UpdateCategoryInput input = new UpdateCategoryInput
            {
                Id = getService.Result.Id,
                CreatedById = getService.Result.CreatedById,
                ModifiedById = User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Name = getService.Result.Name,
                UrlName = getService.Result.UrlName
            };
            return View(input);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateCategoryInput model)
        {
            if (ModelState.IsValid)
            {
                var getService = await _categoryService.Get(id);
                model.CreatedBy = getService.Result.CreatedBy;

                model.Id = getService.Result.Id;
                model.ModifiedBy = User.FindFirst(ClaimTypes.Name).Value;

                var updateService = await _categoryService.Update(model);
                if (updateService.Succeeded)
                {
                    return RedirectToAction("Details", new { id });
                }
                ModelState.AddModelError(string.Empty, updateService.ErrorMessage);
            }
            return View(model);
        }


    }
}
