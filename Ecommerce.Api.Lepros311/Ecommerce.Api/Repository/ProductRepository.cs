using Ecommerce.Api.Models;
using Ecommerce.Api.Data;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Repository;

public class ProductRepository : IProductRepository
{
    private readonly EcommerceDbContext _dbContext;

    public ProductRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<List<Product>>> GetPagedProducts(PaginationParams paginationParams)
    {
        var response = new PagedResponse<List<Product>>(data: new List<Product>(),
                                                       pageNumber: paginationParams.Page,
                                                       pageSize: paginationParams.PageSize,
                                                       totalRecords: 0);
      
        try
        {
            var query = _dbContext.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(paginationParams.ProductName))
                query = query.Where(p => p.ProductName.Contains(paginationParams.ProductName));
            if (!string.IsNullOrEmpty(paginationParams.CategoryName))
                query = query.Where(p => p.Category.CategoryName.Contains(paginationParams.CategoryName));
            if (paginationParams.MinPrice.HasValue)
                query = query.Where(p => p.Price >= paginationParams.MinPrice.Value);
            if (paginationParams.MaxPrice.HasValue)
                query = query.Where(p => p.Price <= paginationParams.MaxPrice.Value);            

            var sortBy = paginationParams.SortBy?.Trim().ToLower() ?? "productid";
            var sortAscending = paginationParams.SortAscending;

            bool useAscending = sortAscending ?? (sortBy == "productid" ? false : true);

            query = sortBy switch
            {
                "productname" => useAscending ? query.OrderBy(p => p.ProductName) : query.OrderByDescending(p => p.ProductName),
                "categoryname" => useAscending ? query.OrderBy(p => p.Category.CategoryName) : query.OrderByDescending(p => p.Category.CategoryName),
                "price" => useAscending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
                _ => useAscending ? query.OrderBy(p => p.ProductId) : query.OrderByDescending(p => p.ProductId)
            };

            var pagedProducts = await query.Skip((paginationParams.Page - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            response.Status = ResponseStatus.Success;
            response.Data = pagedProducts;
        }
        catch (Exception ex)
        {
            response.Message = $"Error in ProductRepository {nameof(GetPagedProducts)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }
        
        return response;
    }

    public async Task<List<int>> GetAllProductIds()
    {
        var productIds = await _dbContext.Products
            .AsNoTracking()
            .Select(p => p.ProductId)
            .ToListAsync();

        return productIds;
    }

    public async Task<List<Product>> GetProductsByIds(IEnumerable<int> productIds)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.Category)
            .Where(p => productIds.Contains(p.ProductId))
            .ToListAsync();
    }


    public async Task<BaseResponse<Product>> GetProductById(int id)
    {
        var response = new BaseResponse<Product>();

        try
        {
            var product = await _dbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Product not found.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = product;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in ProductRepository {nameof(GetProductById)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Product>> CreateProduct(Product product)
    {
        var response = new BaseResponse<Product>();

        try
        {
            _dbContext.Products.Add(product);

            await _dbContext.SaveChangesAsync();

            if (product == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Product not created.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = product;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in ProductRepository {nameof(CreateProduct)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Product>> UpdateProduct(Product updatedProduct)
    {
        var response = new BaseResponse<Product>();

        try
        {
            _dbContext.Products.Update(updatedProduct);
            var affectedRows = await _dbContext.SaveChangesAsync();

            if (affectedRows == 0)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "No changes were saved.";
            }
            else
            {
                await _dbContext.Entry(updatedProduct).Reference(p => p.Category).LoadAsync();

                response.Status = ResponseStatus.Success;
                response.Data = updatedProduct;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in ProductRepository {nameof(UpdateProduct)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Product>> DeleteProduct(int id)
    {
        var response = new BaseResponse<Product>();

        try
        {
            response = await GetProductById(id);

            response.Data.IsDeleted = true;

            _dbContext.Products.Update(response.Data);

            var affectedRows = await _dbContext.SaveChangesAsync();

            if (affectedRows == 0)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Deletion failed.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Message = "Product deleted.";
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in ProductRepository {nameof(DeleteProduct)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }
}
