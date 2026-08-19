namespace silvermax.ecommerceapi.Models;

public class Client
{
    public Guid ClientId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
