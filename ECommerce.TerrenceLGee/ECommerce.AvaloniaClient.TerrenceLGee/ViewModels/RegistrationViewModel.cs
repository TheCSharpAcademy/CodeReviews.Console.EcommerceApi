using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Auth;
using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;
using ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class RegistrationViewModel : ObservableValidator
{
    private readonly IAuthService _authService;
    public event Action? BackRequested;
    public event Action? RegistrationSuccessful;

    public RegistrationViewModel(IAuthService authService)
    {
        _authService = authService;
    }

    [ObservableProperty]
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(25, ErrorMessage = "First name cannot be greater than 25 characters.")]
    [NotifyPropertyChangedFor(nameof(FirstNameErrors))]
    private string _firstName = string.Empty;

    public string? FirstNameErrors => GetErrors(nameof(FirstName))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(25, ErrorMessage = "Last name cannot be greater than 25 characters.")]
    [NotifyPropertyChangedFor(nameof(LastNameErrors))]
    private string _lastName = string.Empty;

    public string? LastNameErrors => GetErrors(nameof(LastName))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [NotifyPropertyChangedFor(nameof(EmailErrors))]
    private string _email = string.Empty;

    public string? EmailErrors => GetErrors(nameof(Email))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Date of birth is required.")]
    [NotifyPropertyChangedFor(nameof(SelectedBirthDateErrors))]
    private DateTimeOffset? _selectedBirthDate;

    public string? SelectedBirthDateErrors => GetErrors(nameof(SelectedBirthDate))
        .FirstOrDefault()?.ErrorMessage;

    public DateOnly? DateOfBirth => SelectedBirthDate.HasValue
        ? DateOnly.FromDateTime(SelectedBirthDate.Value.Date)
        : null;

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
    private string _city;

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
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [NotifyPropertyChangedFor(nameof(PasswordErrors))]
    private string _password = string.Empty;

    public string? PasswordErrors => GetErrors(nameof(Password))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "You must confirm your password.")]
    [CustomValidation(typeof(RegistrationViewModel), nameof(ValidatePasswordConfirmation))]
    [NotifyPropertyChangedFor(nameof(ConfirmPasswordErrors))]
    private string _confirmPassword = string.Empty;

    public string? ConfirmPasswordErrors => GetErrors(nameof(ConfirmPassword))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    private string? _successMessage;
    [ObservableProperty]
    private string? _errorMessage;

    [RelayCommand]
    private async Task RegisterAsync()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        ClearErrors();

        ValidateProperty(FirstName, nameof(FirstName));
        ValidateProperty(LastName, nameof(LastName));
        ValidateProperty(Email, nameof(Email));
        ValidateProperty(AddressLine1, nameof(AddressLine1));
        ValidateProperty(AddressLine2, nameof(AddressLine2));
        ValidateProperty(City, nameof(City));
        ValidateProperty(State, nameof(State));
        ValidateProperty(PostalCode, nameof(PostalCode));
        ValidateProperty(Country, nameof(Country));
        ValidateProperty(Password, nameof(Password));
        ValidateProperty(ConfirmPassword, nameof(ConfirmPassword));

        if (HasErrors)
        {
            return;
        }

        var customer = new UserRegistrationDto
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            DateOfBirth = DateOfBirth!.Value,
            BillingAddress = IsBillingAddress
            ? new CreateAddressDto
            {
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,
                City = City,
                State = State,
                PostalCode = PostalCode,
                Country = Country,
                IsBillingAddress = IsBillingAddress,
                IsShippingAddress = IsShippingAddress
            }
            : null,
            ShippingAddress = IsShippingAddress
            ? new CreateAddressDto
            {
                AddressLine1 = AddressLine1,
                AddressLine2 = AddressLine2,
                City = City,
                State = State,
                PostalCode = PostalCode,
                Country = Country,
                IsBillingAddress = IsBillingAddress,
                IsShippingAddress = IsShippingAddress
            }
            : null,
            Password = Password
        };

        var (success, message) = await _authService.RegisterUserAsync(customer);

        if (success)
        {
            SuccessMessage = message;
            ClearRegistration();
            RegistrationSuccessful?.Invoke();
        }
        else
        {
            ErrorMessage = message;
            ClearRegistration();
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        BackRequested?.Invoke();
    }

    private void ClearRegistration()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        SelectedBirthDate = null;
        AddressLine1 = string.Empty;
        AddressLine2 = string.Empty;
        City = string.Empty;
        State = string.Empty;
        PostalCode = string.Empty;
        Country = string.Empty;
        IsBillingAddress = false;
        IsShippingAddress = false;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
    }

    public static ValidationResult? ValidatePasswordConfirmation(string confirmPassword, ValidationContext context)
    {
        var viewModel = (RegistrationViewModel)context.ObjectInstance;

        var password = viewModel.Password;

        if (!password.Equals(confirmPassword))
        {
            return new ValidationResult("Passwords do not match.");
        }

        return ValidationResult.Success;
    }
}
