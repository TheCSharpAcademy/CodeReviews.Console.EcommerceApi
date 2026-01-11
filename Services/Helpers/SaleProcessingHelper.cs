using System.Net;
using ECommerceApp.RyanW84.Data;
using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Interfaces.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.RyanW84.Services.Helpers;

public class SaleProcessingHelper(ECommerceDbContext db) : ISaleProcessingHelper
{
    private readonly ECommerceDbContext _db = db;

    public ApiResponseDto<Sale>? ValidateCreateSaleRequest(ApiRequestDto<Sale> request)
    {
        var payload = request.Payload;
        if (payload is null || payload.SaleItems.Count == 0)
            return ApiResponseDto<Sale>.Failure(
                HttpStatusCode.BadRequest,
                "Invalid request or no items provided."
            );
        return null;
    }

    public async Task<(
        bool IsError,
        List<Product> Data,
        ApiResponseDto<Sale> Error
    )> FetchAndValidateProductsAsync(Sale payload, CancellationToken cancellationToken)
    {
        var productIds = payload.SaleItems.Select(si => si.ProductId).Distinct().ToList();
        var products = await _db
            .Products.Where(p => productIds.Contains(p.ProductId))
            .ToListAsync(cancellationToken);

        foreach (var item in payload.SaleItems)
        {
            var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
            if (product is null)
                return (
                    true,
                    [],
                    ApiResponseDto<Sale>.Failure(
                        HttpStatusCode.BadRequest,
                        $"Product {item.ProductId} not found."
                    )
                );

            if (product.Stock < item.Quantity)
                return (
                    true,
                    [],
                    ApiResponseDto<Sale>.Failure(
                        HttpStatusCode.Conflict,
                        $"Insufficient stock for product {product.ProductId}."
                    )
                );
        }

        return (false, products, null!);
    }

    public async Task<ApiResponseDto<Sale>> ExecuteSaleTransactionAsync(
        Sale payload,
        List<Product> products,
        CancellationToken cancellationToken
    )
    {
        await using var tx = await _db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var sale = new Sale
            {
                SaleDate = payload.SaleDate,
                CustomerName = payload.CustomerName,
                CustomerEmail = payload.CustomerEmail,
                CustomerAddress = payload.CustomerAddress,
                TotalAmount = 0m,
            };

            _db.Sales.Add(sale);
            await _db.SaveChangesAsync(cancellationToken);

            decimal total = await ProcessSaleItemsAsync(
                sale,
                payload.SaleItems.ToList(),
                products,
                cancellationToken
            );

            sale.TotalAmount = total;
            _db.Sales.Update(sale);
            await _db.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            await _db.Entry(sale)
                .Collection(s => s.SaleItems)
                .Query()
                .Include(si => si.Product)
                .LoadAsync(cancellationToken);
            return ApiResponseDto<Sale>.Success(sale, HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(cancellationToken);
            return ApiResponseDto<Sale>.Failure(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private async Task<decimal> ProcessSaleItemsAsync(
        Sale sale,
        List<SaleItem> saleItems,
        List<Product> products,
        CancellationToken cancellationToken
    )
    {
        decimal total = 0m;
        foreach (var item in saleItems)
        {
            var product = products.First(p => p.ProductId == item.ProductId);
            var unitPrice = product.Price;
            var lineTotal = unitPrice * item.Quantity;
            total += lineTotal;

            var saleItem = new SaleItem
            {
                SaleId = sale.SaleId,
                ProductId = product.ProductId,
                Quantity = item.Quantity,
                UnitPrice = unitPrice,
            };
            _db.SaleItems.Add(saleItem);

            product.Stock -= item.Quantity;
            _db.Products.Update(product);
        }

        await _db.SaveChangesAsync(cancellationToken);
        return total;
    }
}
