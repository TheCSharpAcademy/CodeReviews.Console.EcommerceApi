using Ecommerce.Core.Interfaces.Common;

namespace Ecommerce.Core.Models;

public class Category : IBaseEntity, ISoftDeletable
{
    public const int MaxNameLength = 50;
    public const int MaxDescriptionLength = 250;
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
    
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}