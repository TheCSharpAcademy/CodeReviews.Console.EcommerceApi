using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;

namespace ECommerceApp.RyanW84.Interfaces.Helpers;

public interface ICategoryProcessingHelper
{
    ApiResponseDto<Category>? ValidateCreateRequest(ApiRequestDto<Category> request);

    Category PrepareForCreate(Category incoming);

    Category PrepareForUpdate(Category existing, Category incoming, int id);
}
