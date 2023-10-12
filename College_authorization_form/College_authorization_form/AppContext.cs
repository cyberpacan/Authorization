using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace College_authorization_form
{
    public class AppContext : DbContext
    {
        public AppContext() 
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("..."); 
        }
    }
}
