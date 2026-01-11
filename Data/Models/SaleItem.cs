using System.Text.Json.Serialization;

namespace ECommerceApp.RyanW84.Data.Models;

/// <summary>
/// Sale line item entity.
/// Captures the product, quantity, and unit price at the time of purchase.
/// </summary>
public class SaleItem
{
    /// <summary>
    /// Foreign key to the owning <see cref="Sale"/>.
    /// </summary>
    public int SaleId { get; set; }

    // Don't serialize Sale to avoid circular references
    /// <summary>
    /// Navigation property to the owning sale.
    /// Not serialized to avoid cycles.
    /// </summary>
    [JsonIgnore]
    public Sale? Sale { get; set; }

    /// <summary>
    /// Foreign key to the purchased <see cref="Product"/>.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Navigation property to the purchased product.
    /// </summary>
    public Product? Product { get; set; }

    /// <summary>
    /// Quantity purchased.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit price captured at purchase time.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Computed total for this line item.
    /// </summary>
    public decimal LineTotal => UnitPrice * Quantity;
}
