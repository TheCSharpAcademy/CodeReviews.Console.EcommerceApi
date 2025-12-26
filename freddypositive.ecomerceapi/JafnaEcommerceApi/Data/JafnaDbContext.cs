using JafnaEcommerceApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JafnaEcommerceApi.Data;

public class JafnaDbContext : DbContext
{
    public DbSet<Category> category { get; set; }
    public DbSet<Product> products { get; set; }
    public DbSet<Sale> sale { get; set; }
    public DbSet<SaleDetail> sale_details { get; set; }

    public JafnaDbContext(DbContextOptions<JafnaDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------------- CATEGORY → PRODUCTS (1 to Many) ----------------
        modelBuilder.Entity<Category>()
        .HasMany(s => s.Products)
        .WithOne(c => c.Category)
        .HasForeignKey(p => p.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);

        // ---------------- PRODUCT → SALEDETAILS (1 to Many) ----------------
        modelBuilder.Entity<Product>()
        .HasMany(s => s.SaleDetails)
        .WithOne(sd => sd.Product)
        .HasForeignKey(sd => sd.ProductId)
        .OnDelete(DeleteBehavior.Restrict);

        // ---------------- SALE → SALEDETAILS (1 to Many) -------------------
        modelBuilder.Entity<Sale>()
        .HasMany(s => s.SaleDetails)
        .WithOne(d => d.Sale)
        .HasForeignKey(sd => sd.SaleId)
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Category>()
        .Property(s => s.CreatedDate)
        .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Product>()
        .Property(s => s.CreatedDate)
        .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Category>()
        .Property(s => s.DeletedDate)
        .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Product>()
        .Property(s => s.DeletedDate)
        .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Sale>()
     .Property(s => s.CreatedDate)
     .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<SaleDetail>()
     .Property(s => s.CreatedDate)
     .HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<Category>().HasData(
        new Category
        {
            Id = 1,
            Name = "Fish",
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        },
        new Category
        {
            Id = 2,
            Name = "Plants",
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        },
        new Category
        {
            Id = 3,
            Name = "Aquarium Supplies",
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        }
        );
        modelBuilder.Entity<Product>().HasData(
        new Product
        {
            Id = 1,
            Name = "Goldfish",
            Description = "Beautiful hardy goldfish",
            Price = 100,
            Image = "goldfish.jpg",
            CategoryId = 1,
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        },
        new Product
        {
            Id = 2,
            Name = "Amazon Sword",
            Description = "Popular aquarium plant",
            Price = 150,
            Image = "amazonsword.jpg",
            CategoryId = 2,
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        },
        new Product
        {
            Id = 3,
            Name = "Internal Filter",
            Description = "Small internal filter",
            Price = 500,
            Image = "filter.jpg",
            CategoryId = 3,
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        }
        );
        modelBuilder.Entity<Sale>().HasData(
        new Sale
        {
            Id = 1,
            TotalPrice = 200,
            CreatedDate = DateTime.UtcNow
        }
        );
        modelBuilder.Entity<SaleDetail>().HasData(
        new SaleDetail
        {
            Id = 1,
            SaleId = 1,
            ProductId = 1,
            ProductName = "Goldfish",   // REQUIRED NOW
            ProductQuantity = 2,
            ProductPrice = 100,
            ProductTotalPrice = 200,
            CreatedDate = DateTime.UtcNow
        }
         );

    }
}
