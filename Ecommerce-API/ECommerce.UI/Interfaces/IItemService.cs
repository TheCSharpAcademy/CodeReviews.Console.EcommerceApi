using ECommerce.Shared;
using ECommerce.Shared.Models;

namespace ECommerce.UI.Interfaces;

public interface IItemService
{
    public Task<PagedResponse<ItemDto>> GetItemsAsync(int pageNumber = 1, int pageSize = 10,
        string? searchTerm = null, string? searchGenre = null, List<TagDto>? tags = null);

    public Task<ItemDto> GetItemByIdAsync(int id);
    public Task DeleteItemAsync(int id);

    /// <summary>
    ///     Posts an item asynchronously.
    /// </summary>
    /// <param name="format">Chosen ItemFormat format</param>
    /// <param name="type">Chosen ItemType type</param>
    /// <param name="title">Product's title</param>
    /// <param name="artist">Artist's name</param>
    /// <param name="price">Item price</param>
    /// <param name="genre">Item's genre</param>
    /// <param name="tags">A string of any tags, separated by commas</param>
    /// <returns></returns>
    public Task PostItemAsync(ItemFormat format, ItemType type, string title, string artist,
        decimal price, string genre, string tags);
}