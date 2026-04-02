using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;

public class ProductsAdminRoot : Root
{
    [JsonPropertyName("data")]
    public List<ProductAdminData> Data { get; set; } = [];

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("totalItemsRetrieved")]
    public int TotalItemsRetrieved { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }
}