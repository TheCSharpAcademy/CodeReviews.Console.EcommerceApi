using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;

public interface ICategoryService
{
    Task<CategoryAdminData?> AddCategoryAsync(CreateCategoryDto category);
    Task<CategoryAdminData?> UpdateCategoryAsync(UpdateCategoryDto category);
    Task<CategoryData?> GetCategoryAsync(int id);
    Task<CategoryAdminData?> GetCategoryForAdminAsync(int id);
    Task<CategoriesRoot?> GetCategoriesAsync(CategoryQueryParams queryParams);
    Task<CategoriesAdminRoot?> GetCategoriesForAdminAsync(CategoryQueryParams queryParams);
}
