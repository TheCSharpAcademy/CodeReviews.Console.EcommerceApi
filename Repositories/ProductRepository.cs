using System.Net;
using ECommerceApp.RyanW84.Data;
using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.RyanW84.Repositories
{
    public class ProductRepository(ECommerceDbContext db) : IProductRepository
    {
        private readonly ECommerceDbContext _db = db;

        public async Task<ApiResponseDto<Product>> AddAsync(
            Product entity,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                await _db.Products.AddAsync(entity, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);

                Product? createdProduct = await GetProductWithCategory(
                    entity.ProductId,
                    cancellationToken
                );

                return new ApiResponseDto<Product>
                {
                    RequestFailed = false,
                    ResponseCode = HttpStatusCode.Created,
                    ErrorMessage = string.Empty,
                    Data = createdProduct,
                };
            }
            catch (DbUpdateException ex)
            {
                return HandleDbUpdateException<Product>(ex);
            }
            catch (Exception ex)
            {
                return CreateErrorResponse<Product>(
                    HttpStatusCode.InternalServerError,
                    $"Failed to create product: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponseDto<Product?>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                Product? product = await CompiledQueries.GetProductByIdWithCategory(
                    _db,
                    id,
                    cancellationToken
                );
                return new ApiResponseDto<Product?>
                {
                    RequestFailed = false,
                    ResponseCode = HttpStatusCode.OK,
                    ErrorMessage = string.Empty,
                    Data = product,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<Product?>
                {
                    RequestFailed = true,
                    ResponseCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message,
                    Data = null,
                };
            }
        }

        public async Task<PaginatedResponseDto<List<Product>>> GetAllProductsAsync(
            ProductQueryParameters? parameters,
            CancellationToken cancellationToken = default
        )
        {
            parameters ??= new ProductQueryParameters();

            var page = Math.Max(parameters.Page, 1);
            var pageSize = Math.Clamp(parameters.PageSize, 1, 100);

            try
            {
                IQueryable<Product> query = GetBaseProductQuery();
                query = ApplyProductFilters(query, parameters);

                var descending = string.Equals(
                    parameters.SortDirection,
                    "desc",
                    StringComparison.OrdinalIgnoreCase
                );
                var sortBy = parameters.SortBy?.Trim().ToLowerInvariant();
                IQueryable<Product> orderedQuery = ApplyProductSorting(query, sortBy, descending);

                var totalCount = await orderedQuery.CountAsync(cancellationToken);
                List<Product> products = await orderedQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(cancellationToken);

                return new PaginatedResponseDto<List<Product>>
                {
                    RequestFailed = false,
                    ResponseCode = HttpStatusCode.OK,
                    ErrorMessage = string.Empty,
                    Data = products,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                };
            }
            catch (Exception ex)
            {
                return new PaginatedResponseDto<List<Product>>
                {
                    RequestFailed = true,
                    ResponseCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message,
                    Data = null,
                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalCount = 0,
                };
            }
        }

        private static IQueryable<Product> ApplyProductSorting(
            IQueryable<Product> query,
            string? sortBy,
            bool descending
        )
        {
            return sortBy switch
            {
                "name" => descending
                    ? query.OrderByDescending(p => p.Name)
                    : query.OrderBy(p => p.Name),
                "price" => descending
                    ? query.OrderByDescending(p => p.Price)
                    : query.OrderBy(p => p.Price),
                "createdat" => descending
                    ? query.OrderByDescending(p => p.CreatedAt)
                    : query.OrderBy(p => p.CreatedAt),
                "stock" => descending
                    ? query.OrderByDescending(p => p.Stock)
                    : query.OrderBy(p => p.Stock),
                "category" => descending
                    ? query.OrderByDescending(p => p.Category!.Name)
                    : query.OrderBy(p => p.Category!.Name),
                _ => descending
                    ? query.OrderByDescending(p => p.ProductId)
                    : query.OrderBy(p => p.ProductId),
            };
        }

        private static ApiResponseDto<T> CreateErrorResponse<T>(
            HttpStatusCode statusCode,
            string errorMessage
        )
        {
            return new ApiResponseDto<T>
            {
                RequestFailed = true,
                ResponseCode = statusCode,
                ErrorMessage = errorMessage,
                Data = default,
            };
        }

        private static ApiResponseDto<T> HandleDbUpdateException<T>(DbUpdateException ex)
        {
            if (
                ex.InnerException?.Message.Contains("FOREIGN KEY") == true
                || ex.InnerException?.Message.Contains("constraint") == true
            )
            {
                return CreateErrorResponse<T>(
                    HttpStatusCode.BadRequest,
                    "Invalid data: Foreign key constraint violation or invalid reference."
                );
            }

            return CreateErrorResponse<T>(
                HttpStatusCode.Conflict,
                "Failed to save product due to data conflict."
            );
        }

        private IQueryable<Product> GetBaseProductQuery()
        {
            return _db
                .Products.TagWith("ProductRepository.GetBaseProductQuery")
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted)
                .AsNoTracking()
                .AsQueryable();
        }

        private static IQueryable<Product> ApplyProductFilters(
            IQueryable<Product> query,
            ProductQueryParameters parameters
        )
        {
            var search = parameters.Search?.Trim();
            if (!string.IsNullOrEmpty(search))
            {
                var likePattern = $"%{search}%";
                query = query.Where(p =>
                    EF.Functions.Like(p.Name, likePattern)
                    || EF.Functions.Like(p.Description, likePattern)
                );
            }

            if (parameters.MinPrice is { } minPrice)
            {
                query = query.Where(p => p.Price >= minPrice);
            }

            if (parameters.MaxPrice is { } maxPrice)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }

            if (parameters.CategoryId is { } categoryId)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            return query;
        }

        private static void UpdateProductProperties(Product existing, Product entity)
        {
            existing.Name = entity.Name;
            existing.Description = entity.Description;
            existing.Price = entity.Price;
            existing.Stock = entity.Stock;
            existing.IsActive = entity.IsActive;
            existing.CategoryId = entity.CategoryId;
        }

        private Task<Product?> GetProductWithCategory(
            int productId,
            CancellationToken cancellationToken
        )
        {
            return CompiledQueries.GetProductByIdWithCategory(_db, productId, cancellationToken);
        }

        private static ApiResponseDto<bool> CheckConstraintViolation(DbUpdateException ex)
        {
            return
                ex.InnerException?.Message.Contains("FOREIGN KEY") == true
                || ex.InnerException?.Message.Contains("constraint") == true
                ? CreateErrorResponse<bool>(
                    HttpStatusCode.Conflict,
                    "Cannot delete product as it is referenced by existing sales."
                )
                : CreateErrorResponse<bool>(
                    HttpStatusCode.InternalServerError,
                    "Failed to delete product due to database error."
                );
        }

        public async Task<ApiResponseDto<List<Product>>> GetProductsByCategoryIdAsync(
            int categoryId,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                List<Product> products = await _db
                    .Products.AsNoTracking()
                    .Where(p => p.CategoryId == categoryId)
                    .Include(p => p.Category)
                    .ToListAsync(cancellationToken);
                return new ApiResponseDto<List<Product>>
                {
                    RequestFailed = false,
                    ResponseCode = HttpStatusCode.OK,
                    ErrorMessage = string.Empty,
                    Data = products,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<List<Product>>
                {
                    RequestFailed = true,
                    ResponseCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message,
                    Data = null,
                };
            }
        }

        public async Task<ApiResponseDto<Product>> UpdateAsync(
            Product entity,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                Product? existing = await _db
                    .Products.Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == entity.ProductId, cancellationToken);

                if (existing == null)
                    return CreateErrorResponse<Product>(
                        HttpStatusCode.NotFound,
                        "Product not found"
                    );

                UpdateProductProperties(existing, entity);
                await _db.SaveChangesAsync(cancellationToken);

                Product? updatedProduct = await GetProductWithCategory(
                    existing.ProductId,
                    cancellationToken
                );

                return new ApiResponseDto<Product>
                {
                    RequestFailed = false,
                    ResponseCode = HttpStatusCode.OK,
                    ErrorMessage = string.Empty,
                    Data = updatedProduct,
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                return CreateErrorResponse<Product>(
                    HttpStatusCode.Conflict,
                    "Product was modified by another user. Please refresh and try again."
                );
            }
            catch (DbUpdateException ex)
            {
                return HandleDbUpdateException<Product>(ex);
            }
            catch (Exception ex)
            {
                return CreateErrorResponse<Product>(
                    HttpStatusCode.InternalServerError,
                    $"Failed to update product: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponseDto<bool>> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                Product? product = await _db.Products.FindAsync(
                    new object[] { id },
                    cancellationToken
                );
                if (product == null)
                    return CreateErrorResponse<bool>(HttpStatusCode.NotFound, "Product not found");

                product.IsDeleted = true;
                product.DeletedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(cancellationToken);
                return new ApiResponseDto<bool>
                {
                    RequestFailed = false,
                    ResponseCode = HttpStatusCode.NoContent,
                    ErrorMessage = string.Empty,
                    Data = true,
                };
            }
            catch (DbUpdateException ex)
            {
                return CheckConstraintViolation(ex);
            }
            catch (Exception ex)
            {
                return CreateErrorResponse<bool>(
                    HttpStatusCode.InternalServerError,
                    $"Failed to delete product: {ex.Message}"
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
                Product? product = await _db
                    .Products.IgnoreQueryFilters()
                    .FirstOrDefaultAsync(p => p.ProductId == id, cancellationToken);
                if (product == null)
                    return CreateErrorResponse<bool>(HttpStatusCode.NotFound, "Product not found");
                if (!product.IsDeleted)
                    return CreateErrorResponse<bool>(
                        HttpStatusCode.BadRequest,
                        "Product is not deleted"
                    );

                product.IsDeleted = false;
                product.DeletedAt = null;
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
                    $"Failed to restore product: {ex.Message}"
                );
            }
        }

        public async Task<ApiResponseDto<List<Product>>> GetDeletedProductsAsync(
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                List<Product> deletedProducts = await _db
                    .Products.IgnoreQueryFilters() // Include soft-deleted items
                    .Where(p => p.IsDeleted)
                    .Include(p => p.Category)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                return new ApiResponseDto<List<Product>>
                {
                    RequestFailed = false,
                    ResponseCode = HttpStatusCode.OK,
                    ErrorMessage = string.Empty,
                    Data = deletedProducts,
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseDto<List<Product>>
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
                Product? product = await _db
                    .Products.IgnoreQueryFilters() // Include soft-deleted items
                    .FirstOrDefaultAsync(p => p.ProductId == id, cancellationToken);

                if (product == null)
                {
                    return new ApiResponseDto<bool>
                    {
                        RequestFailed = true,
                        ResponseCode = HttpStatusCode.NotFound,
                        ErrorMessage = "Product not found",
                        Data = false,
                    };
                }

                _db.Products.Remove(product);
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
}
