using ECommerce.API.Data;
using ECommerce.API.Interfaces;
using ECommerce.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repositories;

public class ItemRepository(ApiDbContext db) : IItemRepository
{
    public async Task PostItemAsync(Item item)
    {
        db.Items.Add(item);
        await db.SaveChangesAsync();
    }

    public IQueryable<Item> GetItems()
    {
        return db.Items
            .Include(i => i.Tags)
            .OrderBy(i => i.ItemId)
            .AsQueryable();
    }

    public async Task<Item?> GetItemByIdAsync(int id)
    {
        return await db.Items.FindAsync(id);
    }

    public async Task DeleteItemAsync(Item item)
    {
        db.Items.Remove(item);
        await db.SaveChangesAsync();
    }
}