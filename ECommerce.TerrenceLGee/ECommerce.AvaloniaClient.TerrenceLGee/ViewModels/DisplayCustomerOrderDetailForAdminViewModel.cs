using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.OtherMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;
using ECommerce.Shared.TerrenceLGee.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayCustomerOrderDetailForAdminViewModel : ObservableObject
{
    public int SaleId { get; }
    private readonly CustomerData _customer;
    [ObservableProperty]
    private SaleData? _sale;

    public ObservableCollection<SaleProductData> OrderItems { get; } = [];
    private readonly ISaleService _saleService;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private List<SaleProductData> _orderItemsForDisplay;

    [ObservableProperty]
    private SaleProductData? _selectedItem;
   

    [ObservableProperty]
    private List<SaleStatus> _statuses;
    [ObservableProperty]
    private SaleStatus _selectedStatus;

    public DisplayCustomerOrderDetailForAdminViewModel(ISaleService saleService, int saleId, CustomerData customer, IMessenger messenger)
    {
        _saleService = saleService;
        SaleId = saleId;
        _customer = customer;
        _messenger = messenger;
        _orderItemsForDisplay = new List<SaleProductData>();
        _statuses = new List<SaleStatus>()
        {
            SaleStatus.Pending,
            SaleStatus.Processing,
            SaleStatus.Shipped,
            SaleStatus.Delivered,
            SaleStatus.Canceled
        };
    }

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
    private string? _successMessage;
    [ObservableProperty]
    private string? _errorMessage;

    public async Task GetSaleAsync()
    {
        Sale = await _saleService.GetSaleForAdminAsync(SaleId);
        if (Sale is null) return;
        LoadOrderItems(Sale.SaleProducts);
        FetchOrderItems();
    }

    private void LoadOrderItems(List<SaleProductData> items)
    {
        foreach (var item in items)
        {
            OrderItems.Add(item);
        }

        FetchOrderItems();
    }

    [RelayCommand]
    private void NextPage()
    {
        if (!HasNextPage) return;
        Page++;
        FetchOrderItems();
    }

    [RelayCommand]
    private void PreviousPage()
    {
        if (!HasPreviousPage) return;
        Page--;
        FetchOrderItems();
    }

    [RelayCommand]
    private void FetchOrderItems()
    {
        var pagedItems = OrderItems.Skip((Page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        OrderItemsForDisplay = pagedItems;

        TotalPages = (int)Math.Ceiling(OrderItems.Count / (double)PageSize);
        HasNextPage = Page < TotalPages;
        HasPreviousPage = Page > 1;
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToCustomerDetailsMessage(_customer));
    }

    [RelayCommand]
    private async Task UpdateOrderAsync()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        var updateSaleStatus = new UpdateSaleStatusDto { Status = SelectedStatus };

        var (success, data) = await _saleService.AdminUpdateSaleStatusAsync(SaleId, updateSaleStatus);

        if (success)
        {
            SuccessMessage = data;
            Sale = await _saleService.GetSaleForAdminAsync(SaleId);
            _messenger.Send(new AdminUpdatedCustomerOrderMessage(_customer));
        }
        else
        {
            ErrorMessage = data;
        }
    }

    partial void OnSelectedItemChanged(SaleProductData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new ViewCustomerSaleProductDetailForAdminMessage(value.ProductId));
        }
    }
}
