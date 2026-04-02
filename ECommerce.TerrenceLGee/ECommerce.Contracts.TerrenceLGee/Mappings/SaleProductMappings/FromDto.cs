using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleProductDTOs;

namespace ECommerce.Contracts.TerrenceLGee.Mappings.SaleProductMappings;

public static class FromDto
{
    extension(CreateSaleProductDto saleProductDto)
    {
        public SaleProduct FromCreateSaleProductDto()
        {
            return new SaleProduct
            {
                ProductId = saleProductDto.ProductId,
                Quantity = saleProductDto.Quantity,
                Price = saleProductDto.Price,
                Discount = saleProductDto.Discount,
                TotalPrice = saleProductDto.TotalPrice
            };
        }
    }
}