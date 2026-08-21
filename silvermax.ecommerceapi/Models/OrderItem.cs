namespace silvermax.ecommerceapi.Models;

public class OrderItem
{
    public Guid OrderItemId { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
