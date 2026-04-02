using ECommerce.Contracts.TerrenceLGee.Mappings.SaleProductMappings;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;

namespace ECommerce.Contracts.TerrenceLGee.Mappings.SaleMappings;

public static class ToDto
{
    extension(Sale sale)
    {
        public RetrievedSaleDto ToRetrievedSaleDto()
        {
            return new RetrievedSaleDto
            {
                Id = sale.Id,
                CustomerId = sale.CustomerId,
                CustomerName = (sale.Customer is not null) 
                ? $"{sale.Customer.FirstName} {sale.Customer.LastName}"
                : "N/A",
                TotalBaseAmount = sale.TotalBaseAmount,
                TotalDiscountAmount = sale.TotalDiscountAmount,
                TotalAmount = sale.TotalAmount,
                CreatedAt = sale.CreatedAt,
                UpdatedAt = sale.UpdatedAt,
                SaleStatus = sale.SaleStatus,
                SaleProducts = sale.SaleProducts
                .Select(sp => sp.ToRetrievedSaleProductDto()).ToList()
            };
        }

        public RetrievedSaleSummaryDto ToRetrievedSaleSummaryDto()
        {
            return new RetrievedSaleSummaryDto
            {
                Id = sale.Id,
                CustomerName = (sale.Customer is not null)
                ? $"{sale.Customer.FirstName} {sale.Customer.LastName}"
                : "N/A",
                SaleProductCount = sale.SaleProducts.Count,
                TotalBaseAmount = sale.TotalBaseAmount,
                TotalDiscountAmount = sale.TotalDiscountAmount,
                TotalAmount = sale.TotalAmount,
                CreatedAt = sale.CreatedAt,
                UpdatedAt = sale.UpdatedAt,
                SaleStatus = sale.SaleStatus
            };
        }
    }
}