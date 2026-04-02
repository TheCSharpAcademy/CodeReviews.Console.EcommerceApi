using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;

public interface ICategoryRepository
{
    Task<Category?> AddCategoryAsync(Category category);
    Task<Category?> UpdateCategoryAsync(Category category);
    Task<Category?> GetCategoryAsync(int categoryId);
    Task<PagedList<Category>> GetCategoriesAsync(CategoryQueryParams categoryQueryParams);
}
