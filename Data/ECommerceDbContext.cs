using ECommerceApp.RyanW84.Data.Configuration;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.RyanW84.Data;

/// <summary>
/// EF Core DbContext for eCommerce domain.
/// Delegates model configuration and seeding to separate handler classes per SRP.
/// </summary>
public class ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Sale> Sales { get; set; } = null!;
    public DbSet<SaleItem> SaleItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ModelConfiguration.ConfigureModels(modelBuilder);
        IndexConfiguration.ConfigureIndexes(modelBuilder);
        ConfigureGlobalQueryFilters(modelBuilder);
    }

    private static void ConfigureGlobalQueryFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<SaleItem>().HasQueryFilter(si => !si.Product!.IsDeleted);
    }

    public void SeedData() => DatabaseSeeder.SeedDatabase(this);
}
