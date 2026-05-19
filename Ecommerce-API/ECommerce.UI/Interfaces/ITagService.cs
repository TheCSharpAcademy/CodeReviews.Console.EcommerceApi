using ECommerce.Shared.Models;

namespace ECommerce.UI.Interfaces;

public interface ITagService
{
    public Task<PagedResponse<TagDto>> GetTagsAsync(int pageNumber = 1, int pageSize = 10,
        string? searchTerm = null);

    public Task<int> GetTagIdByNameAsync(string tagName);

    public Task PostTagAsync(string tagName);

    public Task DeleteTagAsync(int tagId);
}