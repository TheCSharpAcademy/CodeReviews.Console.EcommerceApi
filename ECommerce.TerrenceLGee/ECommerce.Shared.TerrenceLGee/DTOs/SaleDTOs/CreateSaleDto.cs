using ECommerce.Shared.TerrenceLGee.DTOs.SaleProductDTOs;
using ECommerce.Shared.TerrenceLGee.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;

public class CreateSaleDto
{
    public string? CustomerId { get; set; }

    [Required]
    [Range(0.00, double.MaxValue, ErrorMessage = "Total base amount must be greater than or equal to $0.00.")]
    public decimal TotalBaseAmount { get; set; }

    [Required]
    [Range(0.00, double.MaxValue, ErrorMessage = "Total discount amount must be greater than or equal to $0.00.")]
    public decimal TotalDiscountAmount { get; set; }

    [Required]
    [Range(0.00, double.MaxValue, ErrorMessage = "Total amount must be greater than or equal to $0.00.")]
    public decimal TotalAmount { get; set; }

    [Required]
    [EnumDataType(typeof(SaleStatus), ErrorMessage = "Invalid sale status.")]
    public SaleStatus SaleStatus { get; set; }

    [Required]
    public List<CreateSaleProductDto> SaleProducts { get; set; } = [];
}