using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;
using ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class PasswordResetViewModel : ObservableValidator
{
    private readonly IAuthService _authService;
    public event Action? BackRequested;
    public event Action? LoginRequested;

    public PasswordResetViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    [Required(ErrorMessage = "Email address is required.")]
    [NotifyPropertyChangedFor(nameof(EmailErrors))]
    private string _email;

    public string? EmailErrors => GetErrors(nameof(Email))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Old password is required.")]
    [NotifyPropertyChangedFor(nameof(OldPasswordErrors))]
    private string _oldPassword;

    public string? OldPasswordErrors => GetErrors(nameof(OldPassword))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "New password is required.")]
    [NotifyPropertyChangedFor(nameof(NewPasswordErrors))]
    private string _newPassword;

    public string? NewPasswordErrors => GetErrors(nameof(NewPassword))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Confirmation of new password is required.")]
    [CustomValidation(typeof(PasswordResetViewModel), nameof(ValidatePasswordResetConfirmation))]
    [NotifyPropertyChangedFor(nameof(ResetConfirmPasswordErrors))]
    private string _resetConfirmPassword;

    public string? ResetConfirmPasswordErrors => GetErrors(nameof(ResetConfirmPassword))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    private string? _successMessage;
    [ObservableProperty]
    private string? _errorMessage;

    [RelayCommand]
    private async Task ResetPassword()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        ClearErrors();

        ValidateProperty(Email, nameof(Email));
        ValidateProperty(OldPassword, nameof(OldPassword));
        ValidateProperty(NewPassword, nameof(NewPassword));
        ValidateProperty(ResetConfirmPassword, nameof(ResetConfirmPassword));

        if (HasErrors)
        {
            return;
        }

        var reset = new UserResetPasswordDto
        {
            Email = Email,
            OldPassword = OldPassword,
            NewPassword = NewPassword,
            ConfirmPassword = ResetConfirmPassword
        };

        var (success, message) = await _authService.ResetUserPasswordAsync(reset);

        if (success)
        {
            SuccessMessage = message;
            ClearPasswordReset();
            LoginRequested?.Invoke();
        }
        else
        {
            ErrorMessage = message;
            ClearPasswordReset();
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        BackRequested?.Invoke();
    }

    public static ValidationResult? ValidatePasswordResetConfirmation(string confirmPassword, ValidationContext context)
    {
        var viewModel = (PasswordResetViewModel)context.ObjectInstance;

        var password = viewModel.NewPassword;

        if (!password.Equals(confirmPassword))
        {
            return new ValidationResult("Passwords do not match.");
        }

        return ValidationResult.Success;
    }

    private void ClearPasswordReset()
    {
        Email = string.Empty;
        OldPassword = string.Empty;
        NewPassword = string.Empty;
        ResetConfirmPassword = string.Empty;
    }
}
