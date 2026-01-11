using ECommerceApp.RyanW84.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApp.RyanW84.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.ProductId);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
        builder.Property(p => p.Price).IsRequired().HasPrecision(18, 2);
        builder.Property(p => p.Stock).IsRequired().HasDefaultValue(0);
        builder.Property(p => p.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(p => p.CategoryId).IsRequired();
        builder.HasIndex(p => p.Name).HasDatabaseName("IX_Products_Name");

        builder
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(p => p.SaleItems)
            .WithOne(si => si.Product)
            .HasForeignKey(si => si.ProductId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}