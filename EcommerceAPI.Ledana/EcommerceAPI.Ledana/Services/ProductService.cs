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
    public class ProductService : IProductService
    {
        private readonly ProductContext _dbContext;
        private readonly IMapper _mapper; 

        public ProductService(ProductContext productContext, IMapper mapper)
        {
            _dbContext = productContext;
            _mapper = mapper;
        }

        public async Task<ApiResponseDto<List<Product>?>> GetAllProducts(ProductOptions productOptions)
        {
            //when calling get product with pagination the user can view, update or delete products.
            //so they will see even products with stock equal to 0
            var query = _dbContext.Products
                .Include(p => p.Category)
                .AsQueryable();

            var totalProducts = await query.CountAsync();
            List<Product>? products;

            if (productOptions.Name is not null)
                query = query.Where(p => p.Name.ToLower() == productOptions.Name.ToLower());
            if (productOptions.Category is not null)
                query = query.Where(p => p.Category.Name.ToLower() == productOptions.Category.ToLower());
            if (productOptions.Price is not null)
                query = query.Where(p => p.Price <= productOptions.Price);
            if (productOptions.Stock is not null)
                query = query.Where(p => p.Stock <= productOptions.Stock);

            if(productOptions.SortBy == "id" || !string.IsNullOrEmpty(productOptions.SortBy))
            {
                switch (productOptions.SortBy)
                {
                    case "name":
                        query = productOptions.SortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(p => p.Name)
                            : query.OrderByDescending(p => p.Name);
                        break;
                    case "category":
                        query = productOptions.SortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(p => p.Category.Name)
                            : query.OrderByDescending(p => p.Category.Name);
                        break;
                    case "price":
                        query = productOptions.SortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(p => p.Price)
                            : query.OrderByDescending(p => p.Price);
                        break;
                    case "stock":
                        query = productOptions.SortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(p => p.Stock)
                            : query.OrderByDescending(p => p.Stock);
                        break;
                    default:
                        query = productOptions.SortOrder.ToUpper() == "ASC"
                            ? query.OrderBy(p => p.Id)
                            : query.OrderByDescending(p => p.Id);
                        break;
                }
            }

            query = query.Skip((productOptions.PageNumber - 1) * productOptions.PageSize)
                .Take(productOptions.PageSize);
            products = await query.ToListAsync();
            bool hasPrevious = productOptions.PageNumber > 1;
            bool hasNext = (productOptions.PageNumber * productOptions.PageSize) < totalProducts;


            return new ApiResponseDto<List<Product>?>
            {
                ResponseCode = HttpStatusCode.OK,
                Data = products,
                TotalCount = totalProducts,
                CurrentPage = productOptions.PageNumber,
                PageSize = productOptions.PageSize,
                HasPrevious = hasPrevious,
                HasNext = hasNext
            };
        }
        public async Task<ApiResponseDto<List<Product>?>> GetAllProducts()
        {
            //when calling get products without pagination user can add these products to a new sale
            //so they will see only products with stock greater then zero
            var products = await _dbContext.Products.Where(p => p.Stock > 0)
                .Include(p => p.Category).ToListAsync();
            if (products is null) return new()
            {
                RequestFailed = true,
                Data = null,
                ErrorMessage = "Could not get products",
                ResponseCode = HttpStatusCode.BadRequest
            };
            return new ApiResponseDto<List<Product>?>
            {
                ResponseCode = HttpStatusCode.OK,
                Data = products
            };
        }
        public async Task<ApiResponseDto<Product?>> GetProductById(int id)
        {
            var product =  await _dbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product is null) return new ApiResponseDto<Product?>
            {
                RequestFailed = true,
                ErrorMessage = "Couldn't find the product",
                ResponseCode = HttpStatusCode.NotFound
            };

            return new ApiResponseDto<Product?>
            {
                ResponseCode = HttpStatusCode.OK,
                Data = product
            }; 
        }

        public async Task<ApiResponseDto<Product>> CreateProduct(ProductDto product)
        {
            Product newProduct = _mapper.Map<Product>(product);

            var response = await _dbContext.Products.AddAsync(newProduct);
            await _dbContext.SaveChangesAsync();

            return new()
            {
                Data = response.Entity,
                ResponseCode = HttpStatusCode.Created
            };
        }

        public async Task<ApiResponseDto<Product>?> UpdateProduct(int id, ProductUpdateDto product)
        {
            Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (existingProduct is null)
                return new ApiResponseDto<Product>()
                { 
                    RequestFailed = true,
                    ErrorMessage = "Couldn't find product",
                    ResponseCode = HttpStatusCode.NotFound,
                    Data = null
                };

            existingProduct = _mapper.Map(product, existingProduct);

            await _dbContext.SaveChangesAsync();

            return new()
            {
                ResponseCode = HttpStatusCode.OK,
                Data = existingProduct
            };
        }

        public async Task<ApiResponseDto<string?>> SoftDeleteProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product is null)
            {
                return new ApiResponseDto<string?>
                {
                    RequestFailed = true,
                    Data = null,
                    ResponseCode = HttpStatusCode.NotFound,
                    ErrorMessage = $"Resource with id {id} was not found"
                };
            }

            product.IsDeleted = true;
            product.DeletedOnUtc = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return new ApiResponseDto<string?>
            {
                Data = $"Product with id {id} is deleted!",
                ResponseCode = HttpStatusCode.NoContent
            };
        }

       
    }
}
