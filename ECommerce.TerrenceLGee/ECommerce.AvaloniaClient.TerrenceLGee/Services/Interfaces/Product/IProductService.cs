using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;

public interface IProductService
{
    Task<ProductAdminData?> AddProductAsync(CreateProductDto product);
    Task<ProductAdminData?> UpdateProductAsync(UpdateProductDto product);
    Task<(bool, string?)> DeleteProductAsync(int productId);
    Task<(bool, string?)> RestoreProductAsync(int productId);
    Task<ProductAdminData?> GetProductForAdminAsync(int productId);
    Task<ProductData?> GetProductAsync(int productId);
    Task<ProductsAdminRoot?> GetProductsForAdminAsync(ProductQueryParams queryParams);
    Task<ProductsRoot?> GetProductsAsync(ProductQueryParams queryParams);
}
