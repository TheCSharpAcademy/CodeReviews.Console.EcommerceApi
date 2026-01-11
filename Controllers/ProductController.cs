using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace ECommerceApp.RyanW84.Controllers;

/// <summary>
/// API controller for managing products.
/// Provides CRUD operations and product queries with pagination support.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Route("api/products")]
[Route("api/v1/products")]
public class ProductController(IProductService productService) : ControllerBase
{
    private readonly IProductService _productService = productService;

    /// <summary>
    /// Retrieves all products with optional filtering and pagination.
    /// </summary>
    /// <param name="queryParameters">Query parameters for filtering and pagination</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of products</returns>
    [HttpGet]
    [OutputCache(PolicyName = "Products")]
    public async Task<IActionResult> GetProductsAsync(
        [FromQuery] ProductQueryParameters queryParameters,
        CancellationToken cancellationToken = default
    )
    {
        PaginatedResponseDto<List<Product>> result = await _productService.GetProductsAsync(
            queryParameters,
            cancellationToken
        );
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific product by its ID.
    /// </summary>
    /// <param name="id">The product ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The requested product or 404 Not Found</returns>
    [HttpGet("{id:int}")]
    [OutputCache(PolicyName = "Products")]
    public async Task<IActionResult> GetProductById(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        ApiResponseDto<Product?> result = await _productService.GetProductByIdAsync(
            id,
            cancellationToken
        );
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);

        Response.Headers.Append(
            "Link",
            $"</api/v1/products/{id}>; rel=\"self\", </api/v1/products>; rel=\"collection\""
        );
        return Ok(result);
    }

    // GET /api/products/category/{categoryId}
    [HttpGet("category/{categoryId:int}")]
    [OutputCache(PolicyName = "Products")]
    public async Task<IActionResult> GetProductsByCategory(
        int categoryId,
        CancellationToken cancellationToken = default
    )
    {
        ApiResponseDto<List<Product>> result = await _productService.GetProductsByCategoryIdAsync(
            categoryId,
            cancellationToken
        );
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    // POST /api/products
    [HttpPost]
    public async Task<IActionResult> CreateProduct(
        [FromBody] ApiRequestDto<Product> request,
        CancellationToken cancellationToken = default
    )
    {
        ApiResponseDto<Product> result = await _productService.CreateProductAsync(
            request,
            cancellationToken
        );
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);

        return CreatedAtAction(nameof(GetProductById), new { id = result.Data!.ProductId }, result);
    }

    // PUT /api/products/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateProduct(
        int id,
        [FromBody] ApiRequestDto<Product> request,
        CancellationToken cancellationToken = default
    )
    {
        ApiResponseDto<Product> result = await _productService.UpdateProductAsync(
            id,
            request,
            cancellationToken
        );
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    // DELETE /api/products/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        ApiResponseDto<bool> result = await _productService.DeleteProductAsync(
            id,
            cancellationToken
        );
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return NoContent();
    }

    // GET /api/products/deleted
    [HttpGet("deleted")]
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetDeletedProducts(
        CancellationToken cancellationToken = default
    )
    {
        ApiResponseDto<List<Product>> result = await _productService.GetDeletedProductsAsync(
            cancellationToken
        );
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    // POST /api/products/{id}/restore
    [HttpPost("{id:int}/restore")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> RestoreProduct(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        ApiResponseDto<bool> result = await _productService.RestoreProductAsync(
            id,
            cancellationToken
        );
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    // POST /api/v1/products/{id}/restorations
    // Noun-based alternative to "restore".
    [HttpPost("{id:int}/restorations")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public Task<IActionResult> CreateRestoration(
        int id,
        CancellationToken cancellationToken = default
    ) => RestoreProduct(id, cancellationToken);
}
