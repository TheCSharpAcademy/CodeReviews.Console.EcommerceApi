namespace Ecommerce.Api.Models;

public class LineItemDto
{
    public int LineItemId { get; set; }

    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public string Category { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public int SaleId { get; set; }
}
