using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DotNetCoreIdentity.Application.BlogServices.Dtos
{
    public class CreatePostInput
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string UrlName { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        public string CreatedById { get; set; }

    }
}
