using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StartEFCore.Models.Shared
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
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public string HiddenValue { get; set; }
    }
}
