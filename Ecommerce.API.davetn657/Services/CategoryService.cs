using Ecommerce.API.davetn657.Data;
using Ecommerce.API.davetn657.Data.Dtos;
using Ecommerce.API.davetn657.Models;

namespace Ecommerce.API.davetn657.Services;

public interface ICategoryService
{
    public PagedResponse<CategoriesDto> AllCategories(PaginationParams pagination);
    public CategoriesDto? CategoryById(int id);
    public CategoriesDto CreateCategory(CategoriesDto category);
    public string? DeleteCategory(int id);
}

public class CategoryService : ICategoryService
{
    private readonly DatabaseContext _dbContext;

    public CategoryService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public PagedResponse<CategoriesDto> AllCategories(PaginationParams pagination)
    {
        var query = _dbContext.Categories.AsQueryable();
        var totalRecords = query.Count();
        var categories = query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(c => new CategoriesDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList()
            .ToList();

        var pagedResponse = new PagedResponse<CategoriesDto>(categories, pagination.PageNumber, pagination.PageSize, totalRecords);

        return pagedResponse;
    }
    public CategoriesDto? CategoryById(int id)
    {
        var savedCategory = _dbContext.Categories
            .Where(c => c.Id == id)
            .Select(c => new CategoriesDto
            {
                Id = c.Id,
                Name = c.Name
            }).FirstOrDefault();

        if (savedCategory == null) return null;
        return savedCategory;
    }
    public CategoriesDto CreateCategory(CategoriesDto category)
    {
        var savedCategory = new Category
        {
            Id = category.Id,
            Name = category.Name
        };
            
        _dbContext.Categories.Add(savedCategory);
        _dbContext.SaveChanges();

        return new CategoriesDto
        {
            Id = savedCategory.Id,
            Name = savedCategory.Name
        };
    }
    public string? DeleteCategory(int id)
    {
        var savedCategory = _dbContext.Categories.Find(id);
        if (savedCategory == null) return null;

        _dbContext.Categories.Remove(savedCategory);
        _dbContext.SaveChanges();

        return $"Successfully deleted category with id {savedCategory.Id}";
    }
}