using AutoMapper;
using JafnaEcommerceApi.Models.Entities;

namespace JafnaEcommerceApi.Repositories;

public interface IProductRepository
{
    Task CreateAsync(Product productEntity);
    Task UpdateAsync(Product productEntity);
    Task DeleteAsync(Product productEntity);
    IQueryable<Product> GetProductQuery();
    Task<Product> GetByIdAsync(int productId);
    Task<Product> GetNameAsync(string name);
    IQueryable<Product> GetCategoryProductQuery(int categoryId);
    Task<List<Product>> GetProductsByIdsAsync(List<int> SalesProductIds);
}
