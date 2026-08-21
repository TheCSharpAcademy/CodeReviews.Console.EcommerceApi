namespace silvermax.ecommerceapi.Dtos;

public class OrderLineDto
{
    public required Guid ProductId { get; set; }
    public required int Quantity { get; set; }
}
