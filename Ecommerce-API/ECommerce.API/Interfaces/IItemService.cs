using ECommerce.API.Models;
using ECommerce.Shared.Models;

namespace ECommerce.API.Interfaces;

public interface IItemService
{
    public Task PostItemAsync(CreateItemDto itemDto);
    public Task<PagedResponse<ItemDto>> GetItemsAsync(PaginationParams paginationParams);
    public Task<ItemDto?> GetItemByIdAsync(int itemId);
    public Task<bool?> DeleteItemByIdAsync(int itemId);
}