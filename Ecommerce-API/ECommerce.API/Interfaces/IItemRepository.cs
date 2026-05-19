using ECommerce.API.Models;

namespace ECommerce.API.Interfaces;

public interface IItemRepository
{
    public Task PostItemAsync(Item item);
    public IQueryable<Item> GetItems();
    public Task<Item?> GetItemByIdAsync(int id);
    public Task DeleteItemAsync(Item item);
}