using Ecommerce.Api.Models;
using Ecommerce.Api.Data;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Repository;

public class LineItemRepository : ILineItemRepository
{
    private readonly EcommerceDbContext _dbContext;

    public LineItemRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<List<LineItem>>> GetPagedLineItems(PaginationParams paginationParams)
    {
        var response = new PagedResponse<List<LineItem>>(data: new List<LineItem>(),
                                                               pageNumber: paginationParams.Page,
                                                               pageSize: paginationParams.PageSize,
                                                               totalRecords: 0);
        try
        {
            var query = _dbContext.LineItems
                .Include(li => li.Sale)
                .Include(li => li.Product)
                .ThenInclude(li => li.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(paginationParams.ProductName))
                query = query.Where(li => li.Product.ProductName.Contains(paginationParams.ProductName));
            if (!string.IsNullOrEmpty(paginationParams.CategoryName))
                query = query.Where(li => li.Product.Category.CategoryName.Contains(paginationParams.CategoryName));
            if (paginationParams.MinPrice.HasValue)
                query = query.Where(li => li.UnitPrice >= paginationParams.MinPrice.Value);
            if (paginationParams.MaxPrice.HasValue)
                query = query.Where(li => li.UnitPrice <= paginationParams.MaxPrice.Value);
            if (paginationParams.MinQuantity.HasValue)
                query = query.Where(li => li.Quantity >= paginationParams.MinQuantity.Value);
            if (paginationParams.MaxQuantity.HasValue)
                query = query.Where(li => li.Quantity <= paginationParams.MaxQuantity.Value);
            if (paginationParams.ProductId.HasValue)
                query = query.Where(li => li.ProductId == paginationParams.ProductId.Value);
            if (paginationParams.SaleId.HasValue)
                query = query.Where(li => li.SaleId == paginationParams.SaleId.Value);

            var sortBy = paginationParams.SortBy?.Trim().ToLower() ?? "lineitemid";
            var sortAscending = paginationParams.SortAscending;

            bool useAscending = sortAscending ?? (sortBy == "lineitemid" ? false : true);

            query = sortBy switch
            {
                "productname" => useAscending ? query.OrderBy(li => li.Product.ProductName) : query.OrderByDescending(li => li.Product.ProductName),
                "categoryname" => useAscending ? query.OrderBy(li => li.Product.Category.CategoryName) : query.OrderByDescending(li => li.Product.Category.CategoryName),
                "unitprice" => useAscending ? query.OrderBy(li => li.UnitPrice) : query.OrderByDescending(li => li.UnitPrice),
                "quantity" => useAscending ? query.OrderBy(li => li.Quantity) : query.OrderByDescending(li => li.Quantity),
                "productid" => useAscending ? query.OrderBy(li => li.ProductId) : query.OrderByDescending(li => li.ProductId),
                "saleid" => useAscending ? query.OrderBy(li => li.SaleId) : query.OrderByDescending(li => li.SaleId),
                _ => useAscending ? query.OrderBy(li => li.LineItemId) : query.OrderByDescending(li => li.LineItemId)
            };

            var pagedLineItems = await query
                                    .Skip((paginationParams.Page - 1) * paginationParams.PageSize)
                                    .Take(paginationParams.PageSize)
                                    .ToListAsync();
            
            response.Status = ResponseStatus.Success;
            response.Data = pagedLineItems;
        }
        catch (Exception ex)
        {
            response.Message = $"Error in LineItemRepository {nameof(GetPagedLineItems)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<LineItem>> GetLineItemById(int id)
    {
        var response = new BaseResponse<LineItem>();

        try
        {
            var lineItem = await _dbContext.LineItems
                .Include(li => li.Sale)
                .Include(li => li.Product)
                .ThenInclude(li => li.Category)
                .FirstOrDefaultAsync(li => li.LineItemId == id);

            if (lineItem == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Line Item not found.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = lineItem;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in LineItemRepository {nameof(GetLineItemById)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<LineItem>> CreateLineItem(LineItem lineItem)
    {
        var response = new BaseResponse<LineItem>();

        try
        {
            _dbContext.LineItems.Add(lineItem);

            await _dbContext.SaveChangesAsync();

            if (lineItem == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Line Item not created.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = lineItem;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in LineItemRepository {nameof(CreateLineItem)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<LineItem>> UpdateLineItem(LineItem updatedLineItem)
    {
        var response = new BaseResponse<LineItem>();

        try
        {
            _dbContext.LineItems.Update(updatedLineItem);
            var affectedRows = await _dbContext.SaveChangesAsync();

            if (affectedRows == 0)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "No changes were saved.";
            }
            else
            {
                await _dbContext.Entry(updatedLineItem).Reference(li => li.Sale).LoadAsync();

                response.Status = ResponseStatus.Success;
                response.Data = updatedLineItem;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in LineItemRepository {nameof(UpdateLineItem)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<LineItem>> DeleteLineItem(int id)
    {
        var response = new BaseResponse<LineItem>();

        try
        {
            response = await GetLineItemById(id);

            response.Data.IsDeleted = true;

            _dbContext.LineItems.Update(response.Data);

            var affectedRows = await _dbContext.SaveChangesAsync();

            if (affectedRows == 0)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Deletion failed.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Message = "Line Item deleted.";
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in LineItemRepository {nameof(DeleteLineItem)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }
}
