using System.Text.Json;
using ECommerce.Shared;
using ECommerce.Shared.Models;
using ECommerce.UI.Configuration;
using ECommerce.UI.Helpers;
using ECommerce.UI.Interfaces;

namespace ECommerce.UI.Services;

public class ItemService(IApiService apiService) : IItemService
{
    public async Task<PagedResponse<ItemDto>> GetItemsAsync(int pageNumber = 1, int pageSize = 10,
        string? searchTerm = null, string? searchGenre = null, List<TagDto>? searchTags = null)
    {
        var requestUrl = Utils.FormatQueryWithPaginationParams(ApiUris.ItemRequestUri,
            pageNumber, pageSize, searchTerm, searchGenre, searchTags);

        var rawJson = await apiService.GetAsync(requestUrl);
        return JsonSerializer.Deserialize<PagedResponse<ItemDto>>(rawJson, Utils.GetJsonSerializerOptions())!;
    }

    public async Task<ItemDto> GetItemByIdAsync(int id)
    {
        var rawJson = await apiService.GetAsync($"{ApiUris.ItemRequestUri}/{id}");

        return JsonSerializer.Deserialize<ItemDto>(rawJson, Utils.GetJsonSerializerOptions())!;
    }

    public async Task DeleteItemAsync(int id)
    {
        await apiService.DeleteAsync($"{ApiUris.ItemRequestUri}/{id}");
    }

    public async Task PostItemAsync(ItemFormat format, ItemType type, string title, string artist, decimal price,
        string genre,
        string tags)
    {
        List<TagDto> tagDtos = [];

        var tagArray = tags.Split(',');
        foreach (var tag in tagArray) tagDtos.Add(new TagDto { TagName = tag });

        var itemDto = new CreateItemDto
        {
            Format = format,
            Type = type,
            Name = title,
            Artist = artist,
            Price = price,
            Genre = genre,
            Tags = tagDtos
        };

        await apiService.PostAsync(ApiUris.ItemRequestUri, itemDto);
    }
}