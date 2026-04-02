using ECommerce.Api.TerrenceLGee.Data;
using ECommerce.Api.TerrenceLGee.Repositories.Helpers;
using ECommerce.Contracts.TerrenceLGee.Common.Extensions;
using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.TerrenceLGee.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ECommerceDbContext _context;
    private readonly ILogger<CategoryRepository> _logger;
    private string _errorMessage = string.Empty;

    public CategoryRepository(
        ECommerceDbContext context,
        ILogger<CategoryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Category?> AddCategoryAsync(Category category)
    {
        try
        {
            var categoryName = category.Name.ToLower();

            var categoryAlreadyExists = await _context.Categories
                .AnyAsync(c => c.Name.ToLower().Equals(categoryName));

            if (categoryAlreadyExists) return null;

            category.CreatedAt = DateTime.UtcNow;

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryRepository)}\n" +
                $"Method: {nameof(AddCategoryAsync)}\n" +
                $"There was an unexpected error adding a new category: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<Category?> UpdateCategoryAsync(Category category)
    {
        try
        {
            var categoryToUpdate = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == category.Id);

            if (categoryToUpdate is null) return null;

            categoryToUpdate.Name = category.Name;
            categoryToUpdate.Description = category.Description;
            categoryToUpdate.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return categoryToUpdate;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryRepository)}\n" +
                $"Method: {nameof(UpdateCategoryAsync)}\n" +
                $"There was an unexpected error updating category {category.Id}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<Category?> GetCategoryAsync(int categoryId)
    {
        try
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            return category;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryRepository)}\n" +
                $"Method: {nameof(GetCategoryAsync)}\n" +
                $"There was an unexpected error retrieving category {categoryId}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<PagedList<Category>> GetCategoriesAsync(CategoryQueryParams categoryQueryParams)
    {
        try
        {
            var categories = _context.Categories
                .AsNoTracking();

            SetFilteringAndSorting(ref categories, categoryQueryParams);

            return await categories.ToPagedListAsync(categories.Count(), categoryQueryParams.Page, categoryQueryParams.PageSize);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(CategoryRepository)}\n" +
                $"Method: {nameof(GetCategoriesAsync)}\n" +
                $"There was an unexpected error retrieving the categories: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }
    private static void SetFilteringAndSorting(ref IQueryable<Category> categories, CategoryQueryParams categoryQueryParams)
    {
        if (!string.IsNullOrEmpty(categoryQueryParams.Description))
        {
            categories = categories.Where(c => !string.IsNullOrEmpty(c.Description)
            && c.Description.ToLower().Contains(categoryQueryParams.Description.ToLower()));
        }

        categories = SortHelper<Category>.ApplySorting(categories, categoryQueryParams.OrderBy);
    }
}
