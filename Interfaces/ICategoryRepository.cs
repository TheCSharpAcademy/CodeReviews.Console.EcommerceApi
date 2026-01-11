using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;

namespace ECommerceApp.RyanW84.Interfaces;

public interface ICategoryRepository
{
    Task<bool> CategoryExistsAsync(
        string categoryName,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<Category?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<Category?>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    );
    Task<PaginatedResponseDto<List<Category>>> GetAllCategoriesAsync(
        CategoryQueryParameters parameters,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<Category>> AddAsync(
        Category entity,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<Category>> UpdateAsync(
        Category entity,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<bool>> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponseDto<bool>> RestoreAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponseDto<List<Category>>> GetDeletedCategoriesAsync(
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<bool>> HardDeleteAsync(
        int id,
        CancellationToken cancellationToken = default
    );
}
