using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce.Core.DTOs.Auth;
using Ecommerce.Core.Interfaces.Services;
using Ecommerce.Core.Models;
using Ecommerce.Core.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    IConfiguration configuration
    ) : IAuthService
{
    public async Task<Result<string>> Register(RegisterDto registerDto)
    {
        ApplicationUser user = new()
        {
            UserName = registerDto.Username,
            Email = registerDto.Email
        };
        var result = await userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("\n", result.Errors.Select(x => $"{x.Code} => {x.Description}"));
            return Result<string>.Fail(errors);
        }

        return Result<string>.Success("User created successfully");
    }

    public async Task<Result<AuthResponseDto>> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByEmailAsync(loginDto.Email);
        if (user is null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            return Result<AuthResponseDto>.Fail("Invalid email or password");

        var userRoles = await userManager.GetRolesAsync(user);

        var userClaims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Name, user.UserName!)
        };

        foreach (var role in userRoles)
            userClaims.Add(new Claim(ClaimTypes.Role, role));

        var token = GenerateJwt(userClaims);

        return Result<AuthResponseDto>.Success(
            new AuthResponseDto(
                new JwtSecurityTokenHandler().WriteToken(token),
                token.ValidTo
            )
        );

        JwtSecurityToken GenerateJwt(List<Claim> claims)
        {
            SymmetricSecurityKey authSecret = new(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!));

            var signingCredentials =
                new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(configuration.GetValue<int>("Jwt:ExpirationInHours")),
                signingCredentials: signingCredentials
            );
        }
    }
}