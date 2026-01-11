using ECommerceApp.RyanW84.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApp.RyanW84.Data.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.CategoryId);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).IsRequired().HasMaxLength(500);

        builder
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(c => c.Sales)
            .WithMany(s => s.Categories)
            .UsingEntity<Dictionary<string, object>>(
                "CategorySale",
                j => j.HasOne<Sale>().WithMany().HasForeignKey("SaleId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId").OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("CategoryId", "SaleId");
                    j.ToTable("CategorySales");
                });
    }
}