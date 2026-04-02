using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using System.Collections.Generic;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services;

public class ShoppingCartService : IShoppingCartService
{
    public static List<CartItemDto> ShoppingCart { get; set; } = [];
}
