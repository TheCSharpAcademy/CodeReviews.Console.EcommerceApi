using ECommerce.Shared.TerrenceLGee.Enums;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;

public class UpdateSaleStatusDto
{
    public int SaleId { get; set; }

    [Required]
    [EnumDataType(typeof(SaleStatus), ErrorMessage = "Invalid sale status.")]
    public SaleStatus Status { get; set; }
}