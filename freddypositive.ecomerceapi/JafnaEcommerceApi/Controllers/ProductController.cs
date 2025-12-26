using JafnaEcommerceApi.Models.DTOs.APIResponse;
using JafnaEcommerceApi.Models.DTOs.PaginationDTOs;
using JafnaEcommerceApi.Models.DTOs.ProductDTOs;
using JafnaEcommerceApi.Services;
using Microsoft.AspNetCore.Mvc; 

namespace JafnaEcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService =  productService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] ProductCreateDto productCreateDto)
    {
        await _productService.CreateAsync(productCreateDto);
        return Ok(ApiResponse<object>.Success(null, "Product created successfully"));
    }

    [HttpPut("{productId}")]
    public async Task<ActionResult> UpdateAsync([FromRoute] int productId, [FromBody] ProductUpdateDto productUpdateDto)
    {
        await _productService.UpdateAsync(productId, productUpdateDto);
        return Ok(ApiResponse<object>.Success(null, "Product updated successfully."));

    }

    [HttpDelete("{productId}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int productId)
    {
        await _productService.DeleteAsync(productId);
        return Ok(ApiResponse<object>.Success(null, "Product deleted successfully."));
    }

    [HttpGet]
    public async Task<ActionResult> GetAllAsync([FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10)
    {
        var productList = await _productService.GetAllAsync(pageNumber, pageSize);
        return Ok(ApiResponse<PagedResponse<ProductDto>>.Success(productList, "Products obtained successfully"));
    }

    [HttpGet("{categoryId}")]
    public async Task<ActionResult> GetProductsByCategoryAsync([FromRoute] int categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var productsList = await _productService.GetProductsByCategoryAsync(categoryId, pageNumber, pageSize);
        return Ok(ApiResponse<PagedResponse<ProductDto>>.Success(productsList, "Products obtained successfully"));
    }
}
