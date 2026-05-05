namespace Sills.GolfShop.eCommerceAPI.Models;

public class Sales
{
    public int Id { get; set; }
    public string customerName { get; set; }

    public string shippingAddress { get; set; } = string.Empty;

    public List<ProductSales> ProductSales { get; } = [];
}
