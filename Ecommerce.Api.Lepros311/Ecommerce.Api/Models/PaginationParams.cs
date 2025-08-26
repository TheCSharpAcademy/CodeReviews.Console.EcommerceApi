namespace Ecommerce.Api.Models;

public class PaginationParams
{
    private const int MaxPageSize = 50;
    public int Page { get; set; } = 1;

    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string? SortBy { get; set; }

    public bool? SortAscending { get; set; }

    public string? ProductName { get; set; }

    public string? CategoryName { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public int? MinQuantity { get; set; }

    public int? MaxQuantity { get; set; }

    public int? ProductId { get; set; }

    public int? SaleId { get; set; }

    public int? LineItemId { get; set; }

    public int? MinLineItems { get; set; }

    public int? MaxLineItems { get; set; }
}
