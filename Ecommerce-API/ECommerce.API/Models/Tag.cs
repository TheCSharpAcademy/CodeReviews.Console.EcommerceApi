using ECommerce.API.Interfaces;

namespace ECommerce.API.Models;

public class Tag : ISoftDeletable
{
    public int TagId { get; set; }
    public required string TagName { get; set; }
    public List<Item> Items { get; } = [];

    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
}