using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JafnaEcommerceApi.Models.Entities;

public class Product
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("price", TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }

    [Column("image")]
    public string Image { get; set; }

    [Column("category_id")]
    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public List<SaleDetail> SaleDetails { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("deleted_date")]
    public DateTime? DeletedDate { get; set; }
}
