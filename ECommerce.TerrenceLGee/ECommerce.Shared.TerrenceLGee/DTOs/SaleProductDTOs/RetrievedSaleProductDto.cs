namespace ECommerce.Shared.TerrenceLGee.DTOs.SaleProductDTOs;

public class RetrievedSaleProductDto
{
    public int SaleId { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
}