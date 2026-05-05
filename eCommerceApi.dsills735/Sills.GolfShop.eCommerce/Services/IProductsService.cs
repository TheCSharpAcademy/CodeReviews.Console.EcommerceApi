using Microsoft.EntityFrameworkCore;
using Sills.GolfShop.eCommerceAPI.Data;
using Sills.GolfShop.eCommerceAPI.Models;

namespace Sills.GolfShop.eCommerceAPI.Services;


public interface IProductsService
{
    Task<List<Product>> GetAllProductsAsync();
    Task<Product> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task UpdateProductAsync(int id, Product product);
    Task DeleteProductAsync(int id);
    Task<List<Product>> GetPagedProductsAsync(int pageNumber, int pageSize);
}
public class ProductsService : IProductsService
{
    private readonly GolfShopDbContext _context;

    public ProductsService(GolfShopDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _context.Products
            .Where(p => p.DeletedAt == null)
           .ToListAsync();
   }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        return await _context.Products
            .Where(p => p.DeletedAt == null)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task UpdateProductAsync(int id, Product product)
    {
        var existingProduct = await _context.Products
            .Where(p => p.DeletedAt == null)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (existingProduct == null)
        {
            return;
        }
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.CategoryId = product.CategoryId;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return;
        }
        product.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Product>> GetPagedProductsAsync(int pageNumber, int pageSize)
    {
        return await _context.Products
            .Where(p => p.DeletedAt == null)
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}