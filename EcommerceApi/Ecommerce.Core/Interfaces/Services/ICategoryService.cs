using Ecommerce.Core.DTOs.Category;
using Ecommerce.Core.Utilities;

namespace Ecommerce.Core.Interfaces.Services;

public interface ICategoryService
{
    Task<Result<CategoryDto>> CreateCategoryAsync(CreateCategoryDto category);
    Task<Result<CategoryDto>> GetCategoryAsync(int id);
    Task<Result<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();
    Task<Result<CategoryDto>> UpdateCategoryAsync(int id, UpdateCategoryDto category);
    Task<Result<bool>> DeleteCategoryAsync(int id);
}