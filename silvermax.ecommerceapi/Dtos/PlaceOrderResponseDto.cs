using silvermax.ecommerceapi.Models;

namespace silvermax.ecommerceapi.Dtos;

public record PlaceOrderResponseDto(OrderResponse? Order, PlaceOrderError? PlaceOrderError);
