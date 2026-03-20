using Ecommerce.API.davetn657.Data;
using Ecommerce.API.davetn657.Data.Dtos;
using Ecommerce.API.davetn657.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.API.davetn657.Services;

public interface IProductService
{
    /*
     CreateProduct
    ViewAllProducts
    ViewProductById
    ViewProductByCategory
     */

    public PagedResponse<ProductsDto> AllProducts(PaginationParams pagination);
    public PagedResponse<ProductsDto> ProductsByCategory(string category, PaginationParams pagination);
    public ProductsDto? ProductById(int id);
    public ProductsDto CreateProduct(ProductsDto product);
}

public class ProductService : IProductService
{
    private readonly DatabaseContext _dbContext;
    public ProductService(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public PagedResponse<ProductsDto> AllProducts(PaginationParams pagination)
    {
        var query = _dbContext.Products.AsQueryable();
        var totalRecords = query.Count();
        var products = query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(p => new ProductsDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category,
                Sales = p.Sales.Select(s => new SalesDto
                {
                    Id = s.Id,
                    Total = s.Total,
                    ProductIds = s.Products.Select(p => p.Id).ToList()
                }).ToList()
            }).ToList();

        var pagedResponse = new PagedResponse<ProductsDto>(products, pagination.PageNumber, pagination.PageSize, totalRecords);

        return pagedResponse;
    }
    public PagedResponse<ProductsDto> ProductsByCategory(string category, PaginationParams pagination)
    {
        var query = _dbContext.Products.Where(p => p.Category.ToLower() == category.ToLower());
        var totalRecords = query.Count();
        var products = query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(p => new ProductsDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category,
                Sales = p.Sales.Select(s => new SalesDto
                {
                    Id = s.Id,
                    Total = s.Total,
                    ProductIds = s.Products.Select(p => p.Id).ToList()
                }).ToList()
            }).ToList();

        var pagedResponse = new PagedResponse<ProductsDto>(products, pagination.PageNumber, pagination.PageSize, totalRecords);

        return pagedResponse;
    }
    public ProductsDto? ProductById(int id)
    {
        return _dbContext.Products
        .Where(p => p.Id == id)
        .Select(p => new ProductsDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Category = p.Category,
            Sales = p.Sales.Select(s => new SalesDto
            {
                Id = s.Id,
                Total = s.Total,
                ProductIds = s.Products.Select(p => p.Id).ToList()
            }).ToList()
        }).FirstOrDefault();
    }
    public ProductsDto CreateProduct(ProductsDto product)
    {
        var savedProduct = new Product
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Category = product.Category
        };

        _dbContext.Products.Add(savedProduct);
        _dbContext.SaveChanges();
        return new ProductsDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Category = product.Category
        };
    }
}