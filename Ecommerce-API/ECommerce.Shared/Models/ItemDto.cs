namespace ECommerce.Shared.Models;

public class ItemDto
{
    public int ItemId { get; set; }
    public required ItemFormat Format { get; set; }
    public required ItemType Type { get; set; }
    public required string Name { get; set; }
    public required string Artist { get; set; }
    public required decimal Price { get; set; }
    public required string Genre { get; set; }
    public List<TagDto> Tags { get; set; } = [];
}