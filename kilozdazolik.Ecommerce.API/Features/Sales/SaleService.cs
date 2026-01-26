
using kilozdazolik.Ecommerce.API.Data;
using Microsoft.EntityFrameworkCore;

namespace kilozdazolik.Ecommerce.API.Features.Sales
{
    public class SaleService(AppDbContext dbContext) : ISaleService
    {
        public async Task<SaleDto> CreateSaleAsync(CreateSaleDto createSaleDto)
        {
            ArgumentNullException.ThrowIfNull(createSaleDto);

            var inputProductIds = createSaleDto.Items
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            var products = await dbContext.Products
                .Where(p => inputProductIds.Contains(p.Id))
                .ToListAsync();


            if (products.Count != inputProductIds.Count)
            {
                throw new ArgumentException("One or more products do not exist.");
            }

            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                TotalAmount = 0m,
                SaleDetails = new List<SaleDetail>()
            };

            foreach (var itemDto in createSaleDto.Items)
            {
                var product = products.First(p => p.Id == itemDto.ProductId);

                var saleDetail = new SaleDetail
                {
                    Id = Guid.NewGuid(),
                    SaleId = sale.Id,
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };

                sale.SaleDetails.Add(saleDetail);
                sale.TotalAmount += saleDetail.Quantity * saleDetail.UnitPrice;
            }

            await dbContext.Sales.AddAsync(sale);
            await dbContext.SaveChangesAsync();


            return new SaleDto(
                sale.Id,
                sale.Date,
                sale.TotalAmount,
                sale.SaleDetails.Select(sd => new SaleDetailsDto(
                    sd.ProductId,
                    products.First(p => p.Id == sd.ProductId).Name,
                    sd.Quantity,
                    sd.UnitPrice,
                    sd.Quantity * sd.UnitPrice
                )).ToList()
            );
        }

        public async Task<SaleDto?> GetSaleByIdAsync(Guid saleId)
        {
            var sale = await dbContext.Sales
                .AsNoTracking()
                .IgnoreQueryFilters()
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .FirstOrDefaultAsync(s => s.Id == saleId);

            if (sale == null) return null;


            return new SaleDto(
                sale.Id,
                sale.Date,
                sale.TotalAmount,
                sale.SaleDetails.Select(sd => new SaleDetailsDto(
                    sd.ProductId,
                    sd.Product.Name ?? "Unknown",
                    sd.Quantity,
                    sd.UnitPrice,
                    sd.Quantity * sd.UnitPrice
                )).ToList()
            );
        }

        public async Task<IEnumerable<SaleDto>> GetSalesAsync(int pageIndex, int pageSize)
        {
            return await dbContext.Sales
                .AsNoTracking()
                .IgnoreQueryFilters()                          
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .OrderByDescending(s => s.Date) 
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(sale => new SaleDto(
                    sale.Id,
                    sale.Date,
                    sale.TotalAmount,
                    sale.SaleDetails.Select(sd => new SaleDetailsDto(
                        sd.ProductId,
                        sd.Product.Name ?? "Unknown", 
                        sd.Quantity,
                        sd.UnitPrice,
                        sd.Quantity * sd.UnitPrice
                    )).ToList()
                ))
                .ToListAsync();
        }
    }
}
