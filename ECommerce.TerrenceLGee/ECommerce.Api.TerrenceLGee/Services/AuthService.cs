using ECommerce.Api.TerrenceLGee.Data;
using ECommerce.Api.TerrenceLGee.Data.Configuration;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Contracts.TerrenceLGee.Mappings.AddressMappings;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.Api.TerrenceLGee.Services;

public class AuthService : IAuthService
{
    private readonly IOptions<AuthConfiguration> _authOptions;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ECommerceDbContext _context;
    private readonly AuthConfiguration _authConfiguration;
    private readonly ILogger<AuthService> _logger;
    private string _errorMessage = string.Empty;

    public AuthService(
        IOptions<AuthConfiguration> authOptions,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        ECommerceDbContext context,
        ILogger<AuthService> logger)
    {
        _authOptions = authOptions;
        _authConfiguration = _authOptions.Value;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _context = context;
        _logger = logger;
    }

    public async Task<Result> RegisterUserAsync(UserRegistrationDto user)
    {
        try
        {
            var existingUser = await _userManager
                .FindByEmailAsync(user.Email);

            if (existingUser is not null)
            {
                return Result.Fail("User already exists. Unable to register a previously registered user.", ErrorType.Conflict);
            }

            var newUser = new ApplicationUser
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                RegistrationDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                Email = user.Email,
                UserName = user.Email,
                Addresses = new List<Address>
                {
                    (user.BillingAddress is not null) ? user.BillingAddress.FromCreateAddressDto() : new Address(),
                    (user.ShippingAddress is not null) ? user.ShippingAddress.FromCreateAddressDto() : new Address()
                }
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (!result.Succeeded)
            {
                return Result.Fail($"Unable to register new user at this time. Please check your input and try again later.", ErrorType.BadRequest);
            }

            result = await _userManager.AddToRoleAsync(newUser, "customer");

            if (!result.Succeeded)
            {
                return Result.Fail("Unable to add user as a customer. Please try again later", ErrorType.BadRequest);
            }

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {RegisterUserAsync}\n" +
                $"An unexpected error occurred while registering the new user: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return Result.Fail($"An unexpected error occurred while registering the new user", ErrorType.InternalServerError);
        }
    }

    public async Task<Result<AuthenticationResponseDto?>> LoginUserAsync(UserLoginDto userDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(userDto.Email);

            if (user is null)
            {
                return Result<AuthenticationResponseDto?>.Fail("User not found", ErrorType.NotFound);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);

            if (!result.Succeeded)
            {
                return Result<AuthenticationResponseDto?>.Fail("Login failed", ErrorType.BadRequest);
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "customer";

            var role = await _roleManager.FindByNameAsync(userRole);
            var roleClaims = (role is not null)
                ? await _roleManager.GetClaimsAsync(role)
                : [];

            var jwtId = string.Empty;

            var jwtToken = GenerateJwtToken(user, userRole, roleClaims, out jwtId);
            var refreshToken = GenerateRefreshToken(jwtId, user.Id);

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            var response = new AuthenticationResponseDto
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token,
                Roles = roles.ToList()
            };

            return Result<AuthenticationResponseDto?>.Ok(response);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {LoginUserAsync}\n" +
                $"An unexpected error occurred during user login: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return Result<AuthenticationResponseDto?>.Fail("An unexpected error occurred during user login", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> ResetPasswordAsync(UserResetPasswordDto userDto)
    {
        try
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => !string.IsNullOrEmpty(u.Email) && u.Email.ToLower().Equals(userDto.Email.ToLower()));

            if (user is null)
            {
                return Result.Fail("User not found", ErrorType.NotFound);
            }

            var result = await _userManager.ChangePasswordAsync(user, userDto.OldPassword, userDto.NewPassword);

            if (!result.Succeeded)
            {
                return Result.Fail($"Unable to change password for {user.FirstName} {user.LastName}", ErrorType.BadRequest);
            }

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(ResetPasswordAsync)}\n" +
                $"There was an unexpected error attempting to change the password for user {userDto.Email}: " +
                $"{ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return Result.Fail("Unexpected error occurred while changing your password", ErrorType.InternalServerError);
        }
    }

    public async Task<Result> LogoutUserAsync(UserLogoutDto userDto)
    {
        try
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId.Equals(userDto.UserId) && !rt.IsRevoked);

            if (refreshToken is null)
            {
                return Result.Fail("Unable to proceed with logout. Invalid authorization", ErrorType.Unauthorized);
            }

            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {LogoutUserAsync}\n" +
                $"An unexpected error occurred during user logout: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return Result.Fail("Unexpected error occurred during user logout.", ErrorType.InternalServerError);
        }

    }

    private string GenerateJwtToken(
        ApplicationUser user,
        string userRole,
        IList<Claim> roleClaims,
        out string jwtId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfiguration.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        jwtId = Guid.NewGuid().ToString();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Iss, _authConfiguration.Issuer),
            new Claim(JwtRegisteredClaimNames.Aud, _authConfiguration.Audience),
            new Claim("role", userRole)
        };

        foreach (var roleClaim in roleClaims)
        {
            claims.Add(new Claim(roleClaim.Type, roleClaim.Value));
        }

        var token = new JwtSecurityToken(
            issuer: _authConfiguration.Issuer,
            audience: _authConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken(string jwtId, string userId)
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        var refreshTokenExpirationDays = _authConfiguration.RefreshTokenExpirationDays;

        return new RefreshToken
        {
            Token = Convert.ToBase64String(randomBytes),
            JwtId = jwtId,
            Expires = DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            IsRevoked = false,
            RevokedAt = null
        };
    }

    
}
