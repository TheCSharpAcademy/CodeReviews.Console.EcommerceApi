using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;

namespace ECommerceApp.RyanW84.Interfaces;

public interface IProductRepository
{

    Task<ApiResponseDto<Product?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    );
    Task<PaginatedResponseDto<List<Product>>> GetAllProductsAsync(ProductQueryParameters parameters, CancellationToken cancellationToken = default);
    Task<ApiResponseDto<Product>> AddAsync(
        Product entity,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<List<Product>>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<ApiResponseDto<Product>> UpdateAsync(
        Product entity,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<bool>> DeleteAsync(int id,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<bool>> RestoreAsync(int id,
        CancellationToken cancellationToken = default
    );
    Task<ApiResponseDto<List<Product>>> GetDeletedProductsAsync(CancellationToken cancellationToken = default);
    Task<ApiResponseDto<bool>> HardDeleteAsync(int id,
        CancellationToken cancellationToken = default
    );
}
