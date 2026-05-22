using EcommerceAPI.Ledana.DTOs;
using EcommerceAPI.Ledana.Models;
using EcommerceAPI.Ledana.Options;

namespace EcommerceAPI.Ledana.Interfaces
{
    public interface IProductService
    {
        Task<ApiResponseDto<Product>> CreateProduct(ProductDto product);
        Task<ApiResponseDto<string?>> SoftDeleteProduct(int id);
        Task<ApiResponseDto<List<Product>?>> GetAllProducts(ProductOptions productOptions);
        Task<ApiResponseDto<List<Product>?>> GetAllProducts();
        Task<ApiResponseDto<Product?>> GetProductById(int id);
        Task<ApiResponseDto<Product>?> UpdateProduct(int id, ProductUpdateDto product);
    }
}
