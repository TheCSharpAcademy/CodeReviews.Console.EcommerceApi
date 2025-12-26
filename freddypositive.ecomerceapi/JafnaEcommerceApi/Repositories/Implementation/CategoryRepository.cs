using JafnaEcommerceApi.Data;
using JafnaEcommerceApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace JafnaEcommerceApi.Repositories;

public class CategoryRepository : ICategoryRepository
{
    public readonly JafnaDbContext _jaffnaDbContext;

    public CategoryRepository(JafnaDbContext jaffnaDbContext)
    {
        _jaffnaDbContext = jaffnaDbContext;
    }

    public async Task Create(Category categoryEntity)
    {
        // repository should not perform validation - service handles that
        _jaffnaDbContext.Add(categoryEntity);
        await _jaffnaDbContext.SaveChangesAsync();
    }

    // changed: accept entity and persist (service will map into existing entity)
    public async Task Update(Category categoryEntity)
    {
        _jaffnaDbContext.category.Update(categoryEntity);
        await _jaffnaDbContext.SaveChangesAsync();
    }
    public async Task Delete(Category categoryEntity)
    {
        _jaffnaDbContext.category.Update(categoryEntity);
        await _jaffnaDbContext.SaveChangesAsync();
    }

    public IQueryable<Category> GetCategoryQuery()
    {
        return _jaffnaDbContext.category.Where(c => !c.IsDeleted);
    }

    public async Task<Category> GetByIdAsync(int categoryId)
    {
        return await _jaffnaDbContext.category
            .FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsDeleted);
    }

    public async Task<Category> GetByNameAsync(string name)
    {
        return await _jaffnaDbContext.category
            .FirstOrDefaultAsync(c => c.Name == name && !c.IsDeleted);
    }
}
