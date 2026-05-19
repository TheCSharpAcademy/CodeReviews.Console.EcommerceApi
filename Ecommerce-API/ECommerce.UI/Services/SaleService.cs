using System.Text.Json;
using ECommerce.Shared.Models;
using ECommerce.UI.Configuration;
using ECommerce.UI.Helpers;
using ECommerce.UI.Interfaces;

namespace ECommerce.UI.Services;

public class SaleService(IApiService apiService, IItemService itemService) : ISaleService
{
    public async Task<PagedResponse<SaleDto>> GetSalesAsync(int pageNumber = 1, int pageSize = 10)
    {
        var requestUrl =
            Utils.FormatQueryWithPaginationParams(ApiUris.SaleRequestUri, pageNumber, pageSize, null, null, null);
        var rawJson = await apiService.GetAsync(requestUrl);

        return JsonSerializer.Deserialize<PagedResponse<SaleDto>>(rawJson, Utils.GetJsonSerializerOptions())!;
    }

    public async Task PostSaleAsync(Dictionary<int, int> itemIdQuantityPairs)
    {
        List<CreateSaleItemDto> saleItems = [];
        foreach (var pair in itemIdQuantityPairs)
        {
            var item = await itemService.GetItemByIdAsync(pair.Key);
            if (item is null) throw new ArgumentException($"No item found with the ID {pair.Key}");

            saleItems.Add(new CreateSaleItemDto { ItemId = pair.Key, Quantity = pair.Value });
        }

        await apiService.PostAsync(ApiUris.SaleRequestUri, saleItems);
    }

    public async Task DeleteSaleAsync(int id)
    {
        await apiService.DeleteAsync($"{ApiUris.SaleRequestUri}?saleId={id}");
    }
}