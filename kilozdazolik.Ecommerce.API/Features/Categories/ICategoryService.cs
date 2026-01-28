namespace kilozdazolik.Ecommerce.API.Features.Categories;

public interface ICategoryService
{
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto category);
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync(CategoryParameters parameters);
    Task UpdateCategoryAsync(Guid id, UpdateCategoryDto category);
    Task DeleteCategoryAsync(Guid id);
    Task RestoreCategoryAsync(Guid id);
}