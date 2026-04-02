using CommunityToolkit.Mvvm.ComponentModel;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private object? _currentView;
}
