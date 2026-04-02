using ECommerce.Api.TerrenceLGee.Controllers.Helpers;
using ECommerce.Api.TerrenceLGee.Responses;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.TerrenceLGee.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CategoriesController(ICategoryService categoryService, UserManager<ApplicationUser> userManager)
    {
        _categoryService = categoryService;
        _userManager = userManager;
    }

    [HttpPost("admin/add")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<RetrievedCategoryForAdminDto?>>> AddCategory([FromBody] CreateCategoryDto category)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedCategoryForAdminDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var response = ApiResponse<RetrievedCategoryForAdminDto?>.GetEmptyResponse;

        var result = await _categoryService.AddCategoryAsync(category);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedCategoryForAdminDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedCategoryForAdminDto?>(201, result.Value);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("admin/update/{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<RetrievedCategoryForAdminDto?>>> UpdateCategory([FromBody] UpdateCategoryDto category, [FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedCategoryForAdminDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        category.Id = id;

        var response = ApiResponse<RetrievedCategoryForAdminDto?>.GetEmptyResponse;

        var result = await _categoryService.UpdateCategoryAsync(category);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedCategoryForAdminDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedCategoryForAdminDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<RetrievedCategoryDto?>>> GetCategoryById([FromRoute] int id)
    {
        var response = ApiResponse<RetrievedCategoryDto?>.GetEmptyResponse;

        var categoryParams = new CategoryParams { CategoryId = id };

        var result = await _categoryService.GetCategoryAsync(categoryParams);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedCategoryDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedCategoryDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("admin/{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<RetrievedCategoryForAdminDto?>>> GetCategoryByIdForAdmin([FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedCategoryForAdminDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var response = ApiResponse<RetrievedCategoryForAdminDto?>.GetEmptyResponse;

        var categoryParams = new CategoryParams { CategoryId = id };

        var result = await _categoryService.GetCategoryForAdminAsync(categoryParams);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedCategoryForAdminDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedCategoryForAdminDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponsePaged<RetrievedCategorySummaryDto>>> GetCategories([FromQuery] CategoryQueryParams categoryQueryParams)
    {
        var result = await _categoryService.GetCategoriesAsync(categoryQueryParams);

        var response = new ApiResponsePaged<RetrievedCategorySummaryDto>(200, result.Value!);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponsePaged<RetrievedCategorySummaryForAdminDto>>> GetCategoriesForAdmin([FromQuery] CategoryQueryParams categoryQueryParams)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationPagedAsync<RetrievedCategorySummaryForAdminDto>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }
        var result = await _categoryService.GetCategoriesForAdminAsync(categoryQueryParams);

        var response = new ApiResponsePaged<RetrievedCategorySummaryForAdminDto>(200, result.Value!);

        return StatusCode(response.StatusCode, response);
    }

    private async Task<(bool isUserValid, ActionResult<ApiResponse<T?>> response)> UserValidationAsync<T>(string? userId)
    {
        var (isValidUser, errorResponse) = await AuthHelper.IsValidUserAsync<T?>(_userManager, userId);

        if (!isValidUser)
        {
            return (false, StatusCode(errorResponse.StatusCode, errorResponse));
        }

        return (true, null!);
    }

    private async Task<(bool isUserValid, ActionResult<ApiResponsePaged<T>> response)> UserValidationPagedAsync<T>(string? userId)
    {
        var (isValidUser, errorResponse) = await AuthHelper.IsValidUserPagedAsync<T?>(_userManager, userId);

        if (!isValidUser)
        {
            return (false, StatusCode(errorResponse.StatusCode, errorResponse));
        }

        return (true, null!);
    }
}
