using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.AddressMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.OtherMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayCustomerAddressForAdminViewModel : ObservableObject
{
    public int AddressId { get; }
    public string? CustomerId { get; }

    [ObservableProperty]
    public AddressData? _address;

    private readonly IAddressService _addressService;
    private readonly IMessenger _messenger;

    public DisplayCustomerAddressForAdminViewModel(
        IAddressService addressService, 
        int addressId, 
        string? customerId, 
        IMessenger messenger)
    {
        _addressService = addressService;
        AddressId = addressId;
        CustomerId = customerId;
        _messenger = messenger;
    }

    public async Task GetAddressAsync()
    {
        Address = await _addressService.GetCustomerAddressForAdminAsync(AddressId, CustomerId);
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToPreviousPageMessage());
    }
}
