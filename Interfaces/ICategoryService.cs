using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;

namespace ECommerceApp.RyanW84.Interfaces;

public interface ICategoryService
{
    Task<ApiResponseDto<Category>> CreateCategoryAsync(
        ApiRequestDto<Category> request,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<Category>> GetCategoryAsync(
        int id,
        CancellationToken cancellationToken = default
    );
    Task<PaginatedResponseDto<List<Category>>> GetAllCategoriesAsync(
        CategoryQueryParameters parameters,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<Category>> UpdateCategoryAsync(
        int id,
        ApiRequestDto<Category> request,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<bool>> DeleteCategoryAsync(
        int id,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<Category>> GetCategoryByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<List<Category>>> GetDeletedCategoriesAsync(
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<bool>> RestoreCategoryAsync(
        int id,
        CancellationToken cancellationToken = default
    );
}
