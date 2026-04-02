using ECommerce.Api.TerrenceLGee.Controllers.Helpers;
using ECommerce.Api.TerrenceLGee.Responses;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.CustomerDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CustomerParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.TerrenceLGee.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomersController(ICustomerService customerService, UserManager<ApplicationUser> userManager)
    {
        _customerService = customerService;
        _userManager = userManager;
    }

    [HttpGet("profile")]
    [Authorize(Roles = "admin,customer")]
    public async Task<ActionResult<ApiResponse<RetrievedCustomerDto?>>> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedCustomerDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var customerRetrieval = new CustomerRetrievalDto
        {
            CustomerId = userId
        };

        var response = ApiResponse<RetrievedCustomerDto?>.GetEmptyResponse;

        var result = await _customerService.GetCustomerProfileAsync(customerRetrieval);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedCustomerDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedCustomerDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponsePaged<RetrievedCustomerDto>>> GetCustomersForAdmin([FromQuery] CustomerQueryParams customerQueryParams)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isUserValid, errorResponse) = await UserValidationPagedAsync<RetrievedCustomerDto>(userId);

        if (!isUserValid)
        {
            return errorResponse;
        }

        var result = await _customerService.GetAllCustomersForAdminAsync(customerQueryParams);

        var response = new ApiResponsePaged<RetrievedCustomerDto>(200, result.Value!);

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
