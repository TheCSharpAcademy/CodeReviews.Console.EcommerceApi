using Ecommerce.API.davetn657.Data;
using Ecommerce.API.davetn657.Data.Dtos;
using Ecommerce.API.davetn657.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.davetn657.Services;

public interface ISaleService
{
    /*
     * View All Sales
     * View Sale By ID
     * Create New Sale
     */
    public PagedResponse<SalesResponseDto> AllSales(PaginationParams pagination);
    public SalesResponseDto? SaleById(int id);
    public SalesResponseDto CreateSale(CreateSalesDto sale);
}

public class SaleService : ISaleService
{
    private readonly DatabaseContext _dbContext;
    public SaleService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public PagedResponse<SalesResponseDto> AllSales([FromQuery] PaginationParams pagination)
    {
        var query = _dbContext.Sales.AsQueryable();
        var totalRecords = query.Count();
        var sales = query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(s => new SalesResponseDto
            {
                Id = s.Id,
                Total = s.Total,
                Products = s.Products.Select(p => new ProductsSalesDto
                {
                    Name = p.Name,
                    Id = p.Id,
                    Price = p.Price,
                    Category = p.Category
                }).ToList()

            }).ToList();

        var pagedResponse = new PagedResponse<SalesResponseDto>(sales, pagination.PageNumber, pagination.PageSize, totalRecords);

        return pagedResponse;
    }
    public SalesResponseDto? SaleById(int id)
    {
        return _dbContext.Sales
            .Where(s => s.Id == id)
            .Select(s => new SalesResponseDto
            {
                Id = s.Id,
                Total = s.Total,
                Products = s.Products.Select(p => new ProductsSalesDto
                {
                    Id = p.Id,
                    Price = p.Price,
                    Category = p.Category
                }).ToList()
            }).FirstOrDefault();
    }
    public SalesResponseDto CreateSale(CreateSalesDto sale)
    {
        var products = _dbContext.Products
            .Where(s => sale.ProductIds.Contains(s.Id))
            .ToList();

        if(products.Count != sale.ProductIds.Count)
        {
            throw new Exception("Could not find one or more products");
        }

        var savedSale = new Sale
        {
            Products = products,
            Total = products.Sum(s => s.Price)
        };

        _dbContext.Sales.Add(savedSale);
        _dbContext.SaveChanges();

        return new SalesResponseDto
        {
            Id = savedSale.Id,
            Total = savedSale.Total,
            Products = savedSale.Products.Select(p => new ProductsSalesDto
            {
                Id = p.Id,
                Price = p.Price,
                Category = p.Category
            }).ToList()
        };
    }
}