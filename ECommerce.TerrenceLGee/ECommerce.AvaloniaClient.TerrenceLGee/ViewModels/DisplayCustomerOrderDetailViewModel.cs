using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.Customer;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayCustomerOrderDetailViewModel : ObservableObject
{
    public int SaleId { get; }
    [ObservableProperty]
    private SaleData? _sale;

    public ObservableCollection<SaleProductData> OrderItems { get; } = [];
    private readonly ISaleService _saleService;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private List<SaleProductData> _orderItemsForDisplay;

    [ObservableProperty]
    private SaleProductData? _selectedItem;

    public DisplayCustomerOrderDetailViewModel(ISaleService saleService, int saleId, IMessenger messenger)
    {
        _saleService = saleService;
        SaleId = saleId;
        _messenger = messenger;
        _orderItemsForDisplay = new List<SaleProductData>();
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
        Sale = await _saleService.GetSaleForCustomerAsync(SaleId);
        if (Sale is null) return;
        LoadOrderItems(Sale.SaleProducts);
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
        _messenger.Send(new DisplayCustomerProfileMessage());
    }

    [RelayCommand]
    private async Task CancelOrderAsync()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        var box = MessageBoxManager
            .GetMessageBoxStandard("Cancel", $"Cancel Order?", ButtonEnum.YesNo, Icon.Question,
            null, WindowStartupLocation.CenterOwner);

        var result = await box.ShowAsync();

        if (result == ButtonResult.Yes)
        {
            var (success, data) = await _saleService.CustomerCancelSaleAsync(SaleId);

            if (success)
            {
                SuccessMessage = data;
                Sale = await _saleService.GetSaleForCustomerAsync(SaleId);
            }
            else
            {
                ErrorMessage = data;
            }
        }
    }

    partial void OnSelectedItemChanged(SaleProductData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new SaleProductSelectedForCustomerDetailMessage(value.ProductId));
        }
    }
}
