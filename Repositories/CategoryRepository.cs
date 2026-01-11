using System.Net;
using ECommerceApp.RyanW84.Data;
using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.RyanW84.Repositories;

public class CategoryRepository(ECommerceDbContext db) : ICategoryRepository
{
    private readonly ECommerceDbContext _db = db;

    public async Task<bool> CategoryExistsAsync(
        string categoryName,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(categoryName))
            return false;
        return await CompiledQueries.CategoryExistsByName(_db, categoryName, cancellationToken);
    }

    public async Task<ApiResponseDto<Category?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            Category? category = await CompiledQueries.GetCategoryByIdWithRelations(
                _db,
                id,
                cancellationToken
            );

            return new ApiResponseDto<Category?>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.OK,
                ErrorMessage = string.Empty,
                Data = category,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponseDto<Category?>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.InternalServerError,
                ErrorMessage = ex.Message,
                Data = null,
            };
        }
    }

    public async Task<ApiResponseDto<Category?>> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            Category? category = await _db
                .Categories.AsNoTracking()
                .AsSplitQuery()
                .Include(c => c.Products)
                .Include(c => c.Sales)
                .FirstOrDefaultAsync(c => c.Name == name, cancellationToken);

            return new ApiResponseDto<Category?>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.OK,
                ErrorMessage = string.Empty,
                Data = category,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponseDto<Category?>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.InternalServerError,
                ErrorMessage = ex.Message,
                Data = null,
            };
        }
    }

    public async Task<PaginatedResponseDto<List<Category>>> GetAllCategoriesAsync(
        CategoryQueryParameters parameters,
        CancellationToken cancellationToken = default
    )
    {
        var (page, pageSize) = NormalizePageParameters(parameters);

        try
        {
            var query = BuildCategoryQuery(parameters);
            var (totalCount, categories) = await FetchCategoriesPageAsync(
                query,
                page,
                pageSize,
                cancellationToken
            );
            return BuildSuccessResponse(categories, page, pageSize, totalCount);
        }
        catch (Exception ex)
        {
            return BuildFailureResponse(ex.Message, page, pageSize);
        }
    }

    private static (int Page, int PageSize) NormalizePageParameters(
        CategoryQueryParameters parameters
    )
    {
        var page = Math.Max(parameters.Page, 1);
        var pageSize = Math.Clamp(parameters.PageSize, 1, 32);
        return (page, pageSize);
    }

    private static async Task<(int TotalCount, List<Category> Categories)> FetchCategoriesPageAsync(
        IQueryable<Category> query,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    )
    {
        var totalCount = await query.CountAsync(cancellationToken);
        var categories = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (totalCount, categories);
    }

    private static PaginatedResponseDto<List<Category>> BuildSuccessResponse(
        List<Category> categories,
        int page,
        int pageSize,
        int totalCount
    ) =>
        new()
        {
            RequestFailed = false,
            ResponseCode = HttpStatusCode.OK,
            ErrorMessage = string.Empty,
            Data = categories,
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = totalCount,
        };

    private static PaginatedResponseDto<List<Category>> BuildFailureResponse(
        string errorMessage,
        int page,
        int pageSize
    ) =>
        new()
        {
            RequestFailed = true,
            ResponseCode = HttpStatusCode.InternalServerError,
            ErrorMessage = errorMessage,
            Data = [],
            CurrentPage = page,
            PageSize = pageSize,
            TotalCount = 0,
        };

    private IQueryable<Category> BuildCategoryQuery(CategoryQueryParameters parameters)
    {
        var query = GetBaseQuery();
        if (parameters.IncludeDeleted)
            query = query.IgnoreQueryFilters();
        query = ApplySearchFilter(query, parameters.Search);
        var descending = string.Equals(
            parameters.SortDirection,
            "desc",
            StringComparison.OrdinalIgnoreCase
        );
        query = ApplyCategorySorting(
            query,
            parameters.SortBy?.Trim().ToLowerInvariant(),
            descending
        );
        return query;
    }

    private static IQueryable<Category> ApplyCategorySorting(
        IQueryable<Category> query,
        string? sortBy,
        bool descending
    )
    {
        return sortBy switch
        {
            "name" => descending
                ? query.OrderByDescending(c => c.Name)
                : query.OrderBy(c => c.Name),
            "createdat" => descending
                ? query.OrderByDescending(c => c.CreatedAt)
                : query.OrderBy(c => c.CreatedAt),
            _ => descending
                ? query.OrderByDescending(c => c.CategoryId)
                : query.OrderBy(c => c.CategoryId),
        };
    }

    private static ApiResponseDto<T> CreateErrorResponse<T>(HttpStatusCode code, string message)
    {
        return new ApiResponseDto<T>
        {
            RequestFailed = true,
            ResponseCode = code,
            ErrorMessage = message,
            Data = default,
        };
    }

    private IQueryable<Category> GetBaseQuery()
    {
        return _db.Categories
            .TagWith("CategoryRepository.GetBaseQuery")
            .AsNoTracking()
            .Include(c => c.Products);
    }

    private IQueryable<Category> ApplySearchFilter(IQueryable<Category> query, string? search)
    {
        if (string.IsNullOrEmpty(search?.Trim()))
            return query;
        var pattern = $"%{search.Trim()}%";
        return query.Where(c =>
            EF.Functions.Like(c.Name, pattern) || EF.Functions.Like(c.Description, pattern)
        );
    }

    public async Task<ApiResponseDto<Category>> AddAsync(
        Category entity,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await _db.Categories.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return new ApiResponseDto<Category>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.Created,
                ErrorMessage = string.Empty,
                Data = entity,
            };
        }
        catch (DbUpdateConcurrencyException)
        {
            return new ApiResponseDto<Category>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.Conflict,
                ErrorMessage = "Concurrency conflict occurred while adding the category.",
                Data = entity,
            };
        }
        catch (DbUpdateException)
        {
            return new ApiResponseDto<Category>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.BadRequest,
                ErrorMessage = "Failed to add category. Please check the data and try again.",
                Data = entity,
            };
        }
        catch (Exception)
        {
            return new ApiResponseDto<Category>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.InternalServerError,
                ErrorMessage = "An unexpected error occurred while adding the category.",
                Data = entity,
            };
        }
    }

    public async Task<ApiResponseDto<Category>> UpdateAsync(
        Category entity,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            _db.Categories.Update(entity);
            await _db.SaveChangesAsync(cancellationToken);

            return new ApiResponseDto<Category>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.OK,
                ErrorMessage = string.Empty,
                Data = entity,
            };
        }
        catch (DbUpdateConcurrencyException)
        {
            return new ApiResponseDto<Category>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.Conflict,
                ErrorMessage = "Concurrency conflict occurred while updating the category.",
                Data = entity,
            };
        }
        catch (DbUpdateException)
        {
            return new ApiResponseDto<Category>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.BadRequest,
                ErrorMessage = "Failed to update category. Please check the data and try again.",
                Data = entity,
            };
        }
        catch (Exception)
        {
            return new ApiResponseDto<Category>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.InternalServerError,
                ErrorMessage = "An unexpected error occurred while updating the category.",
                Data = entity,
            };
        }
    }

    public async Task<ApiResponseDto<bool>> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            Category? category = await _db.Categories.FindAsync(
                new object[] { id },
                cancellationToken
            );
            if (category == null)
                return CreateErrorResponse<bool>(HttpStatusCode.NotFound, "Category not found");

            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(cancellationToken);
            return new ApiResponseDto<bool>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.NoContent,
                ErrorMessage = string.Empty,
                Data = true,
            };
        }
        catch (DbUpdateConcurrencyException)
        {
            return CreateErrorResponse<bool>(
                HttpStatusCode.Conflict,
                "Concurrency conflict occurred while deleting the category."
            );
        }
        catch (DbUpdateException)
        {
            return CreateErrorResponse<bool>(
                HttpStatusCode.BadRequest,
                "Failed to delete category. Please try again."
            );
        }
        catch (Exception)
        {
            return CreateErrorResponse<bool>(
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred while deleting the category."
            );
        }
    }

    public async Task<ApiResponseDto<bool>> RestoreAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            Category? category = await _db
                .Categories.IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.CategoryId == id, cancellationToken);
            if (category == null)
                return CreateErrorResponse<bool>(HttpStatusCode.NotFound, "Category not found");
            if (!category.IsDeleted)
                return CreateErrorResponse<bool>(
                    HttpStatusCode.BadRequest,
                    "Category is not deleted"
                );
            category.IsDeleted = false;
            category.DeletedAt = null;
            await _db.SaveChangesAsync(cancellationToken);
            return new ApiResponseDto<bool>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.OK,
                ErrorMessage = string.Empty,
                Data = true,
            };
        }
        catch (Exception ex)
        {
            return CreateErrorResponse<bool>(
                HttpStatusCode.InternalServerError,
                $"Failed to restore category: {ex.Message}"
            );
        }
    }

    public async Task<ApiResponseDto<List<Category>>> GetDeletedCategoriesAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            List<Category> deletedCategories = await _db
                .Categories.IgnoreQueryFilters() // Include soft-deleted items
                .Where(c => c.IsDeleted)
                .Include(c => c.Products)
                .Include(c => c.Sales)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return new ApiResponseDto<List<Category>>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.OK,
                ErrorMessage = string.Empty,
                Data = deletedCategories,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponseDto<List<Category>>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.InternalServerError,
                ErrorMessage = ex.Message,
                Data = null,
            };
        }
    }

    public async Task<ApiResponseDto<bool>> HardDeleteAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            Category? category = await _db
                .Categories.IgnoreQueryFilters() // Include soft-deleted items
                .FirstOrDefaultAsync(c => c.CategoryId == id, cancellationToken);

            if (category == null)
            {
                return new ApiResponseDto<bool>
                {
                    RequestFailed = true,
                    ResponseCode = HttpStatusCode.NotFound,
                    ErrorMessage = "Category not found",
                    Data = false,
                };
            }

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync(cancellationToken);
            return new ApiResponseDto<bool>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.NoContent,
                ErrorMessage = string.Empty,
                Data = true,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponseDto<bool>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.InternalServerError,
                ErrorMessage = ex.Message,
                Data = false,
            };
        }
    }
}
