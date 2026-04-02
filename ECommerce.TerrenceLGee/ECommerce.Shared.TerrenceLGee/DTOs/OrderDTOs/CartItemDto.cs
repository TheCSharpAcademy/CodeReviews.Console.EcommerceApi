using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;

public class CartItemDto
{
    [Required(ErrorMessage = "Product Id is required.")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Product quantity is required.")]
    public int Quantity { get; set; }

    public string? ProductName { get; set; }
    public decimal? ProductPrice { get; set; }
    public decimal? TotalAmount { get; set; }
}