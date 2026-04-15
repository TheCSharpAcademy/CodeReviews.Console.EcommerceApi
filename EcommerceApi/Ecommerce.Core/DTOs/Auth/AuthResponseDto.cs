namespace Ecommerce.Core.DTOs.Auth;

public record AuthResponseDto(string Token, DateTime Expires);