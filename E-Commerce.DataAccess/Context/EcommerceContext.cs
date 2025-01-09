using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DataAccess.Context
{
    public class EcommerceContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }


        public EcommerceContext() { }
        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options) { }

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

            modelBuilder.Entity<Order>(entity =>
            { 
                // Primary Key
                entity.HasKey(o => o.Id);

                // OrderDate required
                entity.Property(o => o.OrderDate)
                      .IsRequired();
                
                // TotalAmount required
                entity.Property(o => o.TotalAmount)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)"); // Precision for TotalAmount

                // One to Many relationship between customer and order
                entity.HasOne(o => o.Customer)
                      .WithMany(c => c.Orders)
                      .HasForeignKey(o => o.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade); // Delete orders if customer deleted

                // One to Many relationship between order and products
                entity.HasMany(o => o.OrderProducts)
                      .WithOne()
                      .HasForeignKey(op => op.OrderId);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                // Primary Key
                entity.HasKey(o => o.Id);

                // Product
                entity.Property(o => o.ProductName)
                      .IsRequired()
                      .HasMaxLength(128);

                // Price
                entity.Property(o => o.Price)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                // Stock Quantity
                entity.Property(o => o.StockQuantity)
                      .IsRequired();

                // Order Products
                entity.HasMany(o => o.OrderProducts)
                      .WithOne()
                      .HasForeignKey(o => o.ProductId);
            });


            modelBuilder.Entity<User>(entity =>
            {
                // Primary Key
                entity.HasKey(u => u.Id);

                // FirstName zorunlu ve maksimum uzunluk 50 karakter
                entity.Property(u => u.FirstName)
                      .IsRequired()
                      .HasMaxLength(50);

                // LastName zorunlu ve maksimum uzunluk 50 karakter
                entity.Property(u => u.LastName)
                      .IsRequired()
                      .HasMaxLength(50);

                // Email zorunlu, benzersiz ve maksimum uzunluk 100 karakter
                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(u => u.Email)
                      .IsUnique(); // Email benzersiz

                // PhoneNumber opsiyonel, maksimum uzunluk 15 karakter
                entity.Property(u => u.PhoneNumber)
                      .HasMaxLength(15);

                // Password zorunlu
                entity.Property(u => u.Password)
                      .IsRequired();

                // Role zorunlu
                entity.Property(u => u.Role)
                      .IsRequired();

                // Orders ile One-to-Many ilişki
                entity.HasMany(u => u.Orders)
                      .WithOne(o => o.Customer)
                      .HasForeignKey(o => o.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade); // Müşteri silindiğinde siparişler de silinir
            });


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

                entity.Property(o => o.Quantity)
                      .IsRequired();
            });
        }
    }
}
