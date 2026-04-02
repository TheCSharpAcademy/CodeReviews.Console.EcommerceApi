using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using ECommerce.AvaloniaClient.TerrenceLGee.Helpers;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.AddressMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Address;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using ECommerce.Shared.TerrenceLGee.Enums;
using ECommerce.Shared.TerrenceLGee.Parameters.AddressParameters;
using ECommerce.Shared.TerrenceLGee.Parameters.SaleParameters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayCustomerProfileViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;
    private readonly IAddressService _addressService;
    private readonly ISaleService _saleService;
    private readonly IMessenger _messenger;

    public ObservableCollection<AddressData> Addresses { get; } = [];
    public ObservableCollection<SaleSummaryData> Orders { get; } = [];

    [ObservableProperty]
    private List<SaleStatus> _saleStatuses;

    [ObservableProperty]
    private SaleSummaryData? _selectedOrder;

    [ObservableProperty]
    private AddressData? _selectedAddress;

    [ObservableProperty]
    private CustomerData? _profile;

    [ObservableProperty]
    private int _addressPage = 1;
    [ObservableProperty]
    private int _addressPageSize = 10;
    [ObservableProperty]
    private int _addressTotalPages;
    [ObservableProperty]
    private bool _addressHasNextPage;
    [ObservableProperty]
    private bool _addressHasPreviousPage;
    [ObservableProperty]
    private bool _addressIsLoading;

    [ObservableProperty]
    private int _orderPage = 1;
    [ObservableProperty]
    private int _orderPageSize = 10;
    [ObservableProperty]
    private int _orderTotalPages;
    [ObservableProperty]
    private bool _orderHasNextPage;
    [ObservableProperty]
    private bool _orderHasPreviousPage;
    [ObservableProperty]
    private bool _orderIsLoading;
    [ObservableProperty]
    private decimal? _minTotalAmount;
    [ObservableProperty]
    private decimal? _maxTotalAmount;
    [ObservableProperty]
    private string? _status;
    [ObservableProperty]
    private SaleStatus? _selectedStatus;

    public DisplayCustomerProfileViewModel(
        ICustomerService customerService,
        IAddressService addressService,
        ISaleService saleService,
        IMessenger messenger)
    {
        _customerService = customerService;
        _addressService = addressService;
        _saleService = saleService;
        _messenger = messenger;
        SaleStatuses = new List<SaleStatus>
        {
            SaleStatus.Pending,
            SaleStatus.Processing,
            SaleStatus.Shipped,
            SaleStatus.Delivered,
            SaleStatus.Canceled
        };
    }


    public async Task GetProfileAsync()
    {
        Profile = await _customerService.GetCustomerProfileAsync();

        if (Profile is not null)
        {
            await LoadAddressesAsync();
            await LoadOrdersAsync();
        }
    }


    [RelayCommand]
    private async Task NextAddressPageAsync()
    {
        if (!AddressHasNextPage) return;
        AddressPage++;
        await LoadAddressesAsync();
    }

    [RelayCommand]
    private async Task PreviousAddressPage()
    {
        if (!AddressHasPreviousPage) return;
        AddressPage--;
        await LoadAddressesAsync();
    }

    [RelayCommand]
    private async Task FetchOrdersAsync()
    {
        OrderIsLoading = true;

        var queryParams = new SaleQueryParams
        {
            Page = OrderPage,
            PageSize = OrderPageSize,
            MinTotalAmount = MinTotalAmount,
            MaxTotalAmount = MaxTotalAmount,
            Status = (SelectedStatus.HasValue)
            ? SelectedStatus.Value.ToString()
            : null
        };

        var result = await _saleService.GetSalesForCustomerAsync(queryParams);

        if (result is not null)
        {
            Orders.Clear();

            foreach (var order in result.Data)
            {
                Orders.Add(order);
            }

            OrderTotalPages = result.TotalPages;
            OrderHasNextPage = OrderPage < OrderTotalPages;
            OrderHasPreviousPage = OrderPage > 1;
        }

        OrderIsLoading = false;
    }

    [RelayCommand]
    private async Task NextOrderPageAsync()
    {
        if (!OrderHasNextPage) return;
        OrderPage++;
        await FetchOrdersAsync();
    }

    [RelayCommand]
    private async Task PreviousOrderPage()
    {
        if (!OrderHasPreviousPage) return;
        OrderPage--;
        await FetchOrdersAsync();
    }

    [RelayCommand]
    private async Task LoadAddressesAsync()
    {
        AddressIsLoading = true;

        var queryParams = new AddressQueryParams
        {
            Page = AddressPage,
            PageSize = AddressPageSize
        };

        var result = await _addressService.GetAddressesForCustomerAsync(queryParams);

        if (result is not null)
        {
            Addresses.Clear();

            foreach (var address in result.Data)
            {
                Addresses.Add(address);
            }

            AddressTotalPages = result.TotalPages;
            AddressHasNextPage = AddressPage < AddressTotalPages;
            AddressHasPreviousPage = AddressPage > 1;
        }

        AddressIsLoading = false;
    }


    [RelayCommand]
    private async Task LoadOrdersAsync()
    {
        OrderPage = 1;
        await FetchOrdersAsync();
    }

    [RelayCommand]
    private void ClearFilters()
    {
        MinTotalAmount = null;
        MaxTotalAmount = null;
        SelectedStatus = null;
    }

    [RelayCommand]
    private void AddAddress()
    {
        _messenger.Send(new AddAddressMessage());
    }

    async partial void OnMinTotalAmountChanged(decimal? value) => await FilterHelper.OnFilterChangedAsync(OrderPage, LoadOrdersAsync);

    async partial void OnMaxTotalAmountChanged(decimal? value) => await FilterHelper.OnFilterChangedAsync(OrderPage, LoadOrdersAsync);

    async partial void OnSelectedStatusChanged(SaleStatus? value)
    {
        OrderPage = 1;
        await FetchOrdersAsync();
    }

    partial void OnSelectedOrderChanged(SaleSummaryData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new SaleSelectedForCustomerDetailMessage(value.Id));
        }
    }

    partial void OnSelectedAddressChanged(AddressData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new AddressSelectedForDetailMessage(value.Id));
        }
    }
}
