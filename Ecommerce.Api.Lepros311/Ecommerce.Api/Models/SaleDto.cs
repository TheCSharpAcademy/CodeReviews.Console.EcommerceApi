namespace Ecommerce.Api.Models;

public class SaleDto
{
    public int SaleId { get; set; }

    public DateTime DateAndTimeOfSale { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal Subtotal => LineItems.Sum(li => li.Quantity * li.UnitPrice);

    public List<LineItemDto> LineItems { get; set; } = [];
}
