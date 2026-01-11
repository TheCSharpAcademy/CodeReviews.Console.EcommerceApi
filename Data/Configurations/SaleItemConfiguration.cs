using ECommerceApp.RyanW84.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerceApp.RyanW84.Data.Configurations;

public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.HasKey(si => new { si.SaleId, si.ProductId });

        builder.Property(si => si.Quantity).IsRequired();
        builder.Property(si => si.UnitPrice).IsRequired().HasPrecision(18, 2);
    }
}