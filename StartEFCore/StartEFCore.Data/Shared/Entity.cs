using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StartEFCore.Data
{
    // Generic Base Class
    public class Entity<T> : CommonEntity
    {
        // [Key]
        public T Id { get; set; }
        // Generic olarak belirtmek onun tip değişkenliğine sahip olması demek
    }
    // Default Base Class
    public class Entity : CommonEntity
    {
        // [Key]
        public int Id { get; set; }
    }
    // Non Generic Base Class Field
    public class CommonEntity
    {
        [Display(Name = "Oluşturma Tarihi")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [Display(Name = "Son Güncelleme")]
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "Gizli Alan")]
        public string HiddenValue { get; set; }
    }
}
