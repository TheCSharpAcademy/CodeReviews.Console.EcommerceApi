using Ecommerce.Core.Interfaces.Common;

namespace Ecommerce.Core.Models;

public class Product : IBaseEntity, ISoftDeletable
{
    public const int MaxNameLength = 50;
    public const int MaxDescriptionLength = 250;
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}