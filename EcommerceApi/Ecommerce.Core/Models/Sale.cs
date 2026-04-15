using Ecommerce.Core.Interfaces.Common;

namespace Ecommerce.Core.Models;

public class Sale : IBaseEntity, ISoftDeletable
{
    public int Id { get; set; }
    public DateTime CreationDate { get; init; }
    public decimal TotalPrice { get; init; }
    public ICollection<SaleItem> Items { get; init; } = new List<SaleItem>();

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}