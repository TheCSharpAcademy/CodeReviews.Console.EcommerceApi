using ECommerceApp.RyanW84.Data;
using ECommerceApp.RyanW84.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.RyanW84.Repositories;

/// <summary>
/// Compiled queries for high-performance database operations.
/// Compiled queries are cached by EF Core and eliminate query compilation overhead on repeated executions.
/// </summary>
public static class CompiledQueries
{
    /// <summary>
    /// Compiled query to get a Product by ID with its Category included.
    /// </summary>
    public static readonly Func<ECommerceDbContext, int, CancellationToken, Task<Product?>> GetProductByIdWithCategory =
        EF.CompileAsyncQuery((ECommerceDbContext db, int productId, CancellationToken ct) =>
            db.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == productId)
        );

    /// <summary>
    /// Compiled query to get a Category by ID with Products and Sales included using split query.
    /// </summary>
    public static readonly Func<ECommerceDbContext, int, CancellationToken, Task<Category?>> GetCategoryByIdWithRelations =
        EF.CompileAsyncQuery((ECommerceDbContext db, int categoryId, CancellationToken ct) =>
            db.Categories
                .AsNoTracking()
                .AsSplitQuery()
                .Include(c => c.Products)
                .Include(c => c.Sales)
                .FirstOrDefault(c => c.CategoryId == categoryId)
        );

    /// <summary>
    /// Compiled query to get a Sale by ID with SaleItems, Products, and Categories using split query.
    /// </summary>
    public static readonly Func<ECommerceDbContext, int, CancellationToken, Task<Sale?>> GetSaleByIdWithRelations =
        EF.CompileAsyncQuery((ECommerceDbContext db, int saleId, CancellationToken ct) =>
            db.Sales
                .AsNoTracking()
                .AsSplitQuery()
                .Include(s => s.SaleItems)
                    .ThenInclude(si => si.Product)
                .Include(s => s.Categories)
                .FirstOrDefault(s => s.SaleId == saleId)
        );

    /// <summary>
    /// Compiled query to check if a category exists by name.
    /// </summary>
    public static readonly Func<ECommerceDbContext, string, CancellationToken, Task<bool>> CategoryExistsByName =
        EF.CompileAsyncQuery((ECommerceDbContext db, string categoryName, CancellationToken ct) =>
            db.Categories.Any(c => c.Name == categoryName)
        );

    /// <summary>
    /// Compiled query to get a Product by ID (simple, no includes).
    /// </summary>
    public static readonly Func<ECommerceDbContext, int, CancellationToken, Task<Product?>> GetProductById =
        EF.CompileAsyncQuery((ECommerceDbContext db, int productId, CancellationToken ct) =>
            db.Products
                .AsNoTracking()
                .FirstOrDefault(p => p.ProductId == productId)
        );
}
