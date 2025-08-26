using Ecommerce.Api.Data;
using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Repository;

public class SaleRepository : ISaleRepository
{
    private readonly EcommerceDbContext _dbContext;

    public SaleRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResponse<List<Sale>>> GetPagedSales(PaginationParams paginationParams)
    {
        var response = new PagedResponse<List<Sale>>(data: new List<Sale>(),
                                                       pageNumber: paginationParams.Page,
                                                       pageSize: paginationParams.PageSize,
                                                       totalRecords: 0);

        try
        {
            var query = _dbContext.Sales
                .Include(s => s.LineItems)
                .ThenInclude(li => li.Product)
                .ThenInclude(p => p.Category)
                .AsQueryable();

            if (paginationParams.MinPrice.HasValue)
                query = query.Where(s => s.TotalPrice >= paginationParams.MinPrice.Value);
            if (paginationParams.MaxPrice.HasValue)
                query = query.Where(s => s.TotalPrice <= paginationParams.MaxPrice.Value);
            if (paginationParams.LineItemId.HasValue)
                query = query.Where(s => s.TotalPrice == paginationParams.LineItemId.Value);
            if (paginationParams.MinLineItems.HasValue)
                query = query.Where(s => s.LineItems.Count >= paginationParams.MinLineItems.Value);
            if (paginationParams.MaxLineItems.HasValue)
                query = query.Where(s => s.LineItems.Count <= paginationParams.MaxLineItems.Value);

            var sortBy = paginationParams.SortBy?.Trim().ToLower() ?? "saleid";
            var sortAscending = paginationParams.SortAscending;

            bool useAscending = sortAscending ?? (sortBy == "saleid" ? false : true);

            query = sortBy switch
            {
                "totalprice" => useAscending ? query.OrderBy(s => s.TotalPrice) : query.OrderByDescending(s => s.TotalPrice),
                "lineitemcount" => useAscending ? query.OrderBy(s => s.LineItems.Count) : query.OrderByDescending(s => s.LineItems.Count),
                _ => useAscending ? query.OrderBy(s => s.SaleId) : query.OrderByDescending(s => s.SaleId)
            };

            var pagedSales = await query
                                    .Skip((paginationParams.Page - 1) * paginationParams.PageSize)
                                    .Take(paginationParams.PageSize)
                                    .ToListAsync();

            response.Status = ResponseStatus.Success;
            response.Data = pagedSales;
        }
        catch (Exception ex)
        {
            response.Message = $"Error in SaleRepository {nameof(GetPagedSales)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Sale>> GetSaleById(int id)
    {
        var response = new BaseResponse<Sale>();

        try
        {
            var sale = await _dbContext.Sales
                .Include(s => s.LineItems)
                .ThenInclude(li => li.Product)
                .ThenInclude(p => p.Category)
                .FirstOrDefaultAsync(s => s.SaleId == id);

            if (sale == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Sale not found.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = sale;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in SaleRepository {nameof(SaleRepository)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Sale>> CreateSale(Sale sale)
    {
        var response = new BaseResponse<Sale>();

        try
        {
            _dbContext.Sales.Add(sale);

            await _dbContext.SaveChangesAsync();

            if (sale == null)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Sale not created.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = sale;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in SaleRepository {nameof(CreateSale)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Sale>> UpdateSale(Sale updatedSale)
    {
        var response = new BaseResponse<Sale>();

        try
        {
            _dbContext.Sales.Update(updatedSale);
            var affectedRows = await _dbContext.SaveChangesAsync();

            if (affectedRows == 0)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "No changes were saved.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Data = updatedSale;
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in SaleRepository {nameof(UpdateSale)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }

    public async Task<BaseResponse<Sale>> DeleteSale(int id)
    {
        var response = new BaseResponse<Sale>();

        try
        {
            response = await GetSaleById(id);

            response.Data.IsDeleted = true;

            foreach (var lineItem in response.Data.LineItems)
            {
                lineItem.IsDeleted = true;
            }

            _dbContext.Sales.Update(response.Data);
            var affectedRows = await _dbContext.SaveChangesAsync();

            if (affectedRows == 0)
            {
                response.Status = ResponseStatus.Fail;
                response.Message = "Deletion failed.";
            }
            else
            {
                response.Status = ResponseStatus.Success;
                response.Message = "Sale deleted.";
            }
        }
        catch (Exception ex)
        {
            response.Message = $"Error in SaleRepository {nameof(DeleteSale)}: {ex.Message}";
            response.Status = ResponseStatus.Fail;
        }

        return response;
    }
}

