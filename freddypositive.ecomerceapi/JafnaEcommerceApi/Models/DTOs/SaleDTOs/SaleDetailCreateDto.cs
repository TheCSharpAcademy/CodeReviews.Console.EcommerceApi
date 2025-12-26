using System.ComponentModel.DataAnnotations;

namespace JafnaEcommerceApi.Models.DTOs.SaleDTOs;

public class SaleDetailCreateDto
{
    public int ProductId { get; set; }
    [MinLength(1)]
    public int ProductQuantity { get; set; }
}
