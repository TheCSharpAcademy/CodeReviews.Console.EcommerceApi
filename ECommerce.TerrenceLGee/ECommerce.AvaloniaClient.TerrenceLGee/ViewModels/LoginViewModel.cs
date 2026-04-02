using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;
using ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class LoginViewModel : ObservableValidator
{
    private readonly IAuthService _authService;
    public event Action<bool>? LoginSuccessful;
    public event Action? BackRequested;

    [ObservableProperty]
    private string? _successMessage;
    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Email address is required.")]
    [NotifyPropertyChangedFor(nameof(EmailErrors))]
    private string _email;

    public string? EmailErrors => GetErrors(nameof(Email))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Password is required.")]
    [NotifyPropertyChangedFor(nameof(PasswordErrors))]
    private string _password;

    public string? PasswordErrors => GetErrors(nameof(Password))
        .FirstOrDefault()?.ErrorMessage;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [RelayCommand]
    public async Task LoginAsync()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        ClearErrors();

        ValidateProperty(Email, nameof(Email));
        ValidateProperty(Password, nameof(Password));

        if (HasErrors)
        {
            return;
        }

        var login = new UserLoginDto
        {
            Email = Email,
            Password = Password
        };

        var (success, data) = await _authService.LoginUserAsync(login);

        if (success)
        {
            ClearLogin();
            SuccessMessage = "Login successful";

            var isAdmin = (data is not null)
                ? data.Roles.Contains("admin")
                : false;

            LoginSuccessful?.Invoke(isAdmin);
        }
        else
        {
            ClearLogin();
            ErrorMessage = (data is not null)
                ? data.ErrorMessage
                : "Unexpected error occurred while attempting to login";
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        BackRequested?.Invoke();
    }

    private void ClearLogin()
    {
        Email = string.Empty;
        Password = string.Empty;
    }
}
