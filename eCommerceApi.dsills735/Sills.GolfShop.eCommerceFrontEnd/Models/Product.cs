using Newtonsoft.Json;

namespace Sills.GolfShop.eCommerceFrontEnd.Models;

public class Products
{
    [JsonProperty("Product")]
    public required List<Product> ProductsList { get; set; }
}

public class Product 
{
    [JsonProperty("Id")]
    public int Id { get; set; }
    [JsonProperty("Name")]
    public string Name { get; set; }
    [JsonProperty("Description")]
    public string Description { get; set; }
    [JsonProperty("Price")]
    public decimal Price { get; set; }
    [JsonProperty("QuantityInStock")]
    public int QuantityInStock { get; set; }
    [JsonProperty("DeletedAt")]
    public DateTime? DeletedAt { get; set; }
    [JsonProperty("ProductSales")]
    public List<ProductSales> ProductSales { get; } = [];

    [JsonProperty("CategoryId")]
    public int CategoryId { get; internal set; }


}

