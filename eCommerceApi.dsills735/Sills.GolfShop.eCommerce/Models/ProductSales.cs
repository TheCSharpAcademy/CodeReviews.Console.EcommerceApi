using Microsoft.EntityFrameworkCore;

namespace Sills.GolfShop.eCommerceAPI.Models;

public class ProductSales
{
    public int ProductID { get; set; }
    public int SaleID { get; set; }
    public Product Product { get; set; } = null!;
    public Sales Sale { get; set; } = null!;

    
}
