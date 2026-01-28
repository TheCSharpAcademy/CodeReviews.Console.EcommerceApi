using kilozdazolik.Ecommerce.API.Data;
using kilozdazolik.Ecommerce.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace kilozdazolik.Ecommerce.API.Features.Categories;

public class CategoryService(AppDbContext dbContext) : ICategoryService
{
    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto category)
    {
        ArgumentNullException.ThrowIfNull(category);

        Helper.ValidateName(category.Name, "Category name is required");

        bool exists = await dbContext.Categories
            .AnyAsync(c => c.Name == category.Name && !c.IsDeleted);

        if (exists)
            throw new InvalidOperationException($"A category with name '{category.Name}' already exists.");

        Category newCategory = new()
        {
            Id = Guid.NewGuid(),
            Name = category.Name,
            IsDeleted = false
        };
        
        dbContext.Categories.Add(newCategory);
        await dbContext.SaveChangesAsync();
        
        return new CategoryDto(newCategory.Id, newCategory.Name);
        
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        return await dbContext.Categories
            .Where(c => c.Id == id)
            .Select(c => new CategoryDto(c.Id,  c.Name))
            .FirstOrDefaultAsync();
        
    }

    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync(CategoryParameters parameters)
    {
        var query = dbContext.Categories
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
        {
            query = query.Where(c => c.Name.ToLower().Contains(parameters.SearchTerm.ToLower()));
        }

        query = parameters.SortBy?.ToLower() switch
        {
            "name_desc" => query.OrderByDescending(c => c.Name),
            _ => query.OrderBy(c => c.Name) 
        };

        return await query
            .Skip((parameters.PageIndex - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(c => new CategoryDto(c.Id, c.Name))
            .ToListAsync();
    }

    public async Task UpdateCategoryAsync(Guid id, UpdateCategoryDto dto)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        Helper.ValidateName(dto.Name, "Name is required");

        var category = await dbContext.Categories.FindAsync(id);

        if (category is null)
            throw new InvalidOperationException($"Category with id '{id}' not found");
        
        bool exists = await dbContext.Categories
            .AnyAsync(c => c.Name == dto.Name && c.Id != id && !c.IsDeleted);

        if (exists)
            throw new InvalidOperationException($"A category with name '{dto.Name}' already exists.");

        category.Name = dto.Name;

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        var category = await dbContext.Categories.FindAsync(id);

        if (category is null) throw new KeyNotFoundException($"Category with id {id} not found");

        category.IsDeleted = true;

        await dbContext.SaveChangesAsync();
    }

    public async Task RestoreCategoryAsync(Guid id)
    {
        var category = await dbContext.Categories
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (category is null) throw new KeyNotFoundException($"Category with id {id} not found");

        if (!category.IsDeleted) return;

        category.IsDeleted = false;
        
        await dbContext.SaveChangesAsync();
    }
}