using ECommerce.Api.TerrenceLGee.Controllers.Helpers;
using ECommerce.Api.TerrenceLGee.Responses;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.TerrenceLGee.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<string?>>> Register([FromBody] UserRegistrationDto userDto)
    {
        var response = ApiResponse<string?>.GetEmptyResponse;

        var result = await _authService.RegisterUserAsync(userDto);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<string?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string?>(200, "Registration successful");
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthenticationResponseDto?>>> Login([FromBody] UserLoginDto loginDto)
    {
        var response = ApiResponse<AuthenticationResponseDto?>.GetEmptyResponse;

        var result = await _authService.LoginUserAsync(loginDto);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<AuthenticationResponseDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<AuthenticationResponseDto?>(200, result.Value);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("reset")]
    public async Task<ActionResult<ApiResponse<string?>>> ResetPassword([FromBody] UserResetPasswordDto resetDto)
    {
        var response = ApiResponse<string?>.GetEmptyResponse;

        var result = await _authService.ResetPasswordAsync(resetDto);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<string?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string?>(200, $"Password reset successful");
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<string?>>> Logout()
    {
        var response = ApiResponse<string?>.GetEmptyResponse;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            response = new ApiResponse<string?>(404, ["User not found"]);
            return StatusCode(response.StatusCode, response);
        }

        var result = await _authService.LogoutUserAsync(new UserLogoutDto { UserId = userId });

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<string?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string?>(200, "Logout successful");
        return StatusCode(response.StatusCode, response);
    }
}
