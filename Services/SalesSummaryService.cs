using ECommerceApp.RyanW84.Data;
using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.RyanW84.Services;

public class SalesSummaryService(ECommerceDbContext db) : ISalesSummaryService
{
    private readonly ECommerceDbContext _db = db;

    public async Task<List<SalesSummaryDto>> GetSalesSummaryAsync(
        CancellationToken cancellationToken = default
    )
    {
        List<Sale> salesData = await _db
            .Sales.AsNoTracking()
            .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product!)
                    .ThenInclude(p => p.Category)
            .ToListAsync(cancellationToken);

        return salesData
            .SelectMany(s => s.SaleItems, (sale, item) => new { Sale = sale, Item = item })
            .Where(x => x.Item.Product is { Category: not null })
            .GroupBy(x => new
            {
                ProductName = x.Item.Product!.Name,
                CategoryName = x.Item.Product!.Category!.Name,
            })
            .Select(g => new SalesSummaryDto
            {
                ProductName = g.Key.ProductName,
                CategoryName = g.Key.CategoryName,
                TotalQuantitySold = g.Sum(x => x.Item.Quantity),
                TotalRevenue = g.Sum(x => x.Item.LineTotal),
                LastSaleDate = g.Max(x => x.Sale.SaleDate),
            })
            .OrderByDescending(x => x.TotalRevenue)
            .ToList();
    }
}
