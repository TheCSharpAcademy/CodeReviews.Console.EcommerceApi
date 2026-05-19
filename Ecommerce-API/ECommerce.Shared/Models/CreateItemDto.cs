namespace ECommerce.Shared.Models;

public class CreateItemDto
{
    public required ItemFormat Format { get; set; }
    public required ItemType Type { get; set; }
    public required string Name { get; set; }
    public required string Artist { get; set; }
    public required decimal Price { get; set; }
    public required string Genre { get; set; }
    public List<TagDto> Tags { get; set; } = [];
}