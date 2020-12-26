﻿using DotNetCoreIdentity.Application.CategoryServices.Dtos;
using DotNetCoreIdentity.Application.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCoreIdentity.Application
{
    public interface ICategoryService : ICRUDService<int, CategoryDto, CreateCategoryInput, UpdateCategoryInput>
    {
        /* Metodlar buraya */

        //Task<ApplicationResult<CategoryDto>> Create(CreateCategoryInput input);
        //Task<ApplicationResult<CategoryDto>> Get(int Id);
        //Task<ApplicationResult<List<CategoryDto>>> GetAll();
        //Task<ApplicationResult<CategoryDto>> Update(UpdateCategoryInput input);
        //Task<ApplicationResult> Delete(int Id);
    }
}
