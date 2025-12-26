using JafnaEcommerceApi.Models.DTOs.CategoryDTOs;
using JafnaEcommerceApi.Models.DTOs.PaginationDTOs;

namespace JafnaEcommerceApi.Services;

public interface ICategoryService
{
    Task Create(CategoryCreateDto categoryCreateDto);
    Task Update(int categoryId, CategoryUpdateDto categoryUpdateDto);
    Task Delete(int categoryId);
    Task<PagedResponse<CategoryDto>> GetAll(int pageNumber, int pageSize);
}
