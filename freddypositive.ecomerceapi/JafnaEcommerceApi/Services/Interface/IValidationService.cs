using JafnaEcommerceApi.Models.DTOs.SaleDTOs;

namespace JafnaEcommerceApi.Services.Interface;

public interface IValidationService
{
    List<SaleDetailCreateDto> ValidateAndAggregateItems(SaleCreateDto saleCreateDto);
}
