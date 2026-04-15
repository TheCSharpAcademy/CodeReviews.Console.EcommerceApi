using Ecommerce.Core.DTOs;
using Ecommerce.Core.DTOs.Product;
using Ecommerce.Core.Utilities;

namespace Ecommerce.Core.Interfaces.Services;

public interface IProductService
{
    Task<Result<ProductDto>> CreateProductAsync(CreateProductDto product);
    Task<Result<ProductDto>> GetProductAsync(int id);
    Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync(PaginationParams paginationParams);
    Task<Result<ProductDto>> UpdateProductAsync(int id, UpdateProductDto product);
    Task<Result<bool>> DeleteProductAsync(int id);
}