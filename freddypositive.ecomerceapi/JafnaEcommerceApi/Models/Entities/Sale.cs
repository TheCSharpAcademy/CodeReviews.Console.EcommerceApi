using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JafnaEcommerceApi.Models.Entities;

public class Sale
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("total_price", TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    public List<SaleDetail> SaleDetails { get; set; }
}
