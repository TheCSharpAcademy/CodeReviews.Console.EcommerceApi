using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleProductDTOs;

namespace ECommerce.Contracts.TerrenceLGee.Mappings.SaleProductMappings;

public static class ToDto
{
    extension(SaleProduct saleProduct)
    {
        public RetrievedSaleProductDto ToRetrievedSaleProductDto()
        {
            return new RetrievedSaleProductDto
            {
                SaleId = saleProduct.SaleId,
                ProductId = saleProduct.ProductId,
                ProductName = saleProduct?.Product?.Name,
                Quantity = saleProduct!.Quantity,
                Price = saleProduct.Price,
                Discount = saleProduct.Discount,
                TotalPrice = saleProduct.TotalPrice
            };
        }
    }
}