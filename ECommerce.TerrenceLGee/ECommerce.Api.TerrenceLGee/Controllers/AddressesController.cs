using ECommerce.Api.TerrenceLGee.Controllers.Helpers;
using ECommerce.Api.TerrenceLGee.Responses;
using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.AddressParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.TerrenceLGee.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressesController : ControllerBase
{
    private readonly IAddressService _addressService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AddressesController(
        IAddressService addressService,
        UserManager<ApplicationUser> userManager)
    {
        _addressService = addressService;
        _userManager = userManager;
    }

    [HttpPost("add")]
    [Authorize(Roles = "customer")]
    public async Task<ActionResult<ApiResponse<RetrievedAddressDto?>>> AddAddress([FromBody] CreateAddressDto address)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedAddressDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        address.CustomerId = userId;

        var result = await _addressService.AddAddressAsync(address);

        var response = ApiResponse<RetrievedAddressDto?>.GetEmptyResponse;

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedAddressDto?>(result);

            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedAddressDto?>(201, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("update/{id:int}")]
    [Authorize(Roles = "customer")]
    public async Task<ActionResult<ApiResponse<RetrievedAddressDto?>>> UpdateAddress(
        [FromBody] UpdateAddressDto address, 
        [FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedAddressDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        address.Id = id;
        address.CustomerId = userId;

        var result = await _addressService.UpdateAddressAsync(address);

        var response = ApiResponse<RetrievedAddressDto?>.GetEmptyResponse;

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedAddressDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedAddressDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "customer")]
    public async Task<ActionResult<ApiResponse<string?>>> DeleteAddress([FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<string?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var addressIdDto = new AddressIdDto
        {
            Id = id,
            CustomerId = userId
        };

        var result = await _addressService.DeleteAddressAsync(addressIdDto);

        var response = ApiResponse<string?>.GetEmptyResponse;

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<string?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string?>(200, "Address deleted successfully");

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "customer")]
    public async Task<ActionResult<ApiResponse<RetrievedAddressDto?>>> GetAddress([FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedAddressDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var addressIdDto = new AddressIdDto
        {
            Id = id,
            CustomerId = userId
        };

        var result = await _addressService.GetAddressAsync(addressIdDto);

        var response = ApiResponse<RetrievedAddressDto?>.GetEmptyResponse;

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedAddressDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedAddressDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("admin/{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<RetrievedAddressDto?>>> GetCustomerAddressForAdmin([FromRoute] int id, [FromBody] AddressIdDto addressIdDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedAddressDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        addressIdDto.Id = id;

        var result = await _addressService.GetAddressAsync(addressIdDto);

        var response = ApiResponse<RetrievedAddressDto?>.GetEmptyResponse;

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedAddressDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedAddressDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [Authorize(Roles = "admin,customer")]
    public async Task<ActionResult<ApiResponsePaged<RetrievedAddressDto>>> GetAddressesForCustomer([FromQuery] AddressQueryParams queryParams)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationPagedAsync<RetrievedAddressDto>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var userRole = User.FindFirstValue(ClaimTypes.Role);

        if (!userRole!.Equals("admin", StringComparison.OrdinalIgnoreCase))
        {
            queryParams.CustomerId = userId;
        }

        var result = await _addressService.GetCustomerAddressesAsync(queryParams);

        var response = new ApiResponsePaged<RetrievedAddressDto>(200, result.Value!);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponsePaged<RetrievedAddressDto>>> GetAllCustomerAddressesForAdmin([FromQuery] AddressQueryParams queryParams)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationPagedAsync<RetrievedAddressDto>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var result = await _addressService.GetAllCustomerAddressesForAdminAsync(queryParams);

        var response = new ApiResponsePaged<RetrievedAddressDto>(200, result.Value!);

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
