using ECommerce.Api.TerrenceLGee.Data;
using ECommerce.Api.TerrenceLGee.Repositories.Helpers;
using ECommerce.Contracts.TerrenceLGee.Common.Extensions;
using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Enums;
using ECommerce.Shared.TerrenceLGee.Parameters;
using ECommerce.Shared.TerrenceLGee.Parameters.SaleParameters;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.TerrenceLGee.Repositories;

public class SaleRepository : ISaleRepository
{
    private readonly ECommerceDbContext _context;
    private readonly ILogger<SaleRepository> _logger;
    private string _errorMessage = string.Empty;

    public SaleRepository(ECommerceDbContext context, ILogger<SaleRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Sale?> AddSaleAsync(Sale sale)
    {
        try
        {
            var customer = await _context.Users.FirstOrDefaultAsync(c => c.Id.Equals(sale.CustomerId));

            if (customer is null) return null;

            sale.CreatedAt = DateTime.UtcNow;

            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();

            return await _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleProducts)
                .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(s => s.Id == sale.Id);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleRepository)}\n" +
                $"Method: {nameof(AddSaleAsync)}\n" +
                $"There was an unexpected error creating the sale for customer: {sale.CustomerId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<Sale?> GetSaleAsync(int saleId, string? customerId)
    {
        try
        {
            var sale = await _context.Sales
                .AsNoTracking()
                .Include(s => s.Customer)
                .Include(s => s.SaleProducts)
                .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(s => s.Id == saleId && s.Customer != null && s.Customer.Id.Equals(customerId));

            return sale;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleRepository)}\n" +
                $"Method: {nameof(GetSaleAsync)}\n" +
                $"There was an unexpected error retrieving sale {saleId} for customer {customerId}: " +
                $"{ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<Sale?> GetSaleForAdminAsync(int saleId)
    {
        try
        {
            var sale = await _context.Sales
                .AsNoTracking()
                .Include(s => s.Customer)
                .Include(s => s.SaleProducts)
                .ThenInclude(sp => sp.Product)
                .FirstOrDefaultAsync(s => s.Id == saleId);

            return sale;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleRepository)}\n" +
                $"Method: {nameof(GetSaleForAdminAsync)}\n" +
                $"There was an unexpected error retrieving sale {saleId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<(bool, SaleStatus)> AdminUpdateSaleStatusAsync(int saleId, SaleStatus status)
    {
        try
        {
            var saleToUpdate = await _context.Sales
                .FirstOrDefaultAsync(s => s.Id == saleId);

            if (saleToUpdate is null) return (false, SaleStatus.None);

            if (status == SaleStatus.Canceled)
            {
                var result = await RestockAsync(saleToUpdate.SaleProducts);
                if (!result) return (false, SaleStatus.None);
            }

            if (saleToUpdate.SaleStatus == status) return (false, saleToUpdate.SaleStatus);
            if (saleToUpdate.SaleStatus == SaleStatus.Delivered || saleToUpdate.SaleStatus == SaleStatus.Canceled) return (false, saleToUpdate.SaleStatus);
            if (saleToUpdate.SaleStatus == SaleStatus.Processing && status == SaleStatus.Pending) return (false, saleToUpdate.SaleStatus);
            if (saleToUpdate.SaleStatus == SaleStatus.Shipped && status == SaleStatus.Processing) return (false, saleToUpdate.SaleStatus);
            

            saleToUpdate.SaleStatus = status;
            saleToUpdate.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return (true, saleToUpdate.SaleStatus);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleRepository)}\n" +
                $"Method: {nameof(AdminUpdateSaleStatusAsync)}\n" +
                $"There was an unexpected error updating the status of sale {saleId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return (false, SaleStatus.None);
        }
    }

    public async Task<(bool, SaleStatus)> CustomerCancelSaleAsync(int saleId, string? customerId)
    {
        try
        {
            var saleToCancel = await _context.Sales
                .Include(s => s.SaleProducts)
                .FirstOrDefaultAsync(s => s.Id == saleId && s.Customer != null && s.CustomerId.Equals(customerId));

            if (saleToCancel is null) return (false, SaleStatus.None);

            if (saleToCancel.SaleStatus == SaleStatus.Shipped
                || saleToCancel.SaleStatus == SaleStatus.Delivered
                || saleToCancel.SaleStatus == SaleStatus.Canceled) return (false, saleToCancel.SaleStatus);

            var result = await RestockAsync(saleToCancel.SaleProducts);

            if (result)
            {
                saleToCancel.SaleStatus = SaleStatus.Canceled;
                saleToCancel.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return (result, SaleStatus.None);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleRepository)}\n" +
                $"Method: {nameof(CustomerCancelSaleAsync)}\n" +
                $"There was an unexpected error cancelling sale {saleId} for customer {customerId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return (false, SaleStatus.None);
        }
    }

    public async Task<PagedList<Sale>> GetSalesAsync(SaleQueryParams saleQueryParams)
    {
        try
        {
            var sales = _context.Sales
                .Where(s => s.CustomerId.Equals(saleQueryParams.CustomerId))
                .Include(s => s.Customer)
                .Include(s => s.SaleProducts)
                .AsNoTracking();

            SetFilteringAndSorting(ref sales, saleQueryParams);

            return await sales.ToPagedListAsync(sales.Count(), saleQueryParams.Page, saleQueryParams.PageSize);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleRepository)}\n" +
                $"Method: {nameof(GetSalesAsync)}\n" +
                $"There was an unexpected error retrieving the sales for customer {saleQueryParams.CustomerId}: " +
                $"{ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }

    public async Task<PagedList<Sale>> GetAllSalesForAdminAsync(SaleQueryParams saleQueryParams)
    {
        try
        {
            var sales = _context.Sales
                .Include(s => s.Customer)
                .Include(s => s.SaleProducts)
                .AsNoTracking();

            SetFilteringAndSorting(ref sales, saleQueryParams);

            return await sales.ToPagedListAsync(sales.Count(), saleQueryParams.Page, saleQueryParams.PageSize);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(SaleRepository)}\n" +
                $"Method: {nameof(GetAllSalesForAdminAsync)}\n" +
                $"There was an unexpected error retrieving all sales: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }

    private void SetFilteringAndSorting(ref IQueryable<Sale> sales, SaleQueryParams saleQueryParams)
    {
        if (!string.IsNullOrEmpty(saleQueryParams.Status))
        {
            sales = saleQueryParams.Status.ToLower() switch
            {
                "pending" => sales.Where(s => s.SaleStatus == SaleStatus.Pending),
                "processing" => sales.Where(s => s.SaleStatus == SaleStatus.Processing),
                "shipped" => sales.Where(s => s.SaleStatus == SaleStatus.Shipped),
                "delivered" => sales.Where(s => s.SaleStatus == SaleStatus.Delivered),
                "canceled" => sales.Where(s => s.SaleStatus == SaleStatus.Canceled),
                _ => sales
            };
        }

        if (saleQueryParams.MinTotalAmount.HasValue && saleQueryParams.MaxTotalAmount.HasValue)
        {
            if (saleQueryParams.IsValidAmountRange)
            {
                sales = sales.Where(s => s.TotalAmount >= saleQueryParams.MinTotalAmount && s.TotalAmount <=
                saleQueryParams.MaxTotalAmount);
            }
        }
        else if (saleQueryParams.MinTotalAmount.HasValue && !saleQueryParams.MaxTotalAmount.HasValue)
        {
            sales = sales.Where(s => s.TotalAmount >= saleQueryParams.MinTotalAmount);
        }
        else if (saleQueryParams.MaxTotalAmount.HasValue && !saleQueryParams.MinTotalAmount.HasValue)
        {
            sales = sales.Where(s => s.TotalAmount <= saleQueryParams.MaxTotalAmount);
        }

        if (!string.IsNullOrEmpty(saleQueryParams.CustomerFirstName))
        {
            sales = sales.Where(s => s.Customer != null &&
            s.Customer.FirstName.ToLower().Equals(saleQueryParams.CustomerFirstName.ToLower()));
        }

        if (!string.IsNullOrEmpty(saleQueryParams.CustomerLastName))
        {
            sales = sales.Where(s => s.Customer != null &&
            s.Customer.LastName.ToLower().Equals(saleQueryParams.CustomerLastName.ToLower()));
        }

        if (!string.IsNullOrEmpty(saleQueryParams.CustomerId))
        {
            sales = sales.Where(s => s.Customer != null && s.CustomerId.Equals(saleQueryParams.CustomerId));
        }

        sales = SortHelper<Sale>.ApplySorting(sales, saleQueryParams.OrderBy);
    }

    private async Task<bool> RestockAsync(IEnumerable<SaleProduct> saleProducts)
    {
        foreach (var saleProduct in saleProducts)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == saleProduct.ProductId);

            if (product is null) return false;

            var isProductPreviouslyOutOfStock = !product.IsInStock;

            product.StockQuantity += saleProduct.Quantity;

            if (isProductPreviouslyOutOfStock && product.StockQuantity > 0) product.IsInStock = true;
        }
        await _context.SaveChangesAsync();

        return true;
    }
}
