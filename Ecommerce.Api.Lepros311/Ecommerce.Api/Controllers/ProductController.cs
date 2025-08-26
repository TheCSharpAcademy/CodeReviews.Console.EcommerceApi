using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;
using Ecommerce.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetPagedProducts([FromQuery] PaginationParams paginationParams)
        {
            var responseWithDtos = await _productService.GetPagedProducts(paginationParams);

            if (responseWithDtos.Status == ResponseStatus.Fail)
            {
                return BadRequest(responseWithDtos.Message);
            }

            return Ok(responseWithDtos.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var response = await _productService.GetProductById(id);

            if (response.Status == ResponseStatus.Fail)
            {
                return NotFound(response.Message);
            }

            var returnedProduct = response.Data;

            var productDto = new ProductDto
            {
                ProductId = returnedProduct.ProductId,
                ProductName = returnedProduct.ProductName,
                Price = returnedProduct.Price,
                Category = returnedProduct.Category.CategoryName
            };

            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] WriteProductDto writeProductDto)
        {
            var responseWithDataDto = await _productService.CreateProduct(writeProductDto);

            if (responseWithDataDto.Message == "Category not found.")
            {
                return NotFound(responseWithDataDto.Message);
            }

            if (responseWithDataDto.Status == ResponseStatus.Fail)
            {
                return BadRequest(responseWithDataDto.Message);
            }

            return CreatedAtAction(nameof(GetProductById),
                new { id = responseWithDataDto.Data.ProductId }, responseWithDataDto.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(int id, [FromBody] WriteProductDto writeProductDto)
        {
            var response = await _productService.UpdateProduct(id, writeProductDto);

            if (response.Message == "Product not found." || response.Message == "Category not found.")
            {
                return NotFound(response.Message);
            }

            if (response.Status == ResponseStatus.Fail)
            {
                return BadRequest(response.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _productService.DeleteProduct(id);

            if (response.Message == "Product not found.")
            {
                return NotFound(response.Message);
            }
            else if (response.Status == ResponseStatus.Fail)
            {
                return BadRequest(response.Message);
            }

            return NoContent();
        }
    }
}
