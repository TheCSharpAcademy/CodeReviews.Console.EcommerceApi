namespace EcommerceApi.Models;

public class SaleItem
{
    public int? ProductId { get; set; }
    public Product? Product { get; set; } = null!;
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public int SaleId { get; set; }
    public Sale Sale { get; set; } = null!;
    public int Quantity { get; set; }
}
