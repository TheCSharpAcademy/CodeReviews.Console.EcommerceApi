namespace silvermax.ecommerceapi.Dtos;

public class PlaceOrderDto
{
    public required Guid ClientId { get; set; }
    public required List<OrderLineDto> Items { get; set; }
}
