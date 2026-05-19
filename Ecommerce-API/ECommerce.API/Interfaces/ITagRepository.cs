using ECommerce.API.Models;

namespace ECommerce.API.Interfaces;

public interface ITagRepository
{
    public IQueryable<Tag> GetTags();
    public Task<Tag?> GetTagByName(string name);
    public Task<Tag?> GetTagById(int id);
    public Task PostTagAsync(Tag tag);
    public Task DeleteTagAsync(Tag tag);
}