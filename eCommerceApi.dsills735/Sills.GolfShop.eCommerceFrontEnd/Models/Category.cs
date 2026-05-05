using Newtonsoft.Json;

namespace Sills.GolfShop.eCommerceFrontEnd.Models;

internal class Categories
{
    [JsonProperty("Category")]
    public required List<Category> CategoriesList { get; set; }
}
public class Category
{
    [JsonProperty("Id")]
    public int Id { get; set; }
    [JsonProperty("Name")]
    public string Name { get; set; }
    [JsonProperty("Description")]
    public string Description { get; set; }
    [JsonProperty("DeletedAt")]
    public DateTime? DeletedAt { get; set; }
}
    //[JsonProperty("Products")]

    // public List<Product> Products { get; } = [];

