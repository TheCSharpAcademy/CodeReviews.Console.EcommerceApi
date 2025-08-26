using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public interface ICategoryService
{
    Task<PagedResponse<List<CategoryDto>>> GetPagedCategories(PaginationParams paginationParams);

    Task<BaseResponse<Category>> GetCategoryById(int id);

    Task<BaseResponse<CategoryDto>> CreateCategory(WriteCategoryDto category);

    Task<BaseResponse<Category>> UpdateCategory(int id, WriteCategoryDto category);

    Task<BaseResponse<Category>> DeleteCategory(int id);
}

