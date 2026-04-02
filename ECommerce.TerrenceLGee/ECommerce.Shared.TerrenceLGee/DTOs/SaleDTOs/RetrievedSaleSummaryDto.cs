using ECommerce.Shared.TerrenceLGee.Enums;

namespace ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;

public class RetrievedSaleSummaryDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int SaleProductCount { get; set; }
    public decimal TotalBaseAmount { get; set; }
    public decimal TotalDiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public SaleStatus SaleStatus { get; set; }
}