using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Entities.TerrenceLGee.Models;

public class SaleProduct
{
    [Required(ErrorMessage = "Sale Id is required.")]
    public int SaleId { get; set; }

    [ForeignKey("SaleId")]
    public Sale? Sale { get; set; }

    [Required(ErrorMessage = "Product Id is required.")]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public Product? Product { get; set; }

    [Required(ErrorMessage = "Item quantity is required.")]
    [Range(1, 100, ErrorMessage = "Item quantity must be between 1 and 100.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Item price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than $0.00.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Item discount is required.")]
    [Range(0.00, double.MaxValue, ErrorMessage = "Discount must be greater than or equal to $0.00.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }

    [Required(ErrorMessage = "Total price for this item is required.")]
    [Range(0.00, double.MaxValue, ErrorMessage = "Total price must be greater than or equal to $0.00.")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }
}