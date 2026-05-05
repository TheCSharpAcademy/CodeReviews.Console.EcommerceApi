using Newtonsoft.Json;

namespace Sills.GolfShop.eCommerceFrontEnd.Models;

public class ProductSales
{
    [JsonProperty("ProductSales")]
    public required List<ProductSale> ProductSalesList { get; set; }
}

public class ProductSale
{
    [JsonProperty("Id")]
    public int Id { get; set; }
    [JsonProperty("ProductId")]
    public int ProductId { get; set; }
    [JsonProperty("SaleId")]
    public int SaleId { get; set; }
}
