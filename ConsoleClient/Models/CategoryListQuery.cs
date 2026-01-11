namespace ECommerceApp.ConsoleClient.Models;

/// <summary>
/// Represents query parameters for category list filtering and sorting.
/// </summary>
public record CategoryListQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}
