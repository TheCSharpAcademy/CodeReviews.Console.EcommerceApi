namespace ECommerceApp.RyanW84.Data.Models;

/// <summary>
/// Lightweight sales reporting projection.
/// Summarizes sales performance for a product/category combination across a time range.
/// </summary>
public class SalesSummary
{
    /// <summary>
    /// Name of the product being summarized.
    /// </summary>
    public string ProductName { get; set; } = null!;

    /// <summary>
    /// Name of the category the product belongs to.
    /// </summary>
    public string CategoryName { get; set; } = null!;

    /// <summary>
    /// Total units sold over the aggregation period.
    /// </summary>
    public int TotalQuantitySold { get; set; }

    /// <summary>
    /// Total revenue over the aggregation period.
    /// </summary>
    public decimal TotalRevenue { get; set; }

    /// <summary>
    /// Timestamp of the most recent sale included in the aggregation.
    /// </summary>
    public DateTime LastSaleDate { get; set; }
}
