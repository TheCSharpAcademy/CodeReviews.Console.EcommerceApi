using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using System.Collections.Generic;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;

public record ProductSelectedForSaleMessage(ProductData Data, int CategoryId, List<CartItemDto> ShoppingCart);
