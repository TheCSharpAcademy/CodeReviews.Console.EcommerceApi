using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayOrderDetailsViewModel : ViewModelBase
{
    [ObservableProperty]
    private SaleData _sale;
    public ObservableCollection<SaleProductData> OrderItems { get; } = [];

    [ObservableProperty]
    private List<SaleProductData> _orderItemsForDisplay;

    private readonly IMessenger _messenger;

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

    public DisplayOrderDetailsViewModel(SaleData sale, IMessenger messenger)
    {
        _sale = sale;
        _messenger = messenger;
        _orderItemsForDisplay = new List<SaleProductData>();
        LoadOrderItems(sale.SaleProducts);
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
    private void ShopAgain()
    {
        _messenger.Send(new ShopAgainMessage());
    }
}
