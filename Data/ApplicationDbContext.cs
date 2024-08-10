using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VendingMachineApp.Models;

namespace VendingMachineApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Coke", Price = 3.99m, QuantityAvailable = 10 },
                new Product { Id = 2, Name = "Pepsi", Price = 6.885m, QuantityAvailable = 10 },
                new Product { Id = 3, Name = "Water", Price = 0.5m, QuantityAvailable = 10 }
            );
        }
    }
}
