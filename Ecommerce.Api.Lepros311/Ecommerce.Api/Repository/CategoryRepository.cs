using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly EcommerceDbContext _dbContext;

    public CategoryRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<List<Category>>> GetPagedCategories(PaginationParams paginationParams)
    {
        var response = new PagedResponse<List<Category>>(data: new List<Category>(),
                                                               pageNumber: paginationParams.Page,
                                                               pageSize: paginationParams.PageSize,
                                                               totalRecords: 0);
        try
        {
            var query = _dbContext.Categories.Include(c => c.Products).AsQueryable();

            if (!string.IsNullOrEmpty(paginationParams.CategoryName))
                query = query.Where(c => c.CategoryName.Contains(paginationParams.CategoryName));

            var sortBy = paginationParams.SortBy?.Trim().ToLower() ?? "categoryid";
            var sortAscending = paginationParams.SortAscending;

            bool useAscending = sortAscending ?? (sortBy == "categoryid" ? false : true);

            query = sortBy switch
            {
                "categoryname" => useAscending ? query.OrderBy(c => c.CategoryName) : query.OrderByDescending(c => c.CategoryName),
                _ => useAscending ? query.OrderBy(c => c.CategoryId) : query.OrderByDescending(c => c.CategoryId)
            };

            var pagedCategories = await query
                                    .Skip((paginationParams.Page - 1) * paginationParams.PageSize)
                                    .Take(paginationParams.PageSize)
                                    .ToListAsync();

            response.Status = ResponseStatus.Success;
            response.Data = pagedCategories;
        }
        catch (Exception ex)
        {
            response.Message = $"Error in CategoryRepository {nameof(GetPagedCategories)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Category>> GetCategoryById(int id)
    {
        var response = new BaseResponse<Category>();

        try
        {
            var category = await _dbContext.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Category not found.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = category;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in CategoryRepository {nameof(CategoryRepository)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Category>> CreateCategory(Category category)
    {
        var response = new BaseResponse<Category>();

        try
        {
            _dbContext.Categories.Add(category);

            await _dbContext.SaveChangesAsync();

            if (category == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Category not created.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = category;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in CategoryRepository {nameof(CreateCategory)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Category>> UpdateCategory(Category updatedCategory)
    {
        var response = new BaseResponse<Category>();

        try
        {
            _dbContext.Categories.Update(updatedCategory);
            var affectedRows = await _dbContext.SaveChangesAsync();

            if (affectedRows == 0)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "No changes were saved.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = updatedCategory;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in CategoryRepository {nameof(UpdateCategory)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Category>> DeleteCategory(int id)
    {
        var response = new BaseResponse<Category>();

        try
        {
            response = await GetCategoryById(id);

            response.Data.IsDeleted = true;

            _dbContext.Categories.Update(response.Data);
            var affectedRows = await _dbContext.SaveChangesAsync();

            if (affectedRows == 0)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Deletion failed.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Message = "Category deleted.";
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in CategoryRepository {nameof(DeleteCategory)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }
}
