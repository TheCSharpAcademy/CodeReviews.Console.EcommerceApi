using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;

public class CategoryRoot : Root
{
    [JsonPropertyName("data")]
    public CategoryData? Data { get; set; }
}