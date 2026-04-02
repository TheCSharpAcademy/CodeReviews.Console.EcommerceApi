using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;

public interface IProductRepository
{
    Task<Product?> AddProductAsync(Product product);
    Task<Product?> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int productId);
    Task<bool> RestoreProductAsync(int productId);
    Task<Product?> GetProductAsync(int productId);
    Task<PagedList<Product>> GetProductsAsync(ProductQueryParams productQueryParams);
}
