using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces;

public class AuthTokenHolder : IAuthTokenHolder
{
    public string? Token { get; private set; }
    public void SetToken(string? token) => Token = token;
}
