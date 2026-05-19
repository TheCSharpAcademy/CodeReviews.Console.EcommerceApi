using ECommerce.API.Interfaces;

namespace ECommerce.API.Models;

public class Sale : ISoftDeletable
{
    public int SaleId { get; set; }
    public List<SaleItem> SoldItems { get; set; } = [];

    public decimal TotalPrice => SoldItems.Sum(si => si.Item.Price * si.Quantity);

    public bool IsDeleted { get; set; }
    public DateTime? DeletedOnUtc { get; set; }
}