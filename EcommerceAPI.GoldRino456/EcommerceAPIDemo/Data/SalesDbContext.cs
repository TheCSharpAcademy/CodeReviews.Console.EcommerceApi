using EcommerceAPIDemo.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPIDemo.Data;

public class SalesDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<GameProduct> GameProducts { get; set; }
    public DbSet<GameCategory> GameCategories { get; set; }
    public DbSet<Sale> Sales { get; set; }
}
