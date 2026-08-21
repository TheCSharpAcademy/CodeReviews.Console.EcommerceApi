using silvermax.ecommerceapi.Models;

namespace silvermax.ecommerceapi.Dtos;

public record OrderResponse(
    Guid OrderId,
    DateTime Moment,
    OrderStatus OrderStatus,
    double Total,
    Guid ClientId,
    string ClientName,
    List<OrderItemResponse> Items
    );
