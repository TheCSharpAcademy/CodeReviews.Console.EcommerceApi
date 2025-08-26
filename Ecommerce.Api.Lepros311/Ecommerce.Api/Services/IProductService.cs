using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public interface IProductService
{
    Task<PagedResponse<List<ProductDto>>> GetPagedProducts(PaginationParams paginationParams);

    Task<BaseResponse<Product>> GetProductById(int id);

    Task<BaseResponse<ProductDto>> CreateProduct(WriteProductDto product);

    Task<BaseResponse<Product>> UpdateProduct(int id, WriteProductDto product);

    Task<BaseResponse<Product>> DeleteProduct(int id);
}
