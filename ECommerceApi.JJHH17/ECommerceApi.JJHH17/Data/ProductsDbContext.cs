using ECommerceApi.JJHH17.Models;
using Microsoft.EntityFrameworkCore;


namespace ECommerceApi.JJHH17.Data
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Sale> Sales => Set<Sale>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .HasOne(e => e.Category)
                .WithMany(e => e.Products)
                .HasForeignKey(e => e.CategoryId)
                .IsRequired();

            modelBuilder.Entity<Sale>()
                .HasMany(e => e.Products)
                .WithMany(e => e.Sales)
                .UsingEntity<Dictionary<string, object>>(
                "ProductSales",
                j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                j => j.HasOne<Sale>().WithMany().HasForeignKey("SaleId"),
                j =>
                {
                    j.HasKey("ProductId", "SaleId");
                    j.ToTable("ProductSales");
                });

            // Optional data seeding
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Nike" },
                new Category { CategoryId = 2, CategoryName = "Adidas" });

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "Jordan", Price = 100m, CategoryId = 1 },
                new Product { ProductId = 2, ProductName = "Air Force 1", Price = 75m, CategoryId = 1 },
                new Product { ProductId = 3, ProductName = "Ultra Boost", Price = 50m, CategoryId = 1 });
        }
    }
}