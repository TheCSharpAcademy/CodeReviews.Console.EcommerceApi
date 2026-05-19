using ECommerce.API.Models;
using ECommerce.Shared.Models;

namespace ECommerce.API.Interfaces;

public interface ITagService
{
    public Task<PagedResponse<TagDto>> GetTagsAsync(PaginationParams paginationParams);
    public Task<int?> GetTagIdByName(string name);
    public Task PostTagAsync(CreateTagDto tag);
    public Task<bool?> DeleteTagByIdAsync(int id);
}