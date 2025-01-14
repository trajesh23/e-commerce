using E_Commerce.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace E_Commerce.DataAccess.Context
{
    public class EcommerceContext : IdentityDbContext<User>
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Setting> Settings { get; set; }

        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options) { }

        // Added to impede design time db context factory error
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=EcommerceDb;Trusted_Connection=true;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Order Configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(o => o.OrderDate).IsRequired();

                entity.Property(o => o.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");

                entity.HasOne(o => o.Customer)
                      .WithMany(c => c.Orders)
                      .HasForeignKey(o => o.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(o => o.ProductName).IsRequired().HasMaxLength(128);

                entity.Property(o => o.Price).IsRequired().HasColumnType("decimal(18,2)");

                entity.Property(o => o.StockQuantity).IsRequired();
            });

            // OrderProduct Configuration
            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.HasKey(o => new { o.OrderId, o.ProductId });

                entity.HasOne(o => o.Order)
                      .WithMany(o => o.OrderProducts)
                      .HasForeignKey(o => o.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(o => o.Product)
                      .WithMany(o => o.OrderProducts)
                      .HasForeignKey(o => o.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(o => o.Quantity).IsRequired();
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasData(
                    new Setting 
                    { 
                        Id = 1,
                        MaintenanceMode = false 
                    });
            });
        }
    }
}