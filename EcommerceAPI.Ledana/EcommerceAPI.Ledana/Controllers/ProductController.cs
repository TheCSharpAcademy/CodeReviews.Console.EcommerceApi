using EcommerceAPI.Ledana.DTOs;
using EcommerceAPI.Ledana.Interfaces;
using EcommerceAPI.Ledana.Models;
using EcommerceAPI.Ledana.Options;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Ledana.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        
        [HttpGet]
        public async Task<ActionResult<ApiResponseDto<List<Product>>>> Get([FromQuery] ProductOptions productOptions)
        {
            var products = await _productService.GetAllProducts(productOptions);
            if (products is null) return NotFound();

            return Ok(products);
        }
        [HttpGet("all")]
        public async Task<ActionResult<ApiResponseDto<List<Product>>>> Get()
        {
            var products = await _productService.GetAllProducts();
            if (products is null) return NotFound();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseDto<Product>>> Get(int id)
        {
            var product = await _productService.GetProductById(id);

            if (product is null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponseDto<Product>>> Post([FromBody] ProductDto product)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                return BadRequest(errors);
            }

            ApiResponseDto<Product> result = await _productService.CreateProduct(product);

            return new ObjectResult(result) { StatusCode = 201 };
        }

        
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponseDto<Product>?>> Put(int id, [FromBody] ProductUpdateDto product)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _productService.UpdateProduct(id, product);

            if (result is null) return NotFound();

            return Ok(result);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponseDto<string>?>> Delete(int id)
        {
            var response = await _productService.SoftDeleteProduct(id);

            if (response is null) return NotFound();

            return Ok(response);
        }
    }
}
