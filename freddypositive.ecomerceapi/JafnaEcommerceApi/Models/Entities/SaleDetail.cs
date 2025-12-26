using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JafnaEcommerceApi.Models.Entities;

public class SaleDetail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("Product")]
    [Column("product_id")]
    public int ProductId { get; set; }

    [ForeignKey("Sale")]
    [Column("sale_id")]
    public int SaleId { get; set; }

    public Product Product { get; set; }

    public Sale Sale { get; set; }

    [Column("product_name")]
    public string ProductName { get; set; }

    [Column("product_quantity")]
    public int ProductQuantity { get; set; }

    [Column("product_price", TypeName = "decimal(10,2)")]
    public decimal ProductPrice { get; set; }

    [Column("product_total_price", TypeName = "decimal(10,2)")]
    public decimal ProductTotalPrice { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }
}

