using ECommerceApp.RyanW84.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApp.RyanW84.Data.Configurations;

public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(s => s.SaleId);
        builder.Property(s => s.SaleDate).IsRequired();
        builder.Property(s => s.TotalAmount).IsRequired().HasPrecision(18, 2);
        builder.Property(s => s.CustomerName).IsRequired().HasMaxLength(100);
        builder.Property(s => s.CustomerEmail).IsRequired().HasMaxLength(100);
        builder.Property(s => s.CustomerAddress).IsRequired().HasMaxLength(200);

        builder
            .HasMany(s => s.SaleItems)
            .WithOne(si => si.Sale)
            .HasForeignKey(si => si.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}