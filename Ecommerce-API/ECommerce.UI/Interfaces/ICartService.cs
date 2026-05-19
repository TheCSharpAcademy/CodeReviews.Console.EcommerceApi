using ECommerce.Shared.Models;

namespace ECommerce.UI.Interfaces;

public interface ICartService
{
    public Task AddToCartAsync(ItemDto item);
    public Task<List<ItemDto>> GetCartAsync();
    public Task RemoveFromCartAsync(int itemId);
    public void ClearCart();
}