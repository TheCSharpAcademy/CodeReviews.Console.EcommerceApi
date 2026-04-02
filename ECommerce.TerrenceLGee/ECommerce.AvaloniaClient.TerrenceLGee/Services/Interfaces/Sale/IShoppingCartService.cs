using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using System.Collections.Generic;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;

public interface IShoppingCartService
{
    static List<CartItemDto> ShoppingCart { get; set; }
}
