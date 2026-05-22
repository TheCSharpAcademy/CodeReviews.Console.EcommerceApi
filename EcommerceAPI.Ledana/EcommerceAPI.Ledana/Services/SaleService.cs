using AutoMapper;
using EcommerceAPI.Ledana.Data;
using EcommerceAPI.Ledana.DTOs;
using EcommerceAPI.Ledana.Interfaces;
using EcommerceAPI.Ledana.Models;
using EcommerceAPI.Ledana.Options;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EcommerceAPI.Ledana.Services
{
    public class SaleService : ISaleService
    {
        private readonly IMapper _mapper;
        public readonly ProductContext _dbContext;

        public SaleService(IMapper mapper, ProductContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<ApiResponseDto<Sale>> CreateSale(SaleDto sale)
        {

            var newSale = _mapper.Map<Sale>(sale);


            foreach (var item in newSale.SaleProducts)
            {
                var product = await _dbContext.Products.FindAsync(item.ProductsId);

                if (product is null) return new()
                {
                    RequestFailed = true,
                    ErrorMessage = $"Product with ID {item.ProductsId} not found",
                    ResponseCode = HttpStatusCode.BadRequest
                };

                //saving for each product added in sale its price to unit price at sale for saleproduct
                item.UnitPriceAtSale = product.Price;

                //checking if quanity is bigger then stock
                if (item.Quantity > product.Stock)
                    return new()
                    {
                        RequestFailed = true,
                        ErrorMessage = "Amount is bigger than stock",
                        ResponseCode = HttpStatusCode.BadRequest
                    };
                //removing the quantity of products bought from stock of product
                product.Stock -= item.Quantity;
            }

            var response = await _dbContext.Sales
                .AddAsync(newSale);

            if (response is null) return new()
            {
                RequestFailed = true,
                ErrorMessage = "Couldn't add the new sale",
                Data = null,
                ResponseCode = HttpStatusCode.BadRequest
            };

            await _dbContext.SaveChangesAsync();
            return new()
            {
                Data = response.Entity,
                ResponseCode = HttpStatusCode.OK
            };
        }
        public async Task<ApiResponseDto<List<SaleProductViewDto>>> GetAllSales()
        {
            var query = _dbContext.Sales
               .IgnoreQueryFilters()
               .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
               .ThenInclude(p => p.Category)
               .Select(s => new SaleProductViewDto
               {
                   SaleId = s.Id,
                   Date = s.Date,
                   TotalPrice = s.SaleProducts.Sum(sp => sp.TotalPrice),
                   Products = s.SaleProducts.Select(sp => new SaleProductListDto
                   {
                       ProductName = sp.Product.Name,
                       CategoryName = sp.Product.Category.Name,
                       Quantity = sp.Quantity,
                       UnitPriceAtSale = sp.UnitPriceAtSale,
                       Discount = sp.Discount,
                       TotalPrice = sp.TotalPrice
                   }).ToList()
               })
               .AsQueryable();

            List<SaleProductViewDto>? sales;

            sales = await query.ToListAsync();

            if (sales is null) return new()
            {
                RequestFailed = true,
                ErrorMessage = "Couldn't fetch the sales",
                Data = null,
                ResponseCode = HttpStatusCode.BadRequest
            };

            return new ApiResponseDto<List<SaleProductViewDto>>()
            {
                Data = sales,
                ResponseCode = HttpStatusCode.OK
            };
        }
        public async Task<ApiResponseDto<List<SaleProductViewDto>>> GetAllSales(SaleOptions saleOptions)
        {
            var query = _dbContext.Sales
                .IgnoreQueryFilters()
                .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
                .ThenInclude(p => p.Category)
                .Select(s => new SaleProductViewDto
                {
                    SaleId = s.Id,
                    Date = s.Date,
                    TotalPrice = s.SaleProducts.Sum(sp => sp.TotalPrice),
                    Products = s.SaleProducts.Select(sp => new SaleProductListDto
                    {
                        ProductName = sp.Product.Name,
                        CategoryName = sp.Product.Category.Name,
                        Quantity = sp.Quantity,
                        UnitPriceAtSale = sp.UnitPriceAtSale,
                        Discount = sp.Discount,
                        TotalPrice = sp.TotalPrice
                    }).ToList()
                })
                .AsQueryable();

            var totalSales = await query.CountAsync();
            List<SaleProductViewDto>? sales;

            if (saleOptions.ProductName is not null)
                query = query.Where(s => s.Products.Any(p => p.ProductName.ToLower() == saleOptions.ProductName.ToLower()));
            if (saleOptions.CategoryName is not null)
                query = query.Where(s => s.Products.Any(p => p.CategoryName.ToLower() == saleOptions.CategoryName.ToLower()));
            if (saleOptions.TotalPrice is not null)
                query = query.Where(s => s.TotalPrice <= saleOptions.TotalPrice);
            if (saleOptions.Date.HasValue)
                query = query.Where(s => s.Date <= saleOptions.Date);

            if (saleOptions.SortBy == "id" || !string.IsNullOrEmpty(saleOptions.SortBy))
            {
                switch (saleOptions.SortBy)
                {

                    case "total_price":
                        query = saleOptions.SortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(s => s.TotalPrice)
                            : query.OrderByDescending(s => s.TotalPrice);
                        break;
                    case "date":
                        query = saleOptions.SortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(s => s.Date)
                            : query.OrderByDescending(s => s.Date);
                        break;
                    default:
                        query = saleOptions.SortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(s => s.SaleId)
                            : query.OrderByDescending(s => s.SaleId);
                        break;
                }
            }

            query = query.Skip((saleOptions.PageNumber - 1) * saleOptions.PageSize)
                .Take(saleOptions.PageSize);
            sales = await query.ToListAsync();
            bool hasPrevious = saleOptions.PageNumber > 1;
            bool hasNext = (saleOptions.PageNumber * saleOptions.PageSize) < totalSales;

            if (sales is null) return new()
            {
                RequestFailed = true,
                ErrorMessage = "Couldn't fetch the sales",
                Data = null,
                ResponseCode = HttpStatusCode.BadRequest
            };

            return new ApiResponseDto<List<SaleProductViewDto>>()
            {
                Data = sales,
                ResponseCode = HttpStatusCode.OK,
                TotalCount = totalSales,
                CurrentPage = saleOptions.PageNumber,
                PageSize = saleOptions.PageSize,
                HasPrevious = hasPrevious,
                HasNext = hasNext
            };
        }

        public async Task<ApiResponseDto<SaleProductViewDto>> GetSaleById(int id)
        {
            var sale = await _dbContext.Sales.IgnoreQueryFilters()
                .Include(s => s.SaleProducts)
                    .ThenInclude(sp => sp.Product)
                    .ThenInclude(p => p.Category)
                .Select(s => new SaleProductViewDto
                {
                    SaleId = s.Id,
                    Date = s.Date,
                    TotalPrice = s.SaleProducts.Sum(sp => sp.TotalPrice),
                    Products = s.SaleProducts.Select(sp => new SaleProductListDto
                    {
                        ProductName = sp.Product.Name,
                        CategoryName = sp.Product.Category.Name,
                        Quantity = sp.Quantity,
                        UnitPriceAtSale = sp.UnitPriceAtSale,
                        Discount = sp.Discount,
                        TotalPrice = sp.TotalPrice
                    }).ToList()
                }).FirstAsync(s => s.SaleId == id); ;

            if (sale is null) return new()
            {
                RequestFailed = true,
                Data = null,
                ErrorMessage = "Couldn't find the sale",
                ResponseCode = HttpStatusCode.NotFound
            };

            return new()
            {
                Data = sale,
                ResponseCode = HttpStatusCode.OK
            };
        }
    }
}
