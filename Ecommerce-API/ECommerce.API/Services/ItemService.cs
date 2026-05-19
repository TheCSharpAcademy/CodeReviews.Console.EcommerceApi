using ECommerce.API.Interfaces;
using ECommerce.API.Models;
using ECommerce.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Services;

public class ItemService(IItemRepository repo, ITagRepository tagRepo) : IItemService
{
    public async Task PostItemAsync(CreateItemDto itemDto)
    {
        var tags = new List<Tag>();

        foreach (var tagDto in itemDto.Tags)
        {
            var existingTag = await tagRepo.GetTagByName(tagDto.TagName);
            tags.Add(existingTag ?? new Tag { TagName = tagDto.TagName });
        }

        var item = new Item
        {
            Format = itemDto.Format,
            Type = itemDto.Type,
            Name = itemDto.Name,
            Artist = itemDto.Artist,
            Genre = itemDto.Genre,
            Tags = tags,
            Price = itemDto.Price
        };

        await repo.PostItemAsync(item);
    }

    public async Task<PagedResponse<ItemDto>> GetItemsAsync(PaginationParams paginationParams)
    {
        var query = repo.GetItems();

        if (!string.IsNullOrEmpty(paginationParams.SearchTerm))
            query = query.Where(i => i.Name.ToLower().Contains(paginationParams.SearchTerm.ToLower())
                                     || i.Artist.ToLower().Contains(paginationParams.SearchTerm.ToLower()));

        if (!string.IsNullOrEmpty(paginationParams.Genre))
            query = query.Where(i => i.Genre.ToLower() == paginationParams.Genre.ToLower());
        
        if (paginationParams.SearchTags.Count > 0)
            query = query.Where(i => i.Tags.Any(t => paginationParams.SearchTags.Contains(t.TagName)));

        var totalRecords = await query.CountAsync();
        var totalPages =  (int)Math.Ceiling((double)totalRecords / paginationParams.PageSize);
        var items = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .Select(i => new ItemDto
            {
                ItemId = i.ItemId,
                Format = i.Format,
                Type = i.Type,
                Artist = i.Artist,
                Name = i.Name,
                Genre = i.Genre,
                Price = i.Price,
                Tags = i.Tags.Select(t => new TagDto { TagName = t.TagName }).ToList()
            })
            .ToListAsync();

        return new PagedResponse<ItemDto>(items, paginationParams.PageNumber, paginationParams.PageSize, totalRecords, totalPages);
    }

    public async Task<ItemDto?> GetItemByIdAsync(int id)
    {
        var item = await repo.GetItemByIdAsync(id);
        if (item is null) return null;

        return new ItemDto
        {
            Format = item.Format,
            Type = item.Type,
            Artist = item.Artist,
            Name = item.Name,
            Genre = item.Genre,
            Price = item.Price,
            Tags = item.Tags.Select(t => new TagDto { TagName = t.TagName }).ToList()
        };
    }

    /// <summary>
    ///     Deletes an item by ID
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns>True after a successful deletion, false upon an unsuccessful attempt, or null upon a NotFound error.</returns>
    public async Task<bool?> DeleteItemByIdAsync(int itemId)
    {
        var item = await repo.GetItemByIdAsync(itemId);
        if (item is null)
            return null;
        try
        {
            await repo.DeleteItemAsync(item);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}