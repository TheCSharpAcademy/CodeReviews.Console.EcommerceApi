using Ecommerce.Core.DTOs.Auth;
using Ecommerce.Core.Utilities;

namespace Ecommerce.Core.Interfaces.Services;

public interface IAuthService
{
    Task<Result<string>> Register(RegisterDto registerDto);
    Task<Result<AuthResponseDto>> Login(LoginDto loginDto);
}