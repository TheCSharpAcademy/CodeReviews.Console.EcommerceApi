using ECommerce.Api.TerrenceLGee.Controllers.Helpers;
using ECommerce.Api.TerrenceLGee.Responses;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.TerrenceLGee.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProductsController(IProductService productService, UserManager<ApplicationUser> userManager)
    {
        _productService = productService;
        _userManager = userManager;
    }

    [HttpPost("admin/add")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<RetrievedProductForAdminDto?>>> AddProduct([FromBody] CreateProductDto product)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedProductForAdminDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var response = ApiResponse<RetrievedProductForAdminDto?>.GetEmptyResponse;

        var result = await _productService.AddProductAsync(product);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedProductForAdminDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedProductForAdminDto?>(201, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("admin/update/{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<RetrievedProductForAdminDto?>>> UpdateProduct([FromBody] UpdateProductDto product, [FromRoute ]int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedProductForAdminDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        product.Id = id;

        var response = ApiResponse<RetrievedProductForAdminDto?>.GetEmptyResponse;

        var result = await _productService.UpdateProductAsync(product);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedProductForAdminDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedProductForAdminDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("admin/delete/{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<string?>>> DeleteProduct([FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<string?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var productParams = new ProductParams { ProductId = id };

        var response = ApiResponse<string?>.GetEmptyResponse;

        var result = await _productService.DeleteProductAsync(productParams);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<string?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string?>(200, $"Product {id} successfully marked as deleted.");

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("admin/restore/{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<string?>>> RestoreProduct([FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<string?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var productParams = new ProductParams { ProductId = id };

        var response = ApiResponse<string?>.GetEmptyResponse;

        var result = await _productService.RestoreProductAsync(productParams);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<string?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string?>(200, $"Product {id} successfully restored.");

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<RetrievedProductDto?>>> GetProduct([FromRoute] int id)
    {
        var response = ApiResponse<RetrievedProductDto?>.GetEmptyResponse;

        var productParams = new ProductParams { ProductId = id };

        var result = await _productService.GetProductAsync(productParams);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedProductDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedProductDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponsePaged<RetrievedProductDto>>> GetProducts([FromQuery] ProductQueryParams productQueryParams)
    {
        var result = await _productService.GetProductsAsync(productQueryParams);

        var response = new ApiResponsePaged<RetrievedProductDto>(200, result.Value!);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("admin/{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<RetrievedProductForAdminDto?>>> GetProductForAdmin([FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedProductForAdminDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var productParams = new ProductParams { ProductId = id };

        var response = ApiResponse<RetrievedProductForAdminDto?>.GetEmptyResponse;

        var result = await _productService.GetProductForAdminAsync(productParams);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedProductForAdminDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedProductForAdminDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponsePaged<RetrievedProductForAdminDto>>> GetProductsForAdmin([FromQuery] ProductQueryParams productQueryParams)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationPagedAsync<RetrievedProductForAdminDto>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var result = await _productService.GetProductsForAdminAsync(productQueryParams);

        var response = new ApiResponsePaged<RetrievedProductForAdminDto>(200, result.Value!);

        return StatusCode(response.StatusCode, response);
    }

    private async Task<(bool isUserValid, ActionResult<ApiResponse<T?>>)> UserValidationAsync<T>(string? userId)
    {
        var (isValidUser, errorResponse) = await AuthHelper.IsValidUserAsync<T?>(_userManager, userId);

        if (!isValidUser)
        {
            return (false, StatusCode(errorResponse.StatusCode, errorResponse));
        }

        return (true, null!);
    }

    private async Task<(bool isUserValid, ActionResult<ApiResponsePaged<T>>)> UserValidationPagedAsync<T>(string? userId)
    {
        var (isValidUser, errorResponse) = await AuthHelper.IsValidUserPagedAsync<T?>(_userManager, userId);

        if (!isValidUser)
        {
            return (false, StatusCode(errorResponse.StatusCode, errorResponse));
        }

        return (true, null!);
    }
}
