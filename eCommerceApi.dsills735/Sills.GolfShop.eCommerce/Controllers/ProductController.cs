using Microsoft.AspNetCore.Mvc;
using Sills.GolfShop.eCommerceAPI.DTO;
using Sills.GolfShop.eCommerceAPI.Helpers;
using Sills.GolfShop.eCommerceAPI.Models;
using Sills.GolfShop.eCommerceAPI.Services;

namespace Sills.GolfShop.eCommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductController(IProductsService productsService) : ControllerBase
{
    private readonly IProductsService _productService = productsService;
    
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAllProducts([FromQuery]PaginationParameters param)
    {
        var products = await _productService.GetAllProductsAsync();
        
        var pagedProducts = products
            .Skip((param.PageNumber - 1) * param.PageSize)
            .Take(param.PageSize)
            .ToList();
        
        return Ok(pagedProducts);
    }
    
      [HttpGet]
    public async Task<ActionResult<List<Product>>> GetAllProductsAsync([FromQuery] PaginationParameters param)
    {
        var pagedProducts = await _productService
            .GetPagedProductsAsync(param.PageNumber, param.PageSize);

        return Ok(pagedProducts);
    }
     
    [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            var createdProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, ProductUpdateDto productUpdateDto)
    {
        var existingProduct = await _productService.GetProductByIdAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }
        existingProduct.Name = productUpdateDto.Name;
        existingProduct.Description = productUpdateDto.Description;
        existingProduct.QuantityInStock = productUpdateDto.QuantityInStock;

        await _productService.UpdateProductAsync(id, existingProduct);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var existingProduct = await _productService.GetProductByIdAsync(id);
        if (existingProduct == null)
        {
            return NotFound();
        }
        await _productService.DeleteProductAsync(id);

        return NoContent();
    }
}
