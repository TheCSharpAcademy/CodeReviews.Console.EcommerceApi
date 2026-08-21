namespace silvermax.ecommerceapi.Models;

public class Order
{
    public Guid OrderId { get; set; }
    public DateTime Moment { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public double Total { get; set; }
    public Client Client { get; set; } = null!;
    public Guid ClientId { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
