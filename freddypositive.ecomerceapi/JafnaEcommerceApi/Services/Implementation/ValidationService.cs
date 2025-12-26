using JafnaEcommerceApi.Models.DTOs.SaleDTOs;
using JafnaEcommerceApi.Exceptions;
using System.Linq;
using JafnaEcommerceApi.Services.Interface;

namespace JafnaEcommerceApi.Services.Implementation;

public class ValidationService : IValidationService
{
    public List<SaleDetailCreateDto> ValidateAndAggregateItems(SaleCreateDto saleCreateDto)
    {
        if (saleCreateDto.SaleItems == null || saleCreateDto.SaleItems.Count == 0)
            throw new ValidationException("No products in sales details");

        var aggregated = new List<SaleDetailCreateDto>();

        foreach (var item in saleCreateDto.SaleItems)
        {
            if (item.ProductId <= 0)
                throw new ValidationException("Invalid Product");

            if (item.ProductQuantity <= 0)
                throw new ValidationException("Invalid Product Quantity");

            var existing = aggregated.FirstOrDefault(x => x.ProductId == item.ProductId);

            if (existing == null)
            {
                aggregated.Add(new SaleDetailCreateDto
                {
                    ProductId = item.ProductId,
                    ProductQuantity = item.ProductQuantity
                });
            }
            else
            {
                existing.ProductQuantity += item.ProductQuantity;
            }
        }
        return aggregated;
    }
}
