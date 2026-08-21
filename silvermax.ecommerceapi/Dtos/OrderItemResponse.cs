namespace silvermax.ecommerceapi.Dtos;

public record OrderItemResponse(Guid ProductId, string ProductName, int Quantity, double Price)
{
    public double LineTotal => Quantity * Price;
}
