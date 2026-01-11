using System.Text.Json.Serialization;

namespace ECommerceApp.RyanW84.Data.Models;

/// <summary>
/// Product domain entity.
/// Represents a sellable catalog item with pricing, stock, an owning category, and optional sale history.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Primary key for the product.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Display name shown in lists and detail views.
    /// </summary>
    public required string Name { get; set; } = null!;

    /// <summary>
    /// Human-readable product description.
    /// </summary>
    public required string Description { get; set; } = null!;

    /// <summary>
    /// Current unit price.
    /// </summary>
    public required decimal Price { get; set; }

    /// <summary>
    /// Current stock level.
    /// </summary>
    public required int Stock { get; set; }

    /// <summary>
    /// Indicates whether the product is active and should be offered for sale.
    /// </summary>
    public required bool IsActive { get; set; }

    /// <summary>
    /// Foreign key to the owning <see cref="Data.Models.Category"/>.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Navigation property to the owning category.
    /// </summary>
    public Category? Category { get; set; }

    // Don't serialize SaleItems to avoid circular references
    [JsonIgnore]
    public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
}
