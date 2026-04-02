using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Helpers;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.Parameters.CustomerParameters;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ViewCustomersForAdminViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;
    private readonly IMessenger _messenger;
    public ObservableCollection<CustomerData> Customers { get; } = [];

    [ObservableProperty]
    private bool _isLoading;
    [ObservableProperty]
    private CustomerData? _selectedCustomer;

    [ObservableProperty]
    private int _page = 1;
    [ObservableProperty]
    private int _pageSize = 10;
    [ObservableProperty]
    private int _totalPages;
    [ObservableProperty]
    private bool _hasPreviousPage;
    [ObservableProperty]
    private bool _hasNextPage;

    [ObservableProperty]
    private int? _minSaleCount;
    [ObservableProperty]
    private int? _maxSaleCount;
    [ObservableProperty]
    private decimal? _minTotalSpent;
    [ObservableProperty]
    private decimal? _maxTotalSpent;

    async partial void OnMinSaleCountChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadCustomersAsync);
    async partial void OnMaxSaleCountChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadCustomersAsync);
    async partial void OnMinTotalSpentChanged(decimal? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadCustomersAsync);
    async partial void OnMaxTotalSpentChanged(decimal? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadCustomersAsync);

    public ViewCustomersForAdminViewModel(ICustomerService customerService, IMessenger messenger)
    {
        _customerService = customerService;
        _messenger = messenger;
        LoadCustomersCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadCustomersAsync()
    {
        Page = 1;
        await FetchCustomersAsync();
    }

    [RelayCommand]
    private async Task NextPageAsync()
    {
        if (!HasNextPage) return;
        Page++;
        await FetchCustomersAsync();
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        Page--;
        await FetchCustomersAsync();
    }

    private async Task FetchCustomersAsync()
    {
        IsLoading = true;

        var queryParams = new CustomerQueryParams
        {
            Page = Page,
            PageSize = PageSize,
            MinSaleCount = MinSaleCount,
            MaxSaleCount = MaxSaleCount,
            MinTotalSpent = MinTotalSpent,
            MaxTotalSpent = MaxTotalSpent
        };

        var result = await _customerService.GetCustomersForAdminAsync(queryParams);

        if (result is not null)
        {
            Customers.Clear();

            foreach (var customer in result.Data)
            {
                Customers.Add(customer);
            }

            TotalPages = result.TotalPages;
            HasNextPage = Page < TotalPages && result.TotalItemsRetrieved >= PageSize;
            HasPreviousPage = Page > 1;
        }

        IsLoading = false;
    }

    [RelayCommand]
    private async Task ClearFiltersAsync()
    {
        MinSaleCount = null;
        MaxSaleCount = null;
        MinTotalSpent = null;
        MaxTotalSpent = null;
    }

    [RelayCommand]
    private async Task GoBack()
    {
        _messenger.Send(new NavigateBackToCustomerPageMessage());
    }

    partial void OnSelectedCustomerChanged(CustomerData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new DisplayCustomerDetailsForAdminMessage(value));
        }
    }
}
