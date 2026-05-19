using ECommerce.API.Interfaces;
using ECommerce.Shared;

namespace ECommerce.API.Models;

public class Item : ISoftDeletable
{
    public int ItemId { get; set; }
    public required ItemFormat Format { get; set; }
    public required ItemType Type { get; set; }
    public required string Name { get; set; }
    public required string Artist { get; set; }
    public required decimal Price { get; set; }
    public required string Genre { get; set; }
    public List<Tag> Tags { get; set; } = [];
    public List<Sale> Sales { get; set; } = [];

    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
}