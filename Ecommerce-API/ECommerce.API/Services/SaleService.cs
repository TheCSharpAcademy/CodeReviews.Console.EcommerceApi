using ECommerce.API.Interfaces;
using ECommerce.API.Models;
using ECommerce.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Services;

public class SaleService(ISaleRepository repo, IItemRepository itemRepo) : ISaleService
{
    /// <summary>
    ///     Posts a sale async
    /// </summary>
    /// <returns>True upon a successful post, false upon an unsuccessful post, and null upon a NotFound error.</returns>
    public async Task<bool?> PostSaleAsync(List<CreateSaleItemDto> saleItems)
    {
        var soldItems = new List<SaleItem>();
        foreach (var saleItemDto in saleItems)
        {
            var item = await itemRepo.GetItemByIdAsync(saleItemDto.ItemId);
            if (item is null)
                return null;

            soldItems.Add(new SaleItem
            {
                Item = item,
                Quantity = saleItemDto.Quantity
            });
        }

        var sale = new Sale
        {
            SoldItems = soldItems
        };

        try
        {
            await repo.PostSale(sale);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<PagedResponse<SaleDto>> GetSalesAsync(PaginationParams paginationParams)
    {
        var query = repo.GetSales();
        var totalRecords = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalRecords / paginationParams.PageSize);
        var sales = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .Select(s => new SaleDto
            {
                SaleId = s.SaleId,
                SoldItems = s.SoldItems.Select(si => new SaleItemDto
                {
                    Quantity = si.Quantity,
                    Item = new ItemDto
                    {
                        ItemId = si.Item.ItemId,
                        Format = si.Item.Format,
                        Type = si.Item.Type,
                        Name = si.Item.Name,
                        Artist = si.Item.Artist,
                        Genre = si.Item.Genre,
                        Price = si.Item.Price,
                        Tags = si.Item.Tags.Select(t => new TagDto { TagName = t.TagName }).ToList()
                    }
                }).ToList()
            }).ToListAsync();

        return new PagedResponse<SaleDto>
            (sales, paginationParams.PageNumber, paginationParams.PageSize, totalRecords, totalPages);
    }

    /// <summary>
    ///     Deletes a sale asynchronously by ID
    /// </summary>
    /// <returns>
    ///     True upon a successful deletion, false upon an unsuccessful deletion attempt,
    ///     or null upon a NotFound exception
    /// </returns>
    public async Task<bool?> DeleteSaleByIdAsync(int saleId)
    {
        var sale = await repo.GetSaleByIdAsync(saleId);
        if (sale is null)
            return null;

        try
        {
            await repo.DeleteSaleAsync(sale);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}