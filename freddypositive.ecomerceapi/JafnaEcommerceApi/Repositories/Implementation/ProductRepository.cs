using JafnaEcommerceApi.Data;
using JafnaEcommerceApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JafnaEcommerceApi.Repositories;

public class ProductRepository : IProductRepository
{

    public readonly JafnaDbContext _jaffnaDbContext;

    public ProductRepository(JafnaDbContext jaffnaDbContext)
    {
        _jaffnaDbContext = jaffnaDbContext;
    }
    public async Task CreateAsync(Product product)
    {
        _jaffnaDbContext.Add(product);

        await _jaffnaDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        await _jaffnaDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Product productEntity)
    {
        await _jaffnaDbContext.SaveChangesAsync();
    }
    public async Task<List<Product>> GetProductsByIdsAsync(List<int> productIds)
    {
        return await _jaffnaDbContext.products.Where(x => productIds.Contains(x.Id) && !x.IsDeleted).ToListAsync();
    }

    public IQueryable<Product> GetProductQuery()
    {
        return _jaffnaDbContext.products.Where(c => !c.IsDeleted);
    }
    public IQueryable<Product> GetCategoryProductQuery(int categoryId)
    {
        return _jaffnaDbContext.products.Where(c => c.CategoryId == categoryId && !c.IsDeleted);
    }
    public async Task<Product> GetByIdAsync(int id)
    {
        return await _jaffnaDbContext.products
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }
    public async Task<Product> GetNameAsync(string name)
    {
        return await _jaffnaDbContext.products.
            FirstOrDefaultAsync(c => c.Name == name && c.IsDeleted == false);
    }


}
