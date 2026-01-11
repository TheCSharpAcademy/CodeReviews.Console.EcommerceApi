namespace ECommerceApp.ConsoleClient.Models;

/// <summary>
/// Represents query parameters for sale list filtering and sorting.
/// </summary>
public record SaleListQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
}
