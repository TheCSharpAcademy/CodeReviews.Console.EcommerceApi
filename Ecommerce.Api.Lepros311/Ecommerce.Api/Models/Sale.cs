namespace Ecommerce.Api.Models;

public class Sale
{
    public int SaleId { get; set; }

    public DateTime DateAndTimeOfSale { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal Subtotal => LineItems.Sum(li => li.Quantity * li.UnitPrice);

    public List<LineItem> LineItems { get; set; } = [];

    public bool IsDeleted { get; set; } = false;
}
