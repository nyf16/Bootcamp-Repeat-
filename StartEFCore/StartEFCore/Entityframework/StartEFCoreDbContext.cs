using Microsoft.EntityFrameworkCore;
using StartEFCore.Data;
using StartEFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StartEFCore.Entityframework
{
    public class StartEFCoreDbContext : DbContext
    {
        // Yapici method
        public StartEFCoreDbContext(DbContextOptions<StartEFCoreDbContext> options) : base(options)
        {
            // bos constructor
        }

        // Model siniflari al ve ef ile tanıstır yani contexte ekle
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
