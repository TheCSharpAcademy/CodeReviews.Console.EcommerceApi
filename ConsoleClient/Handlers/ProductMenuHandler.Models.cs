namespace ECommerceApp.ConsoleClient.Handlers;

public partial class ProductMenuHandler
{
    private sealed record ProductListQuery
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? CategoryId { get; set; }
        public string SortBy { get; set; } = "(none)";
        public string? SortDirection { get; set; }
    }

    private sealed record ProductFilters
    {
        public static ProductFilters Empty => new() { SortBy = "(none)" };

        public string? Search { get; init; }
        public decimal? MinPrice { get; init; }
        public decimal? MaxPrice { get; init; }
        public int? CategoryId { get; init; }
        public string SortBy { get; init; } = "(none)";
        public string? SortDirection { get; init; }
    }

    private sealed record ProductUpdate
    {
        public string? Name { get; init; }
        public string? Description { get; init; }
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public int CategoryId { get; init; }
        public bool IsActive { get; init; }
    }

    private sealed record CategoryDto
    {
        public int CategoryId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
    }

    private sealed record ProductDto
    {
        public int ProductId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public bool IsActive { get; init; }
        public int CategoryId { get; init; }
    }
}
