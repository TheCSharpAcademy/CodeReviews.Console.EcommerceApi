using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Interfaces.Helpers;

namespace ECommerceApp.RyanW84.Services.Helpers;

public class CategoryProcessingHelper : ICategoryProcessingHelper
{
    public ApiResponseDto<Category>? ValidateCreateRequest(ApiRequestDto<Category> request)
    {
        if (request.Payload is null || string.IsNullOrWhiteSpace(request.Payload.Name))
            return ApiResponseDto<Category>.Failure(System.Net.HttpStatusCode.BadRequest, "Category name is required.");
        return null;
    }

    public Category PrepareForCreate(Category incoming)
    {
        return new Category
        {
            Name = incoming.Name.Trim(),
            Description = incoming.Description.Trim(),
        };
    }

    public Category PrepareForUpdate(Category existing, Category incoming, int id)
    {
        return new Category
        {
            CategoryId = id,
            Name = string.IsNullOrWhiteSpace(incoming.Name) ? existing.Name : incoming.Name.Trim(),
            Description = string.IsNullOrWhiteSpace(incoming.Description) ? existing.Description : incoming.Description.Trim(),
            Products = existing.Products,
            Sales = existing.Sales,
            IsDeleted = existing.IsDeleted,
            DeletedAt = existing.DeletedAt,
            CreatedAt = existing.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
