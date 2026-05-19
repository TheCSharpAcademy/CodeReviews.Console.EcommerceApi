using System.Text.Json;
using ECommerce.Shared.Models;
using ECommerce.UI.Configuration;
using ECommerce.UI.Helpers;
using ECommerce.UI.Interfaces;

namespace ECommerce.UI.Services;

public class TagService(IApiService apiService) : ITagService
{
    public async Task<PagedResponse<TagDto>> GetTagsAsync(int pageNumber = 1, int pageSize = 10,
        string? searchTerm = null)
    {
        var requestUrl = Utils.FormatQueryWithPaginationParams(ApiUris.TagRequestUri,
            pageNumber, pageSize, searchTerm, null, null);

        var rawJson = await apiService.GetAsync(requestUrl);
        return JsonSerializer.Deserialize<PagedResponse<TagDto>>(rawJson, Utils.GetJsonSerializerOptions())!;
    }

    public async Task<int> GetTagIdByNameAsync(string tagName)
    {
        var requestUrl = $"{ApiUris.TagRequestUri}/{tagName}";

        var rawJson = await apiService.GetAsync(requestUrl);
        return JsonSerializer.Deserialize<int>(rawJson, Utils.GetJsonSerializerOptions());
    }

    public async Task PostTagAsync(string tagName)
    {
        var tagDto = new CreateTagDto
        {
            TagName = tagName
        };

        await apiService.PostAsync(ApiUris.TagRequestUri, tagDto);
    }

    public async Task DeleteTagAsync(int tagId)
    {
        await apiService.DeleteAsync($"{ApiUris.TagRequestUri}/{tagId}");
    }
}