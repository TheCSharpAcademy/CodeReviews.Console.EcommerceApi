using ECommerce.Api.TerrenceLGee.Data.Configuration;
using ECommerce.Api.TerrenceLGee.Data.DatabaseConfigs;
using ECommerce.Entities.TerrenceLGee.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.TerrenceLGee.Data;

public class ECommerceDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<SaleProduct> SaleProducts { get; set; }

    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new CustomerConfiguration());
        builder.ApplyConfiguration(new SaleProductConfiguration());
    }
}
