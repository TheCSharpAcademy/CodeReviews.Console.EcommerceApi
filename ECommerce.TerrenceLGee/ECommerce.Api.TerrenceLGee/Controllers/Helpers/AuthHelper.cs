using ECommerce.Api.TerrenceLGee.Responses;
using ECommerce.Entities.TerrenceLGee.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.TerrenceLGee.Controllers.Helpers;

public static class AuthHelper
{
    public static async Task<(bool isValid, ApiResponse<T?> response)> IsValidUserAsync<T>(
        UserManager<ApplicationUser> 
        userManager, string? userId)
    {
        ApiResponse<T?> response;

        if (string.IsNullOrEmpty(userId))
        {
            response = new ApiResponse<T?>(404, ["User Id not found"]);
            return (false, response);
        }

        var userState = await IsUserValidAndAuthorized(userManager, userId);

        if (userState != UserState.Authorized)
        {
            response = userState switch
            {
                UserState.NotFound => new ApiResponse<T?>(404, ["User not found"]),
                UserState.Unauthorized => new ApiResponse<T?>(401, ["Unauthorized"]),
                _ => new ApiResponse<T?>(400, ["Bad Request"])
            };
            return (false, response);
        }
        return (true, null!);
    }

    public static async Task<(bool isValid, ApiResponsePaged<T> response)> IsValidUserPagedAsync<T>(
        UserManager<ApplicationUser> userManager, 
        string? userId)
    {
        ApiResponsePaged<T> response;

        if (string.IsNullOrEmpty(userId))
        {
            response = new ApiResponsePaged<T>(404, ["User Id not found"]);
            return (false, response);
        }

        var userState = await IsUserValidAndAuthorized(userManager, userId);

        if (userState != UserState.Authorized)
        {
            response = userState switch
            {
                UserState.NotFound => new ApiResponsePaged<T>(404, ["User not found"]),
                UserState.Unauthorized => new ApiResponsePaged<T>(401, ["Unauthorized"]),
                _ => new ApiResponsePaged<T>(400, ["Bad Request"])
            };

            return (false, response);
        }

        return (true, null!);
    }

    private static async Task<UserState> IsUserValidAndAuthorized(UserManager<ApplicationUser> userManager, string? userId)
    {
        var user = await userManager.Users
            .Where(u => u.Id.Equals(userId))
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync();

        if (user is null) return UserState.NotFound;

        var refreshToken = user.RefreshTokens
            .LastOrDefault();

        if (refreshToken is null || refreshToken.IsRevoked) return UserState.Unauthorized;

        return UserState.Authorized;
    }
}

public enum UserState
{
    Authorized,
    Unauthorized,
    NotFound
}
