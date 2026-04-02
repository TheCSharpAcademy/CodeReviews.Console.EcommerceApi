using ECommerce.Entities.TerrenceLGee.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ECommerce.Api.TerrenceLGee.Data.DatabaseConfigs;

public class SaleProductConfiguration : IEntityTypeConfiguration<SaleProduct>
{
    public void Configure(EntityTypeBuilder<SaleProduct> builder)
    {
        builder.HasKey(sp => new { sp.SaleId, sp.ProductId });

        builder.HasOne(sp => sp.Sale)
            .WithMany(s => s.SaleProducts)
            .HasForeignKey(sp => sp.SaleId);

        builder.HasOne(sp => sp.Product)
            .WithMany(p => p.SaleProducts)
            .HasForeignKey(sp => sp.ProductId);
    }
}
