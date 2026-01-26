using kilozdazolik.Ecommerce.API.Data;
using kilozdazolik.Ecommerce.API.Features.Categories;
using kilozdazolik.Ecommerce.API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace kilozdazolik.Ecommerce.API.Features.Products
{
    public class ProductService(AppDbContext dbContext) : IProductService
    {
        public async Task<ProductDto> CreateProductAsync(CreateProductDto product)
        {
            ArgumentNullException.ThrowIfNull(product);
            Helper.ValidateName(product.Name, "Product name is required");

            var category = await dbContext.Categories.FindAsync(product.CategoryId);

            if ( category is null || category.IsDeleted)
            {
                throw new ArgumentException("Invalid Category ID.");
            }

            if (product.Price < 0)
                throw new ArgumentException("Product price cannot be negative", nameof(product.Price));

            Product newProduct = new()
            {
                Id = Guid.NewGuid(),
                CategoryId = product.CategoryId,
                Name = product.Name,
                Price = product.Price,
                IsDeleted = false
            };

            dbContext.Products.Add(newProduct);
            await dbContext.SaveChangesAsync();

            return new ProductDto(
                        newProduct.Id,
                        category.Id,
                        category.Name,
                        newProduct.Name,
                        newProduct.Price,
                        newProduct.IsDeleted
                    );
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await dbContext.Products.FindAsync(id);

            if (product is null) throw new KeyNotFoundException($"Product with id {id} not found");

            product.IsDeleted = true;

            await dbContext.SaveChangesAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            return await dbContext.Products
                    .Where(p => p.Id == id)
                    .Select(p => new ProductDto(
                        p.Id,
                        p.Category.Id,     
                        p.Category.Name,  
                        p.Name,
                        p.Price,
                        p.IsDeleted
                    ))
                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync(ProductParameters parameters)
        {
            var query = dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category) 
                .AsQueryable(); 


            if (!string.IsNullOrWhiteSpace(parameters.SearchTerm))
            {
                query = query.Where(p => p.Name.ToLower().Contains(parameters.SearchTerm.ToLower()));
            }

            if (parameters.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == parameters.CategoryId.Value);
            }

            if (parameters.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= parameters.MinPrice.Value);
            }

            if (parameters.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= parameters.MaxPrice.Value);
            }

            query = parameters.SortBy?.ToLower() switch
            {
                "price_asc" => query.OrderBy(p => p.Price),      
                "price_desc" => query.OrderByDescending(p => p.Price),
                "name_desc" => query.OrderByDescending(p => p.Name),  
                _ => query.OrderBy(p => p.Name) 
            };

            var products = await query
                    .Skip((parameters.PageIndex - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .Select(product => new ProductDto(
                        product.Id,                                     
                        product.CategoryId,                             
                        product.Category != null ? product.Category.Name : "N/A", 
                        product.Name,                                 
                        product.Price,                                 
                        product.IsDeleted                              
                    ))
                    .ToListAsync();

            return products;
        }

        public async Task RestoreProductAsync(Guid id)
        {
            var product = await dbContext.Products
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
                throw new KeyNotFoundException($"Product with id {id} not found");

            var category = await dbContext.Categories
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(c => c.Id == product.CategoryId);

            if (category is null || category.IsDeleted)
            {
                throw new InvalidOperationException("Cannot restore product because its category is deleted.");
            }

            product.IsDeleted = false;

            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Guid id, UpdateProductDto dto)
        {
            var product = await dbContext.Products.FindAsync(id);

            if (product is null)
                throw new InvalidOperationException($"Product with id '{id}' not found");

            if (dto is null)
                throw new ArgumentNullException(nameof(dto));

            Helper.ValidateName(dto.Name, "Product name is required");

            if (dto.Price < 0)
                throw new ArgumentException("Product price cannot be negative", nameof(dto.Price));

            if (dto.CategoryId != product.CategoryId)
            {
                var category = await dbContext.Categories.FindAsync(dto.CategoryId);
                if (category is null || category.IsDeleted)
                {
                    throw new ArgumentException("Invalid Category ID.");
                }

                product.CategoryId = dto.CategoryId;

            }
            product.Name = dto.Name;
            product.Price = dto.Price;

            await dbContext.SaveChangesAsync();
        }
    }
}
