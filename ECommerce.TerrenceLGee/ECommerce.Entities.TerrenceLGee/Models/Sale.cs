using ECommerce.Shared.TerrenceLGee.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Entities.TerrenceLGee.Models;

public class Sale
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Customer Id is required.")]
    public string CustomerId { get; set; } = string.Empty;

    [ForeignKey("CustomerId")]
    public ApplicationUser? Customer { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.00, double.MaxValue, ErrorMessage = "Total base amount must be greater than or equal to $0.00.")]
    public decimal TotalBaseAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.00, double.MaxValue, ErrorMessage = "Total discount amount must be greater than or equal to $0.00")]
    public decimal TotalDiscountAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.00, double.MaxValue, ErrorMessage = "Total amount must be greater than or equal to $0.00.")]
    public decimal TotalAmount { get; set; }

    [Required]
    [EnumDataType(typeof(SaleStatus), ErrorMessage = "Invalid Sale Status.")]
    public SaleStatus SaleStatus { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [Required]
    public ICollection<SaleProduct> SaleProducts { get; set; } = [];
}