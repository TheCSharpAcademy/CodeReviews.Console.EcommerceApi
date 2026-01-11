using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.RyanW84.Controllers;

[ApiController]
[Route("api/[controller]")]
[Route("api/v1/sales")]
/// <summary>
/// Sales API endpoints.
/// Supports creating and managing sales, including read operations that can optionally include historical (soft-deleted) products.
/// </summary>
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService) => _saleService = saleService;

    // POST /api/sales
    /// <summary>
    /// Creates a sale with line items.
    /// </summary>
    [HttpPost]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Create(
        [FromBody] ApiRequestDto<Sale> request,
        CancellationToken cancellationToken
    )
    {
        ApiResponseDto<Sale> result = await _saleService.CreateSaleAsync(request, cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.SaleId }, result);
    }

    /// <summary>
    /// Retrieves a single sale by id.
    /// </summary>
    [HttpGet("{id:int}")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        ApiResponseDto<Sale> result = await _saleService.GetSaleByIdAsync(id, cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);

        Response.Headers.Append(
            "Link",
            $"</api/v1/sales/{id}>; rel=\"self\", </api/v1/sales>; rel=\"collection\""
        );
        return Ok(result);
    }

    /// <summary>
    /// Returns a paginated list of sales.
    /// Supports filtering/sorting via <see cref="SaleQueryParameters"/>.
    /// </summary>
    [HttpGet]
    [ResponseCache(
        Duration = 30,
        Location = ResponseCacheLocation.Any,
        VaryByQueryKeys = new[] { "*" }
    )]
    public async Task<IActionResult> GetAll(
        [FromQuery] SaleQueryParameters queryParameters,
        CancellationToken cancellationToken = default
    )
    {
        PaginatedResponseDto<List<Sale>> result = await _saleService.GetSalesAsync(queryParameters, cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    /// <summary>
    /// Lists sales including those whose items reference soft-deleted products (historical view).
    /// </summary>
    [HttpGet("with-deleted-products")]
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetAllWithDeletedProducts(CancellationToken cancellationToken)
    {
        ApiResponseDto<List<Sale>> result = await _saleService.GetHistoricalSalesAsync(cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    // GET /api/v1/sales/history
    // Noun-based alternative to "with-deleted-products".
    [HttpGet("history")]
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
    public Task<IActionResult> GetHistory(CancellationToken cancellationToken) =>
        GetAllWithDeletedProducts(cancellationToken);

    /// <summary>
    /// Retrieves a single sale by id, including historical (soft-deleted) products.
    /// </summary>
    [HttpGet("{id:int}/with-deleted-products")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetByIdWithDeletedProducts(
        int id,
        CancellationToken cancellationToken
    )
    {
        ApiResponseDto<Sale> result = await _saleService.GetSaleByIdWithHistoricalProductsAsync(
            id,
            cancellationToken
        );
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    // GET /api/v1/sales/{id}/history
    // Noun-based alternative to "with-deleted-products".
    [HttpGet("{id:int}/history")]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public Task<IActionResult> GetByIdHistory(int id, CancellationToken cancellationToken) =>
        GetByIdWithDeletedProducts(id, cancellationToken);

    // PUT /api/sales/{id}
    /// <summary>
    /// Updates an existing sale by id.
    /// </summary>
    [HttpPut("{id:int}")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] ApiRequestDto<Sale> request,
        CancellationToken cancellationToken
    )
    {
        ApiResponseDto<Sale> result = await _saleService.UpdateSaleAsync(id, request, cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    // DELETE /api/sales/{id}
    /// <summary>
    /// Soft-deletes a sale by id.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        ApiResponseDto<bool> result = await _saleService.DeleteSaleAsync(id, cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return NoContent();
    }
}
