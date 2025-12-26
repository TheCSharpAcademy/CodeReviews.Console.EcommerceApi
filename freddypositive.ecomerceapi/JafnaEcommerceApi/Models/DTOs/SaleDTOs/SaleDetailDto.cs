using System.ComponentModel.DataAnnotations;

namespace JafnaEcommerceApi.Models.DTOs.SaleDTOs;

public class SaleDetailDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    [MinLength(1)]
    [Range(1, int.MaxValue)]
    public int ProductQuantity { get; set; }
    [Range(0.01, double.MaxValue)]
    public decimal ProductPrice { get; set; }
    public decimal ProductTotalPrice { get; set; }
}
