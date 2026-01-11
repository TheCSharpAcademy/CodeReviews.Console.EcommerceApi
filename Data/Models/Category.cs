using System.Text.Json.Serialization;

namespace ECommerceApp.RyanW84.Data.Models;

/// <summary>
/// Category domain entity.
/// Represents a logical grouping of products (e.g., Electronics) and participates in sales reporting.
/// </summary>
public class Category : BaseEntity
{
    // EF Core needs settable properties for materialization
    /// <summary>
    /// Primary key for the category.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Display name of the category.
    /// </summary>
    public required string Name { get; set; } = null!;

    /// <summary>
    /// Human-readable description shown to clients.
    /// </summary>
    public required string Description { get; set; } = null!;

    // Navigation property to Products - don't serialize to avoid circular references
    /// <summary>
    /// Products currently assigned to this category.
    /// </summary>
    [JsonIgnore]
    public ICollection<Product> Products { get; set; } = new List<Product>();

    // Navigation property to Sales - don't serialize to avoid circular references
    /// <summary>
    /// Sales associated with this category (via many-to-many relationship).
    /// </summary>
    [JsonIgnore]
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
