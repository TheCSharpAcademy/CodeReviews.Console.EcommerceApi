using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JafnaEcommerceApi.Models.Entities;

public class Category
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("deleted_date")]
    public DateTime? DeletedDate { get; set; }

    public List<Product> Products { get; set; }
}

