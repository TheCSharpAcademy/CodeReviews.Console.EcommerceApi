namespace ECommerce.Shared.Models;

public class CreateSaleItemDto
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
}

public class SaleItemDto
{
    public int Quantity { get; set; }
    public ItemDto Item { get; set; }
}