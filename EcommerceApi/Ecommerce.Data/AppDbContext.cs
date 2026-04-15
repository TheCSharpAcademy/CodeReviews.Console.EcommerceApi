using Ecommerce.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options)

{
    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Sale> Sales { get; set; }

    public DbSet<SaleItem> SaleItems { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder
            .Entity<Product>()
            .HasQueryFilter(p => !p.IsDeleted)
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder
            .Entity<Sale>()
            .HasQueryFilter(p => !p.IsDeleted)
            .Property(s => s.TotalPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Category>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder
            .Entity<SaleItem>()
            .HasKey(si => new { si.SaleId, si.ProductId });

        modelBuilder
            .Entity<SaleItem>()
            .Property(si => si.UnitPriceAtTimeOfSale)
            .HasPrecision(18, 2);
    }
}