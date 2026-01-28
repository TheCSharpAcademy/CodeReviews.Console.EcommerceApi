
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

        public async Task<IEnumerable<SaleDto>> GetSalesAsync(SaleParameters parameters)
        {
            var query = dbContext.Sales
                .AsNoTracking()
                .IgnoreQueryFilters() 
                .Include(s => s.SaleDetails)
                .ThenInclude(sd => sd.Product)
                .AsQueryable();


            if (parameters.FromDate.HasValue)
            {
                query = query.Where(s => s.Date >= parameters.FromDate.Value);
            }

            if (parameters.ToDate.HasValue)
            {
                query = query.Where(s => s.Date <= parameters.ToDate.Value);
            }

            if (parameters.MinAmount.HasValue)
            {
                query = query.Where(s => s.TotalAmount >= parameters.MinAmount.Value);
            }

            if (parameters.ProductId.HasValue)
            {
                query = query.Where(s => s.SaleDetails.Any(sd => sd.ProductId == parameters.ProductId.Value));
            }

            query = parameters.SortBy?.ToLower() switch
            {
                "amount_desc" => query.OrderByDescending(s => s.TotalAmount), 
                "amount_asc" => query.OrderBy(s => s.TotalAmount),           
                "date_asc" => query.OrderBy(s => s.Date),                     
                _ => query.OrderByDescending(s => s.Date)                    
            };

            return await query
                .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .Select(sale => new SaleDto(
                    sale.Id,
                    sale.Date,
                    sale.TotalAmount,
                    sale.SaleDetails.Select(sd => new SaleDetailsDto(
                        sd.ProductId,
                        sd.Product != null ? sd.Product.Name : "Unknown Product",
                        sd.Quantity,
                        sd.UnitPrice,
                        sd.Quantity * sd.UnitPrice
                    )).ToList()
                ))
                .ToListAsync();
        }
    }
}
