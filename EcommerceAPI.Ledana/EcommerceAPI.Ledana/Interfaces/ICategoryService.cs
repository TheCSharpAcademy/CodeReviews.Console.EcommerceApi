using EcommerceAPI.Ledana.DTOs;
using EcommerceAPI.Ledana.Models;

namespace EcommerceAPI.Ledana.Interfaces
{
    public interface ICategoryService
    {
        
        Task<ApiResponseDto<List<Category>>> GetAllCategories();
        Task<ApiResponseDto<Category>> GetCategoryById(int id);
        Task<ApiResponseDto<Category>> CreateCategory(CategoryDto category);
        Task<ApiResponseDto<Category>> UpdateCategory(int id, CategoryDto category);
        Task<ApiResponseDto<string?>> SoftDeleteCategory(int id);
        
    }
}
