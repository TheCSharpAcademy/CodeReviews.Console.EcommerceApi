using Microsoft.AspNetCore.Mvc;

namespace kilozdazolik.Ecommerce.API.Features.Products
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IProductService service) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto product)
        {
            var createdProduct = await service.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id)
        {
            var product = await service.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
            [FromQuery] ProductParameters parameters)
        {
            var products = await service.GetProductsAsync(parameters);
            return Ok(products);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(Guid id, UpdateProductDto updatedProduct)
        {
            await service.UpdateProductAsync(id, updatedProduct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            await service.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpPut("{id}/restore")]
        public async Task<ActionResult> RestoreProduct(Guid id)
        {
            await service.RestoreProductAsync(id);
            return NoContent();
        }
    }

}
