using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;

public class CreateOrderDto
{
    public string? CustomerId { get; set; }

    [Required(ErrorMessage = "Shopping cart is required.")]
    public List<CartItemDto> ShoppingCart { get; set; } = [];
}