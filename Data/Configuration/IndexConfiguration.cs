using ECommerceApp.RyanW84.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.RyanW84.Data.Configuration;

/// <summary>
/// Configures database indexes for optimized query performance.
/// Indexes are crucial for fast lookups and filtered queries.
/// </summary>
public static class IndexConfiguration
{
    public static void ConfigureIndexes(ModelBuilder modelBuilder)
    {
        // Product indexes for common queries
        modelBuilder
            .Entity<Product>()
            .HasIndex(p => p.CategoryId)
            .HasDatabaseName("IX_Products_CategoryId");

        modelBuilder.Entity<Product>().HasIndex(p => p.Price).HasDatabaseName("IX_Products_Price");

        modelBuilder
            .Entity<Product>()
            .HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Products_IsActive");

        modelBuilder
            .Entity<Product>()
            .HasIndex(p => p.CreatedAt)
            .HasDatabaseName("IX_Products_CreatedAt");

        modelBuilder
            .Entity<Product>()
            .HasIndex(p => new { p.IsDeleted, p.IsActive })
            .HasDatabaseName("IX_Products_IsDeleted_IsActive");

        // Category indexes
        modelBuilder
            .Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique()
            .HasDatabaseName("IX_Categories_Name");

        modelBuilder
            .Entity<Category>()
            .HasIndex(c => c.IsDeleted)
            .HasDatabaseName("IX_Categories_IsDeleted");

        // Sale indexes for date range queries
        modelBuilder.Entity<Sale>().HasIndex(s => s.SaleDate).HasDatabaseName("IX_Sales_SaleDate");

        modelBuilder
            .Entity<Sale>()
            .HasIndex(s => s.CustomerEmail)
            .HasDatabaseName("IX_Sales_CustomerEmail");

        modelBuilder
            .Entity<Sale>()
            .HasIndex(s => s.CustomerName)
            .HasDatabaseName("IX_Sales_CustomerName");

        modelBuilder
            .Entity<Sale>()
            .HasIndex(s => s.TotalAmount)
            .HasDatabaseName("IX_Sales_TotalAmount");

        // SaleItem indexes
        modelBuilder
            .Entity<SaleItem>()
            .HasIndex(si => si.SaleId)
            .HasDatabaseName("IX_SaleItems_SaleId");

        modelBuilder
            .Entity<SaleItem>()
            .HasIndex(si => si.ProductId)
            .HasDatabaseName("IX_SaleItems_ProductId");
    }
}
