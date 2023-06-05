using Microsoft.EntityFrameworkCore;
using selection_committee.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace selection_committee.DB
{
    class ApplicationContext : DbContext
    {
        public DbSet<Entrant> Entrants { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }


    }
}
