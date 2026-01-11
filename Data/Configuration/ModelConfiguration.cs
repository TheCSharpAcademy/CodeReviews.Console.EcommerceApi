using ECommerceApp.RyanW84.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.RyanW84.Data.Configuration;

/// <summary>
/// Handles Entity Framework model configuration following SRP.
/// Centralizes all FluentAPI configurations for entities.
/// </summary>
public static class ModelConfiguration
{
    public static void ConfigureModels(ModelBuilder modelBuilder)
    {
        ConfigureProduct(modelBuilder);
        ConfigureCategory(modelBuilder);
        ConfigureSale(modelBuilder);
        ConfigureSaleItem(modelBuilder);
        ConfigureCategorySealeRelationship(modelBuilder);
    }

    private static void ConfigureProduct(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.ProductId);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Description).IsRequired().HasMaxLength(500);
            entity.Property(p => p.Price).IsRequired().HasPrecision(18, 2);
            entity.Property(p => p.Stock).IsRequired().HasDefaultValue(0);
            entity.Property(p => p.IsActive).IsRequired().HasDefaultValue(true);
            entity.Property(p => p.CategoryId).IsRequired();
            entity.HasIndex(p => p.Name).HasDatabaseName("IX_Products_Name");

            entity
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasMany(p => p.SaleItems)
                .WithOne(si => si.Product)
                .HasForeignKey(si => si.ProductId)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

    private static void ConfigureCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.CategoryId);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
            entity.Property(c => c.Description).IsRequired().HasMaxLength(500);
            entity.HasIndex(c => c.Name).IsUnique().HasDatabaseName("IX_Categories_Name");
        });

        modelBuilder
            .Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureSale(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(s => s.SaleId);
            entity.Property(s => s.SaleDate).IsRequired();
            entity.Property(s => s.TotalAmount).IsRequired().HasPrecision(18, 2);
            entity.Property(s => s.CustomerName).IsRequired().HasMaxLength(100);
            entity.Property(s => s.CustomerEmail).IsRequired().HasMaxLength(100);
            entity.Property(s => s.CustomerAddress).IsRequired().HasMaxLength(200);

            entity
                .HasMany(s => s.SaleItems)
                .WithOne(si => si.Sale)
                .HasForeignKey(si => si.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private static void ConfigureSaleItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.HasKey(si => new { si.SaleId, si.ProductId });
            entity.Property(si => si.Quantity).IsRequired();
            entity.Property(si => si.UnitPrice).IsRequired().HasPrecision(18, 2);
        });
    }

    private static void ConfigureCategorySealeRelationship(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Category>()
            .HasMany(c => c.Sales)
            .WithMany(s => s.Categories)
            .UsingEntity<Dictionary<string, object>>(
                "CategorySale",
                j =>
                    j.HasOne<Sale>()
                        .WithMany()
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade),
                j =>
                    j.HasOne<Category>()
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("CategoryId", "SaleId");
                    j.ToTable("CategorySales");
                }
            );
    }
}
