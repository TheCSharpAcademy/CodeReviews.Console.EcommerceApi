namespace ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;

public interface IAuthTokenHolder
{
    string? Token { get; }
    void SetToken(string? token);
}
