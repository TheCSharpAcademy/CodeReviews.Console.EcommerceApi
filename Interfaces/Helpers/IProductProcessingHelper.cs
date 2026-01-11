using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;

namespace ECommerceApp.RyanW84.Interfaces.Helpers;

public interface IProductProcessingHelper
{
    ApiResponseDto<Product>? ValidateCreateRequest(ApiRequestDto<Product> request);

    Product PrepareForUpdate(Product existing, Product incoming, int id);
}
