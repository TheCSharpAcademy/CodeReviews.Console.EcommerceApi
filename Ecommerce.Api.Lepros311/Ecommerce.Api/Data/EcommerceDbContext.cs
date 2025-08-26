using Ecommerce.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Data;

public class EcommerceDbContext : DbContext
{
    public EcommerceDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Sale> Sales { get; set; }

    public DbSet<LineItem> LineItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EcommerceDb;Trusted_Connection=True;Initial Catalog=EcommerceDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.Property(c => c.CategoryName).IsRequired();
            entity.Property(c => c.IsDeleted).IsRequired();
            entity.HasQueryFilter(c => !c.IsDeleted);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products", t => t.HasCheckConstraint("CK_Products_Price", "Price > 0"));
            entity.Property(p => p.ProductName).IsRequired();
            entity.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(p => p.CategoryId).IsRequired();
            entity.Property(p => p.IsDeleted).IsRequired();
            entity.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId).IsRequired();
            entity.HasQueryFilter(p => !p.IsDeleted);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("Sales");
            entity.Property(s => s.DateAndTimeOfSale).IsRequired();
            entity.Property(s => s.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
            entity.Ignore(s => s.Subtotal);
            entity.HasMany(s => s.LineItems).WithOne(li => li.Sale).HasForeignKey(li => li.SaleId).IsRequired();
            entity.HasQueryFilter(s => !s.IsDeleted);
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.ToTable("LineItems", t => t.HasCheckConstraint("CK_LineItems_Quantity", "Quantity > 0"));
            entity.Property(li => li.SaleId).IsRequired();
            entity.Property(li => li.ProductId).IsRequired();
            entity.Property(li => li.Quantity).IsRequired();
            entity.Property(li => li.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
            entity.HasOne(li => li.Sale).WithMany(s  => s.LineItems).HasForeignKey(li => li.SaleId).IsRequired();
            entity.HasOne(li => li.Product).WithMany(p => p.LineItems).HasForeignKey(li => li.ProductId).IsRequired();
            entity.HasQueryFilter(li => !li.IsDeleted);
        });

        modelBuilder.Entity<Category>()
            .HasData(new List<Category>
            {
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Shoes",
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Socks",
                },
                new Category
                {
                    CategoryId = 3,
                    CategoryName = "Pants",
                },
                new Category
                {
                    CategoryId = 4,
                    CategoryName = "Shirts",
                }
            });

        modelBuilder.Entity<Product>()
           .HasData(new List<Product>
           {
                new Product
                {
                    ProductId = 1,
                    ProductName = "Hightop Sneakers",
                    Price = 75.50m,
                    CategoryId = 1,
                },
                new Product
                {
                    ProductId = 2,
                    ProductName = "Boat Loafers",
                    Price = 53.75m,
                    CategoryId = 1,
                },
                new Product
                {
                    ProductId = 3,
                    ProductName = "Dress Socks",
                    Price = 15.25m,
                    CategoryId = 2,
                },
                new Product
                {
                    ProductId = 4,
                    ProductName = "Ankle Socks",
                    Price = 10.15m,
                    CategoryId = 2,
                },
                new Product
                {
                    ProductId = 5,
                    ProductName = "Dress Slacks",
                    Price = 35.99m,
                    CategoryId = 3,
                },
                new Product
                {
                    ProductId = 6,
                    ProductName = "Stonewash Jeans",
                    Price = 45.95m,
                    CategoryId = 3,
                },
                new Product
                {
                    ProductId = 7,
                    ProductName = "Flannel Shirt",
                    Price = 34.75m,
                    CategoryId = 4,
                },
                new Product
                {
                    ProductId = 8,
                    ProductName = "Shortsleeve Polo",
                    Price = 22.99m,
                    CategoryId = 4,
                }
           });

        modelBuilder.Entity<Sale>()
            .HasData(new List<Sale>
            {
                new Sale
                {
                    SaleId = 1,
                    DateAndTimeOfSale = new DateTime(2025, 08, 01, 12, 37, 22),
                    TotalPrice = 22.99m
                },
                new Sale
                {
                    SaleId = 2,
                    DateAndTimeOfSale = new DateTime(2025, 08, 03, 14, 30, 48),
                    TotalPrice = 61.20m
                },
                new Sale
                {
                    SaleId = 3,
                    DateAndTimeOfSale = new DateTime(2025, 08, 07, 11, 39, 10),
                    TotalPrice = 156.20m
                },
                new Sale
                {
                    SaleId = 4,
                    DateAndTimeOfSale = new DateTime(2025, 08, 07, 19, 13, 55),
                    TotalPrice = 107.50m
                },
                new Sale
                {
                    SaleId = 5,
                    DateAndTimeOfSale = new DateTime(2025, 08, 08, 9, 04, 17),
                    TotalPrice = 95.80m
                }
            });

        modelBuilder.Entity<LineItem>().HasData(
            new LineItem { LineItemId = 1, SaleId = 1, ProductId = 8, Quantity = 1, UnitPrice = 22.99m },
            new LineItem { LineItemId = 2, SaleId = 2, ProductId = 6, Quantity = 1, UnitPrice = 45.95m },
            new LineItem { LineItemId = 3, SaleId = 2, ProductId = 3, Quantity = 1, UnitPrice = 15.25m },
            new LineItem { LineItemId = 4, SaleId = 3, ProductId = 7, Quantity = 1, UnitPrice = 34.75m },
            new LineItem { LineItemId = 5, SaleId = 3, ProductId = 6, Quantity = 1, UnitPrice = 45.95m },
            new LineItem { LineItemId = 6, SaleId = 3, ProductId = 1, Quantity = 1, UnitPrice = 75.50m },
            new LineItem { LineItemId = 7, SaleId = 4, ProductId = 2, Quantity = 2, UnitPrice = 53.75m },
            new LineItem { LineItemId = 8, SaleId = 5, ProductId = 4, Quantity = 2, UnitPrice = 10.15m },
            new LineItem { LineItemId = 9, SaleId = 5, ProductId = 1, Quantity = 1, UnitPrice = 75.50m }
        );

    }
}
