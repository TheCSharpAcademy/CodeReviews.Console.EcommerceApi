using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.SaleProductDTOs;

public class ShoppingCartItem
{
    [Required(ErrorMessage = "Product Id is required.")]
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Quantity of product purchasing is required.")]
    public int Quantity { get; set; }
}