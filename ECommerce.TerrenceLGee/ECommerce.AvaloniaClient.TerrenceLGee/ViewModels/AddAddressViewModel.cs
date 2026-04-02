using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.AddressMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;
using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class AddAddressViewModel : ObservableValidator
{
    private readonly IAddressService _addressService;
    private readonly IMessenger _messenger;

    public AddAddressViewModel(IAddressService addressService, IMessenger messenger)
    {
        _addressService = addressService;
        _messenger = messenger;
    }

    [ObservableProperty]
    [Required(ErrorMessage = "Address Line 1 is required.")]
    [MaxLength(100, ErrorMessage = "Address Line 1 cannot exceed 100 characters.")]
    [NotifyPropertyChangedFor(nameof(AddressLine1Errors))]
    private string _addressLine1;

    public string? AddressLine1Errors => GetErrors(nameof(AddressLine1))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [MaxLength(100, ErrorMessage = "Address Line 2 cannot exceed 100 characters.")]
    [NotifyPropertyChangedFor(nameof(AddressLine2Errors))]
    private string? _addressLine2;

    public string? AddressLine2Errors => GetErrors(nameof(AddressLine2))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "City is required.")]
    [MaxLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
    [NotifyPropertyChangedFor(nameof(CityErrors))]
    public string _city;

    public string? CityErrors => GetErrors(nameof(City))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "State is required.")]
    [MaxLength(50, ErrorMessage = "State cannot exceed 50 characters.")]
    [NotifyPropertyChangedFor(nameof(StateErrors))]
    private string _state;

    public string? StateErrors => GetErrors(nameof(State))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Postal code is required.")]
    [RegularExpression(@"\d{4,6}$", ErrorMessage = "Invalid postal code.")]
    [NotifyPropertyChangedFor(nameof(PostalCodeErrors))]
    private string _postalCode;

    public string? PostalCodeErrors => GetErrors(nameof(PostalCode))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Country is required.")]
    [MaxLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
    [NotifyPropertyChangedFor(nameof(CountryErrors))]
    private string _country;

    public string? CountryErrors => GetErrors(nameof(Country))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    private bool _isBillingAddress;

    [ObservableProperty]
    private bool _isShippingAddress;

    [ObservableProperty]
    private string? _successMessage;

    [ObservableProperty]
    private string? _errorMessage;

    [RelayCommand]
    private async Task AddAddressAsync()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        ClearErrors();

        ValidateProperty(AddressLine1, nameof(AddressLine1));
        ValidateProperty(AddressLine2, nameof(AddressLine2));
        ValidateProperty(City, nameof(City));
        ValidateProperty(State, nameof(State));
        ValidateProperty(PostalCode, nameof(PostalCode));
        ValidateProperty(Country, nameof(Country));

        if (HasErrors)
        {
            return;
        }

        var address = new CreateAddressDto
        {
            AddressLine1 = AddressLine1,
            AddressLine2 = AddressLine2,
            City = City,
            State = State,
            PostalCode = PostalCode,
            Country = Country,
            IsBillingAddress = IsBillingAddress,
            IsShippingAddress = IsShippingAddress
        };

        var data = await _addressService.AddAddressAsync(address);

        if (data is null)
        {
            ErrorMessage = "Unable to add address at this time";
            return;
        }

        if (string.IsNullOrEmpty(data.ErrorMessage))
        {
            ClearAddressAdd();
            SuccessMessage = "Address added successfully";
            _messenger.Send(new AddressAddedMessage(data));
        }
        else
        {
            ErrorMessage = data.ErrorMessage;
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new DisplayCustomerProfileMessage());
    }

    private void ClearAddressAdd()
    {
        AddressLine1 = string.Empty;
        AddressLine2 = string.Empty;
        City = string.Empty;
        State = string.Empty;
        PostalCode = string.Empty;
        Country = string.Empty;
        IsBillingAddress = false;
        IsShippingAddress = false;
    }
}
