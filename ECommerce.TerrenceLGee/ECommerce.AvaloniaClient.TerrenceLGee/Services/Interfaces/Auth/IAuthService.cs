using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Auth;
using ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;

public interface IAuthService
{
    string? JwtToken { get; set; }
    Task<(bool, string?)> RegisterUserAsync(UserRegistrationDto userDto);
    Task<(bool, AuthData?)> LoginUserAsync(UserLoginDto userDto);
    Task<(bool, string?)> ResetUserPasswordAsync(UserResetPasswordDto userDto);
    Task<bool> LogoutUserAsync();
}
