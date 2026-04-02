using ECommerce.Contracts.TerrenceLGee.Mappings.SaleProductMappings;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;

namespace ECommerce.Contracts.TerrenceLGee.Mappings.SaleMappings;

public static class FromDto
{
    extension(CreateSaleDto saleDto)
    {
        public Sale FromCreateSaleDto()
        {
            return new Sale
            {
                CustomerId = saleDto.CustomerId ?? "N/A",
                TotalBaseAmount = saleDto.TotalBaseAmount,
                TotalDiscountAmount = saleDto.TotalDiscountAmount,
                TotalAmount = saleDto.TotalAmount,
                SaleStatus = saleDto.SaleStatus,
                SaleProducts = saleDto.SaleProducts
                .Select(sp => sp.FromCreateSaleProductDto())
                .ToList()
            };
        }
    }
}