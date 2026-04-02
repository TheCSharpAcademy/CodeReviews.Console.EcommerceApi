namespace ECommerce.Api.TerrenceLGee.Data.Configuration;

public class AuthConfiguration
{
    public string Section { get; set; } = "JwtConfiguration";
    public string Key { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int RefreshTokenExpirationDays { get; init; }
}
