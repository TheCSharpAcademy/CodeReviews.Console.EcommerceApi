using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using System.Collections.Generic;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;

public record CheckoutMessage(List<CartItemDto> ShoppingCart);
