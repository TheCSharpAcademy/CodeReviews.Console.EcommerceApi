using JafnaEcommerceApi.Models.DTOs.ProductDTOs;
using JafnaEcommerceApi.Models.DTOs.PaginationDTOs;

namespace JafnaEcommerceApi.Services;

public interface IProductService
{
    Task CreateAsync(ProductCreateDto productCreateDto);
    Task UpdateAsync(int categoryId, ProductUpdateDto productUpdateDto);
    Task DeleteAsync(int productId);
    Task<PagedResponse<ProductDto>> GetAllAsync(int pageNumber, int pageSize);
    Task<PagedResponse<ProductDto>> GetProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize);
}
