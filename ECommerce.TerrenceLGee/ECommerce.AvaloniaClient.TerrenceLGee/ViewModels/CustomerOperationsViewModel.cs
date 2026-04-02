using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.Customer;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class CustomerOperationsViewModel : ViewModelBase
{
    private readonly IMessenger _messenger;

    public CustomerOperationsViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }

    [RelayCommand]
    private void ViewCustomers()
    {
        _messenger.Send(new ViewCustomersForAdminMessage());
    }
}
