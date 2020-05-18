﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DotNetCoreIdentity.Web.ViewModels.Identity
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [StringLength(30, ErrorMessage = "{0} alanı en fazla {1}, en az {2} karakter olmalıdır.", MinimumLength = 5)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Parola")]
        [StringLength(20, ErrorMessage = "{0} alanı en fazla {1}, en az {2} karakter olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Parola Doğrulama")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler aynı değil!")]
        public string ConfirmPassword { get; set; }
        [Required]
        [Display(Name = "Ad")]
        [StringLength(30, ErrorMessage = "{0} alanı en fazla {1}, en az {2} karakter olmalıdır.", MinimumLength = 3)]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Soyad")]
        [StringLength(30, ErrorMessage = "{0} alanı en fazla {1}, en az {2} karakter olmalıdır.", MinimumLength = 3)]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "T.C. Kimlik Numarası")]
        [StringLength(11, ErrorMessage = "{0} alanı {1} karakter olmalıdır!", MinimumLength = 11)]
        public string NationalIdNumber { get; set; }
    }
}
