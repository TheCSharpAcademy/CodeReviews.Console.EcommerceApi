namespace Ecommerce.Core.Models;

public class SaleItem
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int SaleId { get; set; }
    public Sale? Sale { get; set; }
    public decimal UnitPriceAtTimeOfSale { get; set; }
    public int Quantity { get; set; }
}