using EcommerceAPI.Ledana.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Ledana.Data
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }


        public ProductContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasQueryFilter(b => !b.IsDeleted);

            modelBuilder.Entity<Category>().HasQueryFilter(b => !b.IsDeleted);

            //view in db using entity salewithtotal
            modelBuilder.Entity<SaleWithTotal>()
    .ToView("v_SalesWithTotal")
    .HasKey(x => x.Id);


            modelBuilder.Entity<Product>()
                .HasMany(p => p.Sales)
                .WithMany(s => s.Products)
                .UsingEntity<SaleProduct>(
                j => j.HasOne(sp => sp.Sale)
                    .WithMany(s => s.SaleProducts)
                    .HasForeignKey(sp => sp.SalesId),
                j => j.HasOne(sp => sp.Product)
                    .WithMany(p => p.SaleProducts)
                    .HasForeignKey(sp => sp.ProductsId),
                j =>
                {
                    j.HasKey(sp => new { sp.SalesId, sp.ProductsId });
                    j.Property(sp => sp.TotalPrice)
                    .HasComputedColumnSql(
                        "([Quantity] * [UnitPriceAtSale]) * (1 - [Discount])",
                        stored: true
                        );
                }
                );


            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Wireless Mouse", Price = 9.99m, Stock = 115, CategoryId = 1 },
                new Product() { Id = 2, Name = "Nose Cancelling HeadPhones", Price = 29.99m, CategoryId = 1 },
                new Product() { Id = 3, Name = "Silent Keyboard", Price = 19.99m, CategoryId = 1 },

                new Product() { Id = 4, Name = "Curved Monitor", Price = 599.99m, Stock = 89, CategoryId = 2 },
                new Product() { Id = 5, Name = "All Surface Projector", Price = 129.99m, Stock = 115, CategoryId = 2 },
                new Product() { Id = 6, Name = "Wide 4K Monitor", Price = 219.99m, Stock = 120, CategoryId = 2 },

                new Product() { Id = 7, Name = "Gaming Laptop", Price = 899.99m, Stock = 115, CategoryId = 3 },
                new Product() { Id = 8, Name = "Workstation PC", Price = 1029.99m, Stock = 250, CategoryId = 3 },

                new Product() { Id = 9, Name = "CPU", Price = 299.99m, Stock = 115, CategoryId = 4 },
                new Product() { Id = 10, Name = "GPU", Price = 109.99m, Stock = 200, CategoryId = 4 },
                new Product() { Id = 11, Name = "RAM", Price = 489.99m, Stock = 115, CategoryId = 4 },
                new Product() { Id = 12, Name = "SSD", Price = 119.99m, Stock = 170, CategoryId = 4 },

                new Product() { Id = 13, Name = "Gaming Headset", Price = 89.99m, Stock = 115, CategoryId = 5 },
                new Product() { Id = 14, Name = "Studio Mic", Price = 199.99m, Stock = 200, CategoryId = 5 },

                new Product() { Id = 15, Name = "Webcam", Price = 215.99m, Stock = 115, CategoryId = 6 },
                new Product() { Id = 16, Name = "Capture Card", Price = 708.99m, Stock = 220, CategoryId = 6 },
                new Product() { Id = 17, Name = "360 Camera", Price = 4009.99m, Stock = 115, CategoryId = 6 }
                );

            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Peripherals" },
                new Category() { Id = 2, Name = "Monitors and Displays" },
                new Category() { Id = 3, Name = "Computers and Laptops" },
                new Category() { Id = 4, Name = "Components" },
                new Category() { Id = 5, Name = "Audio and Headsets" },
                new Category() { Id = 6, Name = "Video and Cameras" }
                );

            modelBuilder.Entity<Sale>().HasData(new Sale() { Id = 1, Date = new DateTime(2026, 04, 30, 12, 00, 00) },
                new Sale() { Id = 2, Date = new DateTime(2026, 05, 01, 12, 00, 00) }, new Sale() { Id = 3, Date = new DateTime(2026, 05, 02, 12, 00, 00) },
                new Sale() { Id = 4, Date = new DateTime(2026, 05, 03, 12, 00, 00) }, new Sale() { Id = 5, Date = new DateTime(2026, 05, 04, 12, 00, 00) }
                );

            modelBuilder.Entity<SaleProduct>().HasData(
                new SaleProduct()
                {
                    ProductsId = 1,
                    SalesId = 1,
                    Quantity = 2,
                    UnitPriceAtSale = 9.99m,
                    Discount = 0.10m
                },
                new SaleProduct()
                {
                    ProductsId = 1,
                    SalesId = 2,
                    Quantity = 1,
                    UnitPriceAtSale = 9.99m,
                    Discount = 0m
                },
                new SaleProduct()
                {
                    ProductsId = 3,
                    SalesId = 1,
                    Quantity = 2,
                    UnitPriceAtSale = 19.99m,
                    Discount = 0.05m
                },
                new SaleProduct()
                {
                    ProductsId = 4,
                    SalesId = 2,
                    Quantity = 2,
                    UnitPriceAtSale = 599.99m,
                    Discount = 0.20m
                },

                new SaleProduct()
                {
                    ProductsId = 5,
                    SalesId = 3,
                    Quantity = 1,
                    UnitPriceAtSale = 129.99m,
                    Discount = 0.10m
                },
                new SaleProduct()
                {
                    ProductsId = 7,
                    SalesId = 3,
                    Quantity = 1,
                    UnitPriceAtSale = 899.99m,
                    Discount = 0m
                },
                new SaleProduct()
                {
                    ProductsId = 8,
                    SalesId = 3,
                    Quantity = 1,
                    UnitPriceAtSale = 1029.99m,
                    Discount = 0.05m
                },
                new SaleProduct()
                {
                    ProductsId = 5,
                    SalesId = 2,
                    Quantity = 2,
                    UnitPriceAtSale = 129.99m,
                    Discount = 0.20m
                },

                new SaleProduct()
                {
                    ProductsId = 15,
                    SalesId = 4,
                    Quantity = 1,
                    UnitPriceAtSale = 215.99m,
                    Discount = 0.10m
                },
                new SaleProduct()
                {
                    ProductsId = 9,
                    SalesId = 4,
                    Quantity = 1,
                    UnitPriceAtSale = 299.99m,
                    Discount = 0m
                },
                new SaleProduct()
                {
                    ProductsId = 8,
                    SalesId = 4,
                    Quantity = 1,
                    UnitPriceAtSale = 1029.99m,
                    Discount = 0.05m
                },
                new SaleProduct()
                {
                    ProductsId = 5,
                    SalesId = 4,
                    Quantity = 2,
                    UnitPriceAtSale = 129.99m,
                    Discount = 0.20m
                },

                new SaleProduct()
                {
                    ProductsId = 11,
                    SalesId = 5,
                    Quantity = 1,
                    UnitPriceAtSale = 489.99m,
                    Discount = 0.10m
                },
                new SaleProduct()
                {
                    ProductsId = 12,
                    SalesId = 5,
                    Quantity = 1,
                    UnitPriceAtSale = 119.99m,
                    Discount = 0m
                },
                new SaleProduct()
                {
                    ProductsId = 17,
                    SalesId = 4,
                    Quantity = 1,
                    UnitPriceAtSale = 4009.99m,
                    Discount = 0.05m
                },
                new SaleProduct()
                {
                    ProductsId = 1,
                    SalesId = 5,
                    Quantity = 2,
                    UnitPriceAtSale = 9.99m,
                    Discount = 0.20m
                }
                );
        }
    }
}
