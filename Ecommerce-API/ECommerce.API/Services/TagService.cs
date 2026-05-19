using ECommerce.API.Interfaces;
using ECommerce.API.Models;
using ECommerce.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Services;

public class TagService(ITagRepository repo) : ITagService
{
    public async Task<PagedResponse<TagDto>> GetTagsAsync(PaginationParams paginationParams)
    {
        var query = repo.GetTags();

        if (!string.IsNullOrEmpty(paginationParams.SearchTerm))
            query = query.Where(t => t.TagName.ToLower().Contains(paginationParams.SearchTerm.ToLower()));

        var totalRecords = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalRecords / paginationParams.PageSize);
        var tags = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .Select(t => new TagDto
            {
                TagName = t.TagName
            }).ToListAsync();

        return new PagedResponse<TagDto>(tags, paginationParams.PageNumber, paginationParams.PageSize, totalRecords, totalPages);
    }

    public async Task<int?> GetTagIdByName(string name)
    {
        var tag = await repo.GetTagByName(name);
        return tag?.TagId;
    }

    public async Task PostTagAsync(CreateTagDto tagDto)
    {
        if (await repo.GetTagByName(tagDto.TagName) is not null) return;

        var tag = new Tag
        {
            TagName = tagDto.TagName
        };

        await repo.PostTagAsync(tag);
    }

    /// <summary>
    ///     Deletes a tag asynchronously
    /// </summary>
    /// <returns>
    ///     true upon a successful deletion, false upon an unsuccessful attempt, and null
    ///     upon a NotFound exception.
    /// </returns>
    public async Task<bool?> DeleteTagByIdAsync(int id)
    {
        var tag = await repo.GetTagById(id);
        if (tag is null)
            return null;

        try
        {
            await repo.DeleteTagAsync(tag);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}