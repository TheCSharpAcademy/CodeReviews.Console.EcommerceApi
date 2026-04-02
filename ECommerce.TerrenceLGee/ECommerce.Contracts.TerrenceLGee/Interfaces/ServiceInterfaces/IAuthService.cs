using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;

public interface IAuthService
{
    Task<Result> RegisterUserAsync(UserRegistrationDto user);
    Task<Result<AuthenticationResponseDto?>> LoginUserAsync(UserLoginDto userDto);
    Task<Result> LogoutUserAsync(UserLogoutDto userDto);
    Task<Result> ResetPasswordAsync(UserResetPasswordDto userDto);
}
