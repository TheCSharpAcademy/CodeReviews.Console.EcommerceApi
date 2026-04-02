using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.AddressMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.OtherMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayAddressViewModel : ViewModelBase
{
    private readonly IAddressService _addressService;
    private readonly IMessenger _messenger;
    private readonly int _addressId;
    [ObservableProperty]
    private AddressData? _address;

    public DisplayAddressViewModel(IAddressService addressService, int addressId, IMessenger messenger)
    {
        _addressService = addressService;
        _addressId = addressId;
        _messenger = messenger;
    }

    public async Task GetAddressAsync()
    {
        Address = await _addressService.GetAddressAsync(_addressId);

        if (Address is null)
        {
            _messenger.Send(new NavigateBackToPreviousPageMessage());
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        _messenger.Send(new DisplayCustomerProfileMessage());
    }

    [RelayCommand]
    private void UpdateAddress()
    {
        _messenger.Send(new AddressSelectedForUpdateMessage(Address!));
    }

    [RelayCommand]
    private async Task DeleteAddress()
    {
        var box = MessageBoxManager
                .GetMessageBoxStandard("Delete", $"Delete Address?", ButtonEnum.YesNo, Icon.Warning,
                null, WindowStartupLocation.CenterOwner);

        var result = await box.ShowAsync();

        if (result == ButtonResult.Yes)
        {
            var (success, data) = await _addressService.DeleteAddressAsync(Address!.Id);

            if (success)
            {
                box = MessageBoxManager
                    .GetMessageBoxStandard("Success", $"{data}", ButtonEnum.Ok, Icon.Success,
                    null, WindowStartupLocation.CenterOwner);
                result = await box.ShowAsync();
                if (result == ButtonResult.Ok)
                {
                    _messenger.Send(new DisplayCustomerProfileMessage());
                }
            }
            else
            {
                box = MessageBoxManager
                    .GetMessageBoxStandard("Error", $"{data}", ButtonEnum.Ok, Icon.Error,
                    null, WindowStartupLocation.CenterOwner);

                result = await box.ShowAsync();
                if (result == ButtonResult.Ok)
                {
                    _messenger.Send(new DisplayCustomerProfileMessage());
                }
            }
        }
    }
}
