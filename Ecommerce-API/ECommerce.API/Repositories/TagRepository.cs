using ECommerce.API.Data;
using ECommerce.API.Interfaces;
using ECommerce.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Repositories;

public class TagRepository(ApiDbContext db) : ITagRepository
{
    public IQueryable<Tag> GetTags()
    {
        return db.Tags
            .OrderBy(t => t.TagId)
            .AsQueryable();
    }

    public async Task<Tag?> GetTagByName(string name)
    {
        return await db.Tags.FirstOrDefaultAsync(t => t.TagName.ToLower().Trim() == name.ToLower().Trim());
    }

    public async Task<Tag?> GetTagById(int id)
    {
        return await db.Tags.FirstOrDefaultAsync(t => t.TagId == id);
    }

    public async Task PostTagAsync(Tag tag)
    {
        db.Tags.Add(tag);
        await db.SaveChangesAsync();
    }

    public async Task DeleteTagAsync(Tag tag)
    {
        db.Tags.Remove(tag);
        await db.SaveChangesAsync();
    }
}