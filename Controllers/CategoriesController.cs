using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.RyanW84.Controllers;

[ApiController]
[Route("api/[controller]")]
[Route("api/v1/categories")]
/// <summary>
/// Category CRUD API endpoints.
/// Provides create, read (by id/name), update, delete, and soft-delete restore operations for <see cref="Category"/>.
/// Responses are wrapped in the project's standard API response DTOs and follow the same error mapping conventions.
/// </summary>
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService) =>
        _categoryService = categoryService;

    // POST /api/categories
    /// <summary>
    /// Creates a new category.
    /// </summary>
    [HttpPost]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Create(
        [FromBody] ApiRequestDto<Category> request,
        CancellationToken cancellationToken
    )
    {
        ApiResponseDto<Category> result = await _categoryService.CreateCategoryAsync(request, cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.CategoryId }, result);
    }

    // GET /api/categories
    /// <summary>
    /// Returns a paginated list of categories.
    /// Supports filtering/sorting via <see cref="CategoryQueryParameters"/>.
    /// </summary>
    [HttpGet]
    [ResponseCache(
        Duration = 120,
        Location = ResponseCacheLocation.Any,
        VaryByQueryKeys = new[] { "*" }
    )]
    public async Task<IActionResult> GetAll(
        [FromQuery] CategoryQueryParameters queryParameters,
        CancellationToken cancellationToken
    )
    {
        PaginatedResponseDto<List<Category>> result = await _categoryService.GetAllCategoriesAsync(
            queryParameters,
            cancellationToken
        );
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    // GET /api/categories/{id}
    /// <summary>
    /// Retrieves a single category by numeric identifier.
    /// </summary>
    [HttpGet("{id:int}")]
    [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        ApiResponseDto<Category> result = await _categoryService.GetCategoryAsync(id, cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);

        Response.Headers.Append(
            "Link",
            $"</api/v1/categories/{id}>; rel=\"self\", </api/v1/categories>; rel=\"collection\""
        );
        return Ok(result);
    }

    // GET /api/categories/name/{name}
    /// <summary>
    /// Retrieves a category by its name.
    /// </summary>
    [HttpGet("name/{name}")]
    [ResponseCache(
        Duration = 60,
        Location = ResponseCacheLocation.Any,
        VaryByQueryKeys = new[] { "name" }
    )]
    public async Task<IActionResult> GetByName(string name, CancellationToken cancellationToken)
    {
        ApiResponseDto<Category> result = await _categoryService.GetCategoryByNameAsync(name, cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    // PUT /api/categories/{id}
    /// <summary>
    /// Updates an existing category by id.
    /// </summary>
    [HttpPut("{id:int}")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] ApiRequestDto<Category> request,
        CancellationToken cancellationToken
    )
    {
        ApiResponseDto<Category> result = await _categoryService.UpdateCategoryAsync(id, request, cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    // DELETE /api/categories/{id}
    /// <summary>
    /// Soft-deletes a category by id.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        ApiResponseDto<bool> result = await _categoryService.DeleteCategoryAsync(id, cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return NoContent();
    }

    // GET /api/categories/deleted
    /// <summary>
    /// Lists categories that have been soft-deleted.
    /// </summary>
    [HttpGet("deleted")]
    [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetDeletedCategories(CancellationToken cancellationToken)
    {
        ApiResponseDto<List<Category>> result = await _categoryService.GetDeletedCategoriesAsync(cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    // POST /api/categories/{id}/restore
    /// <summary>
    /// Restores a previously soft-deleted category.
    /// </summary>
    [HttpPost("{id:int}/restore")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Restore(int id, CancellationToken cancellationToken)
    {
        ApiResponseDto<bool> result = await _categoryService.RestoreCategoryAsync(id, cancellationToken);
        if (result.RequestFailed)
            return this.FromFailure(result.ResponseCode, result.ErrorMessage);
        return Ok(result);
    }

    // POST /api/v1/categories/{id}/restorations
    // Noun-based alternative to "restore".
    [HttpPost("{id:int}/restorations")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public Task<IActionResult> CreateRestoration(int id, CancellationToken cancellationToken) =>
        Restore(id, cancellationToken);
}
