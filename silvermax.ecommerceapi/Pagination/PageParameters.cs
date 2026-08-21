using System.Text.Json.Serialization;

namespace silvermax.ecommerceapi.Pagination;

public class PageParameters
{
    [JsonPropertyName("pageNumber")]
    public int Pagenumber { get; set; } = 1;
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 10;
}
