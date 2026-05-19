namespace ECommerce.API.Models;

public class SaleItem
{
    public int SaleId { get; set; }
    public Sale Sale { get; set; }

    public int ItemId { get; set; }
    public Item Item { get; set; }

    public int Quantity { get; set; }
}