using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleProductDTOs;
using ECommerce.Shared.TerrenceLGee.Enums;

namespace ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;

public class RetrievedSaleDto
{
    public int Id { get; set; }
    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public decimal TotalBaseAmount { get; set; }
    public decimal TotalDiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public SaleStatus SaleStatus { get; set; }
    public List<RetrievedSaleProductDto> SaleProducts { get; set; } = [];
}