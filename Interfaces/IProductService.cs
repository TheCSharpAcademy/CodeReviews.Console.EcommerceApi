using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;

namespace ECommerceApp.RyanW84.Interfaces;

public interface IProductService
{
    Task<ApiResponseDto<Product>> CreateProductAsync(ApiRequestDto<Product> request, CancellationToken cancellationToken = default);
    Task<PaginatedResponseDto<List<Product>>> GetProductsAsync(ProductQueryParameters parameters, CancellationToken cancellationToken = default);
    Task<ApiResponseDto<Product?>> GetProductByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponseDto<List<Product>>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);
    Task<ApiResponseDto<Product>> UpdateProductAsync(int id, ApiRequestDto<Product> request, CancellationToken cancellationToken = default);
    Task<ApiResponseDto<bool>> DeleteProductAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponseDto<List<Product>>> GetDeletedProductsAsync(CancellationToken cancellationToken = default);
    Task<ApiResponseDto<bool>> RestoreProductAsync(int id, CancellationToken cancellationToken = default);
}
