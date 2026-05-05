using Newtonsoft.Json;

namespace Sills.GolfShop.eCommerceFrontEnd.Models;

internal class Sales
{
    [JsonProperty("Sales")]
    public required List<Sale> SalesList { get; set; }
}
internal class Sale
{
    [JsonProperty("Id")]
    public int Id { get; set; }
    [JsonProperty("CustomerName")]
    public string CustomerName { get; set; }
    [JsonProperty("ShippingAddress")]
    public string ShippingAddress { get; set; } = string.Empty;
    [JsonProperty("ProductSales")]
    public List<ProductSales> ProductSales { get; } = [];
}
