namespace ECommerce.Shared.Models;

public class SaleDto
{
    public int SaleId { get; set; }
    public List<SaleItemDto> SoldItems { get; set; } = [];

    public decimal TotalPrice => SoldItems.Sum(si => si.Item.Price * si.Quantity);
}