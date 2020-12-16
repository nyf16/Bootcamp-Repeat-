﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DotNetCoreIdentity.Application.CategoryServices.Dtos
{
    public class CreateCategoryInput
    {
        [Required]
        [Display(Name = "Kategori Adı")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Seo Dostu Url")]
        public string UrlName { get; set; }
        public string CreatedById { get; set; }
    }
}
