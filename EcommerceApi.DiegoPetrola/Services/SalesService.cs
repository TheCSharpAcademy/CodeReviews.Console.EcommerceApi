using EcommerceApi.Context;
using EcommerceApi.Models;
using EcommerceApi.Models.DTOs;
using EcommerceApi.Results;
using EcommerceApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Services;

public class SalesService(EcommerceDbContext context)
{
    public async Task<Result<List<SaleDto>>> GetSalesByPage(int pageNumber, int pageSize)
    {
        var sales = await context.Sales
            .Include(s => s.SaleItems)
            .Skip(pageSize * pageNumber)
            .Take(pageSize)
            .Select(s => s.ToDto())
            .ToListAsync();

        return Result<List<SaleDto>>.Ok(sales);
    }

    public async Task<Result<SaleDto>> GetSale(int id)
    {
        var sale = await context.Sales
            .Include(s => s.SaleItems)
            .ThenInclude(si => si.Product)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (sale is null) return Result<SaleDto>.NotFound("Sale not found");

        return Result<SaleDto>.Ok(sale.ToDto());
    }

    public async Task<Result<SaleDto>> CreateSale(CreateSaleDto dto)
    {
        if (dto.Items == null || dto.Items.Count == 0)
            return Result<SaleDto>.Invalid("Sale must contain at least one item.");

        var sale = new Sale { SaleDate = DateTime.UtcNow };
        var productIds = dto.Items.Select(i => i.ProductId).ToList();
        var products = await context.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        if (products.Count < productIds.Count)
            return Result<SaleDto>.Invalid($"Can't make sale of invalid products!");

        foreach (var item in dto.Items)
        {
            products.TryGetValue(item.ProductId, out var product);
            if (product is null)
                return Result<SaleDto>.Invalid($"Can't make sale of invalid products!");

            sale.SaleItems.Add(new SaleItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                ProductName = product.Name,
                ProductPrice = product.Price
            });
        }
        try
        {
            context.Sales.Add(sale);
            await context.SaveChangesAsync();
            return Result<SaleDto>.Ok(sale.ToDto());
        }
        catch
        {
            return Result<SaleDto>.InternalServerError("Something went wrong");
        }
    }

    public async Task<Result<SaleDto>> DeleteSale(int id)
    {
        var sale = await context.Sales.FindAsync(id);
        if (sale is null)
            return Result<SaleDto>.NotFound("Sale not found");
        try
        {
            sale.IsDeleted = true;
            await context.SaveChangesAsync();
            return Result<SaleDto>.Ok(sale.ToDto());
        }
        catch
        {
            return Result<SaleDto>.InternalServerError("Something went wrong");
        }
    }
}
