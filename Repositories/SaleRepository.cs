using System.Net;
using ECommerceApp.RyanW84.Data;
using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.RyanW84.Repositories;

public class SaleRepository(ECommerceDbContext db) : ISaleRepository
{
    private readonly ECommerceDbContext _db = db;

    public async Task<ApiResponseDto<Sale?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            Sale? sale = await CompiledQueries.GetSaleByIdWithRelations(
                _db,
                id,
                cancellationToken
            );

            return new ApiResponseDto<Sale?>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.OK,
                ErrorMessage = string.Empty,
                Data = sale,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponseDto<Sale?>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.InternalServerError,
                ErrorMessage = ex.Message,
                Data = null,
            };
        }
    }

    private static IQueryable<Sale> ApplySaleSorting(
        IQueryable<Sale> query,
        string? sortBy,
        bool descending
    )
    {
        return sortBy switch
        {
            "customername" => descending
                ? query.OrderByDescending(s => s.CustomerName)
                : query.OrderBy(s => s.CustomerName),
            "totalamount" => descending
                ? query.OrderByDescending(s => s.TotalAmount)
                : query.OrderBy(s => s.TotalAmount),
            "saledate" => descending
                ? query.OrderByDescending(s => s.SaleDate)
                : query.OrderBy(s => s.SaleDate),
            _ => descending
                ? query.OrderByDescending(s => s.SaleDate)
                : query.OrderBy(s => s.SaleDate),
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

    public async Task<PaginatedResponseDto<List<Sale>>> GetAllSalesAsync(
        SaleQueryParameters parameters,
        CancellationToken cancellationToken = default
    )
    {
        var page = Math.Max(parameters.Page, 1);
        var pageSize = Math.Clamp(parameters.PageSize, 1, 100);

        try
        {
            IQueryable<Sale> query = GetBaseSalesQuery();
            query = ApplyFilters(query, parameters);

            var descending = string.Equals(
                parameters.SortDirection,
                "desc",
                StringComparison.OrdinalIgnoreCase
            );
            var sortBy = parameters.SortBy?.Trim().ToLowerInvariant();
            IQueryable<Sale> orderedQuery = ApplySaleSorting(query, sortBy, descending);

            var totalCount = await orderedQuery.CountAsync(cancellationToken);
            List<Sale> sales = await orderedQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResponseDto<List<Sale>>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.OK,
                ErrorMessage = string.Empty,
                Data = sales,
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
            };
        }
        catch (Exception ex)
        {
            return new PaginatedResponseDto<List<Sale>>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.InternalServerError,
                ErrorMessage = ex.Message,
                Data = [],
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = 0,
            };
        }
    }

    private IQueryable<Sale> GetBaseSalesQuery()
    {
        return _db
            .Sales.TagWith("SaleRepository.GetBaseSalesQuery")
            .AsNoTracking()
            .AsSplitQuery()
            .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
            .Include(s => s.Categories)
            .AsQueryable();
    }

    private static IQueryable<Sale> ApplyFilters(
        IQueryable<Sale> query,
        SaleQueryParameters parameters
    )
    {
        if (parameters.StartDate is { } startDate)
        {
            query = query.Where(s => s.SaleDate >= startDate);
        }

        if (parameters.EndDate is { } endDate)
        {
            query = query.Where(s => s.SaleDate <= endDate);
        }

        var customerName = parameters.CustomerName?.Trim();
        if (!string.IsNullOrEmpty(customerName))
        {
            var likePattern = $"%{customerName}%";
            query = query.Where(s => EF.Functions.Like(s.CustomerName, likePattern));
        }

        var customerEmail = parameters.CustomerEmail?.Trim();
        if (!string.IsNullOrEmpty(customerEmail))
        {
            query = query.Where(s => s.CustomerEmail == customerEmail);
        }

        return query;
    }

    public async Task<ApiResponseDto<Sale>> AddAsync(
        Sale entity,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            await _db.Sales.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            // Reload the sale with related SaleItems and Categories
            Sale? createdSale = await _db
                .Sales.AsNoTracking()
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .Include(s => s.Categories)
                .FirstOrDefaultAsync(s => s.SaleId == entity.SaleId, cancellationToken);

            return new ApiResponseDto<Sale>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.Created,
                ErrorMessage = string.Empty,
                Data = createdSale,
            };
        }
        catch (DbUpdateConcurrencyException)
        {
            return CreateErrorResponse<Sale>(
                HttpStatusCode.Conflict,
                "Concurrency conflict occurred while adding the sale."
            );
        }
        catch (DbUpdateException)
        {
            return CreateErrorResponse<Sale>(
                HttpStatusCode.BadRequest,
                "Failed to add sale. Please check the data and try again."
            );
        }
        catch (Exception)
        {
            return CreateErrorResponse<Sale>(
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred while adding the sale."
            );
        }
    }

    public async Task<ApiResponseDto<Sale>> UpdateAsync(
        Sale entity,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            _db.Sales.Update(entity);
            await _db.SaveChangesAsync(cancellationToken);

            // Reload the sale with related SaleItems and Categories
            Sale? updatedSale = await _db
                .Sales.AsNoTracking()
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .Include(s => s.Categories)
                .FirstOrDefaultAsync(s => s.SaleId == entity.SaleId, cancellationToken);

            return new ApiResponseDto<Sale>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.OK,
                ErrorMessage = string.Empty,
                Data = updatedSale,
            };
        }
        catch (DbUpdateConcurrencyException)
        {
            return CreateErrorResponse<Sale>(
                HttpStatusCode.Conflict,
                "Concurrency conflict occurred while updating the sale."
            );
        }
        catch (DbUpdateException)
        {
            return CreateErrorResponse<Sale>(
                HttpStatusCode.BadRequest,
                "Failed to update sale. Please check the data and try again."
            );
        }
        catch (Exception)
        {
            return CreateErrorResponse<Sale>(
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred while updating the sale."
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
            Sale? sale = await _db.Sales.FindAsync(new object[] { id }, cancellationToken);
            if (sale == null)
            {
                return CreateErrorResponse<bool>(HttpStatusCode.NotFound, "Sale not found");
            }
            _db.Sales.Remove(sale);
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
                "Concurrency conflict occurred while deleting the sale."
            );
        }
        catch (DbUpdateException)
        {
            return CreateErrorResponse<bool>(
                HttpStatusCode.BadRequest,
                "Failed to delete sale. Please try again."
            );
        }
        catch (Exception)
        {
            return CreateErrorResponse<bool>(
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred while deleting the sale."
            );
        }
    }

    public async Task<ApiResponseDto<Sale?>> GetByIdWithHistoricalProductsAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            Sale? sale = await GetSaleWithHistoricalProducts(id, cancellationToken);

            sale?.SaleItems = FilterHistoricalSaleItems(sale.SaleItems, sale.SaleDate);

            return new ApiResponseDto<Sale?>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.OK,
                ErrorMessage = string.Empty,
                Data = sale,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponseDto<Sale?>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.InternalServerError,
                ErrorMessage = ex.Message,
                Data = null,
            };
        }
    }

    private Task<Sale?> GetSaleWithHistoricalProducts(int id, CancellationToken cancellationToken)
    {
        return _db
            .Sales.AsNoTracking()
            .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
            .Include(s => s.Categories)
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.SaleId == id, cancellationToken);
    }

    private static List<SaleItem> FilterHistoricalSaleItems(
        IEnumerable<SaleItem> saleItems,
        DateTime saleDate
    )
    {
        return saleItems
            .Where(si =>
                si.Product != null && (!si.Product.IsDeleted || si.Product.DeletedAt > saleDate)
            )
            .ToList();
    }

    public async Task<ApiResponseDto<List<Sale>>> GetHistoricalSalesAsync(
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            List<Sale> list = await _db
                .Sales.AsNoTracking()
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .Include(s => s.Categories)
                .IgnoreQueryFilters() // Include deleted products
                .ToListAsync(cancellationToken);

            FilterAllHistoricalSaleItems(list);

            return new ApiResponseDto<List<Sale>>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.OK,
                ErrorMessage = string.Empty,
                Data = list,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponseDto<List<Sale>>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.InternalServerError,
                ErrorMessage = ex.Message,
                Data = [],
            };
        }
    }

    private static void FilterAllHistoricalSaleItems(List<Sale> sales)
    {
        foreach (Sale sale in sales)
        {
            if (true)
            {
                sale.SaleItems = FilterHistoricalSaleItems(sale.SaleItems, sale.SaleDate);
            }
        }
    }
}
