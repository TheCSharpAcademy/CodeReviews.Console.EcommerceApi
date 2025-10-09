using ECommerceApi.JJHH17.Data;
using ECommerceApi.JJHH17.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.JJHH17.Services
{
    public interface ISaleService
    {
        public List<SaleWithProductsDto> GetAllSales();
        public CategoryWithProductsDto GetSaleById(int id);
        public Sale CreateSale(CreateSaleDto sale);
    }

    public class SaleService : ISaleService
    {
        private readonly ProductsDbContext _dbContext;

        public SaleService(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<SaleWithProductsDto> GetAllSales()
        {
            return _dbContext.Sales
                .AsNoTracking()
                .Include(s => s.Products)
                .Select(s => new SaleWithProductsDto(
                    s.SaleId,
                    s.ItemCount,
                    s.SalePrice,
                    s.Products
                        .OrderBy(p => p.ProductName)
                        .Select(p => new ProductDto(p.ProductId, p.ProductName, p.Price))
                        .ToList()
                        ))
                .ToList();
        }

        public CategoryWithProductsDto GetSaleById(int id)
        {
            return _dbContext.Categories
                .AsNoTracking()
                .Where(p => p.CategoryId == id)
                .Select(p => new CategoryWithProductsDto(
                    p.CategoryId,
                    p.CategoryName,
                    p.Products
                        .OrderBy(pr => pr.ProductName)
                        .Select(pr => new ProductDto(pr.ProductId, pr.ProductName, pr.Price))
                        .ToList()
                        ))
                .SingleOrDefault();
        }

        public Sale CreateSale(CreateSaleDto sale)
        {
            if (sale is null)
            {
                throw new ArgumentNullException(nameof(sale));
            }

            if (sale.ProductIds is null || sale.ProductIds.Count == 0)
            {
                throw new ArgumentException("Provide atleast one product id", nameof(sale));
            }

            var uniqueId = sale.ProductIds.Distinct().ToList();
            var products = _dbContext.Products
                .Where(p => uniqueId.Contains(p.ProductId))
                .ToList();

            var foundId = products.Select(p => p.ProductId).ToHashSet();
            var missing = uniqueId.Where(id => !foundId.Contains(id)).ToList();
            if (missing.Count > 0)
            {
                throw new InvalidOperationException($"Unknown product ID's {string.Join(", ", missing)}");
            }

            var newSale = new Sale();
            newSale.Products.AddRange(products);

            _dbContext.Sales.Add(newSale);
            _dbContext.SaveChanges();

            return newSale;
        }
    }
}
