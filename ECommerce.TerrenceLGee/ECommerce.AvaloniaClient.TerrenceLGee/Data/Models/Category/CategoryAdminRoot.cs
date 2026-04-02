using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;

public class CategoryAdminRoot : Root
{
    [JsonPropertyName("data")]
    public CategoryAdminData? Data { get; set; }
}