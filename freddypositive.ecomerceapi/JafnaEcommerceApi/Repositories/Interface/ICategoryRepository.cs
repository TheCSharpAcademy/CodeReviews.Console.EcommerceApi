using JafnaEcommerceApi.Models.Entities;

namespace JafnaEcommerceApi.Repositories;

public interface ICategoryRepository
{
    Task Create(Category categoryEntity);
    Task Update(Category categoryEntity);
    Task Delete(Category categoryEntity);
    IQueryable<Category> GetCategoryQuery();
    Task<Category> GetByIdAsync(int categoryId);
    Task<Category> GetByNameAsync(string name);
}
