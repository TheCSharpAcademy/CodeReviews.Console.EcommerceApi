using Microsoft.EntityFrameworkCore;
using Sills.GolfShop.eCommerceAPI.Data;
using Sills.GolfShop.eCommerceAPI.Models;
using Sills.GolfShop.eCommerceAPI.Services;

namespace Sills.GolfShop.eCommerceAPI.Services;


    public interface ICategoryService
    {
        IQueryable<Categories> GetAllCategoriesQuery();
        Task<Categories> GetCategoryByIdAsync(int id);
        Task<Categories> CreateCategoryAsync(Categories category);
        Task UpdateCategoryAsync(int id, Categories category);
        Task DeleteCategoryAsync(int id);
        Task <List<Categories>> GetPagedCategoriesAsync(int pageNumber, int pageSize);
}

public class CategoryService : ICategoryService
{
    private readonly GolfShopDbContext _context;

    public CategoryService(GolfShopDbContext context)
    {
        _context = context;
    }

    public IQueryable<Categories> GetAllCategoriesQuery()
    {
        return _context.Categories.Where(c => c.DeletedAt == null);       
    }

    public async Task<Categories> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories
            .Where(c => c.DeletedAt == null)
            .FirstOrDefaultAsync(c => c.Id == id);
            
    }

    public async Task<Categories> CreateCategoryAsync(Categories category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task UpdateCategoryAsync(int id, Categories category)
    {
        var existingCategory = await _context.Categories.FindAsync(id);
        if (existingCategory == null)
        {
            return;
        }
        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return;
        }
        category.DeletedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task<List<Categories>> GetPagedCategoriesAsync(int pageNumber, int pageSize)
    {
        return await _context.Categories
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}


