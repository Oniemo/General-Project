using System;
using System.Collections.Generic;
using System.Text;
using 
using test2.Models;

namespace test2.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Product> Products => Set<Product>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
            "Host=localhost;" +
            "Port=5432;" +
            "Database=Product;" +
            "Username=postgres;" +
            "Password=ROOT");
        }
    }
}
