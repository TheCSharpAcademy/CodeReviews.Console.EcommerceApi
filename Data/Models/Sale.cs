using System.Text.Json.Serialization;

namespace ECommerceApp.RyanW84.Data.Models;

/// <summary>
/// Sale domain entity.
/// Represents a customer purchase transaction with customer details and one or more <see cref="SaleItem"/> line items.
/// </summary>
public class Sale
{
    // EF Core needs settable properties for materialization
    /// <summary>
    /// Primary key for the sale.
    /// </summary>
    public int SaleId { get; set; }

    /// <summary>
    /// Timestamp for when the sale occurred.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Total sale amount, typically derived from item totals.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Customer display name associated with the sale.
    /// </summary>
    public required string CustomerName { get; set; } = null!;

    /// <summary>
    /// Customer email associated with the sale.
    /// </summary>
    public required string CustomerEmail { get; set; } = null!;

    /// <summary>
    /// Customer address associated with the sale.
    /// </summary>
    public required string CustomerAddress { get; set; } = null!;

    // Sale has many items (each item references a product)
    /// <summary>
    /// Line items belonging to this sale.
    /// </summary>
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

    // Don't serialize Categories to avoid circular references
    /// <summary>
    /// Categories associated with this sale (via product/category relationships).
    /// Not serialized to avoid cycles.
    /// </summary>
    [JsonIgnore]
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
