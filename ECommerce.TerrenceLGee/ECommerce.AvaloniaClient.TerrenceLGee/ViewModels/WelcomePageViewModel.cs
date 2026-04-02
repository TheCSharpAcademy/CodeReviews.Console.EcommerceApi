using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class WelcomePageViewModel : ObservableObject
{
    public event Action? LoginRequested;
    public event Action? RegistrationRequested;
    public event Action? PasswordResetRequested;

    [RelayCommand]
    private void NavigateToLogin()
    {
        LoginRequested?.Invoke();
    }

    [RelayCommand]
    private void NavigateToRegistration()
    {
        RegistrationRequested?.Invoke();
    }

    [RelayCommand]
    private void NavigateToPasswordReset()
    {
        PasswordResetRequested?.Invoke();
    }

    [RelayCommand]
    private void Exit()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }
}
