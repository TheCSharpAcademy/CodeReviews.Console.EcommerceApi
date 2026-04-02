using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.AddressMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.OtherMessages;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayAddedAddressViewModel : ViewModelBase
{
    private readonly IMessenger _messenger;
    public AddressData Address { get; }

    public DisplayAddedAddressViewModel(AddressData address, IMessenger messenger)
    {
        Address = address;
        _messenger = messenger;
    }

    [RelayCommand]
    private void AddAnotherAddress()
    {
        _messenger.Send(new AddAddressMessage());
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new DisplayCustomerProfileMessage());
    }

}
