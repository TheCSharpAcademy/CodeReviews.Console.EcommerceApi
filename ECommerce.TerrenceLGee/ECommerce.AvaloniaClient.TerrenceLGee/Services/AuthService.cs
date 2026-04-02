using ECommerce.AvaloniaClient.TerrenceLGee.Data;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Auth;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;
using ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<AuthService> _logger;
    private readonly IAuthTokenHolder _tokenHolder;
    public string? JwtToken { get; set; }
    private readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    private string _errorMessage = string.Empty;
    private const string ClientName = "client";
    private const string MediaType = "application/json";
    private const string LogErrorString = "{msg}\n\n";

    public AuthService(IHttpClientFactory clientFactory, ILogger<AuthService> logger, IAuthTokenHolder tokenHolder)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _tokenHolder = tokenHolder;
    }

    public async Task<(bool, string?)> RegisterUserAsync(UserRegistrationDto userDto)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.RegisterUrl}";

            var content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var registrationResponse = JsonSerializer.Deserialize<RegistrationRoot>(responseContent, options);

            if (registrationResponse is null)
            {
                return (false, "Unable to register user at this time.");
            }

            if (!registrationResponse.IsSuccess || registrationResponse.StatusCode != 200)
            {
                var errors = string.Join('\n', registrationResponse.Errors);
                return (false, errors);
            }

            return (true, registrationResponse.Data);
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(RegisterUserAsync)}\n" +
                $"There was an API error attempting to register a new user: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, "Registration request failed.");
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(RegisterUserAsync)}\n" +
                $"There was an unexpected error attempting to register a new user: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, "Unexpected error during user registration\nRegistration failed");
        }
    }

    public async Task<(bool, AuthData?)> LoginUserAsync(UserLoginDto userDto)
    {
        var authDataForLoginFailure = new AuthData();
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.LoginUrl}";

            var content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                _tokenHolder.SetToken(null);
                authDataForLoginFailure.ErrorMessage = $"Login failed\nReason: {response.ReasonPhrase}.";
                return (false, authDataForLoginFailure);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<AuthRoot>(responseContent, options);

            if (loginResponse is null)
            {
                _tokenHolder.SetToken(null);
                authDataForLoginFailure.ErrorMessage = "Unable to login at this time.";
                return (false, authDataForLoginFailure);
            }

            if (loginResponse.Data?.AccessToken is null)
            {
                _tokenHolder.SetToken(null);
                authDataForLoginFailure.ErrorMessage = $"Unable to retrieve valid authorization credientals.";
                return (false, authDataForLoginFailure);
            }

            _tokenHolder.SetToken(loginResponse.Data.AccessToken);
            JwtToken = loginResponse.Data.AccessToken;
            return (true, loginResponse.Data);
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(LoginUserAsync)}\n" +
                $"There was an API error during the user's login attempt: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            authDataForLoginFailure.ErrorMessage = "Error occurred during connection to the endpoint\nLogin failed.";
            return (false, authDataForLoginFailure);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(LoginUserAsync)}\n" +
                $"There was an unexpected error during the user's login attempt: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            authDataForLoginFailure.ErrorMessage = "Unable to connect\nLogin failed.";
            return (false, authDataForLoginFailure);
        }
    }

    public async Task<(bool, string?)> ResetUserPasswordAsync(UserResetPasswordDto userDto)
    {
        try
        {
            var httpClient = _clientFactory.CreateClient();
            var url = $"{Urls.BaseUrl}{Urls.PasswordResetUrl}";

            var content = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, MediaType);

            var response = await httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var resetResponse = JsonSerializer.Deserialize<PasswordResetRoot>(responseContent, options);

            if (resetResponse is null)
            {
                return (false, $"Unable to reset password, please try again later");
            }

            if (!resetResponse.IsSuccess || resetResponse.StatusCode != 200)
            {
                return (false, $"{string.Join('\n', resetResponse.Errors)}");
            }

            return (true, resetResponse.Data);
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(ResetUserPasswordAsync)}\n" +
                $"There was an API error during the user's password reset attempt: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, "Error occurred during connection to the endpoint\nPassword reset failed.");
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(ResetUserPasswordAsync)}\n" +
                $"There was an unexpected error during the user's password reset attempt: {ex.Message}";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return (false, "Unable to connect\nPassword reset failed.");
        }
    }

    public async Task<bool> LogoutUserAsync()
    {
        try
        {
            var httpClient = _clientFactory.CreateClient(ClientName);
            var url = $"{Urls.BaseUrl}{Urls.LogoutUrl}";

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode) return false;

            var responseContent = await response.Content.ReadAsStringAsync();
            var logoutResponse = JsonSerializer.Deserialize<LogoutRoot>(responseContent, options);

            if (logoutResponse is null || string.IsNullOrEmpty(logoutResponse.Data)) return false;

            if (!logoutResponse.IsSuccess || logoutResponse.StatusCode != 200) return false;

            return true;
        }
        catch (HttpRequestException ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(LogoutUserAsync)}\n" +
                $"There was an API error logging the user out of the system: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return false;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(AuthService)}\n" +
                $"Method: {nameof(LogoutUserAsync)}\n" +
                $"There was an unexpected error logging the user out of the system: {ex.Message}\n";
            _logger.LogError(ex, LogErrorString, _errorMessage);
            return false;
        }
    }
}