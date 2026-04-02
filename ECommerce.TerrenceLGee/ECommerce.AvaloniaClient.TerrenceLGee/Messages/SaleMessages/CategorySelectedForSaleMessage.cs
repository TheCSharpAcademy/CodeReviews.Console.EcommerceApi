using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using System.Collections.Generic;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;

public record CategorySelectedForSaleMessage(int CategoryId, List<CartItemDto> ShoppingCart);
