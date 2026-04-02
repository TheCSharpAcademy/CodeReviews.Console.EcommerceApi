using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.OtherMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class CheckoutViewModel : ObservableObject
{
    private readonly ISaleService _saleService;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private static List<CartItemDto> _shoppingCart;
    [ObservableProperty]
    private static List<CartItemDto> _shoppingCartForDisplay;
    [ObservableProperty]
    private string? _successMessage;
    [ObservableProperty]
    private string? _errorMessage;
    [ObservableProperty]
    private CartItemDto? _selectedItem;

    [ObservableProperty]
    private int _page = 1;
    [ObservableProperty]
    private int _pageSize = 10;
    [ObservableProperty]
    private int _totalPages;
    [ObservableProperty]
    private bool _hasNextPage;
    [ObservableProperty]
    private bool _hasPreviousPage;

    [ObservableProperty]
    private decimal _subTotal;

    public CheckoutViewModel(ISaleService saleService, List<CartItemDto> shoppingCart, IMessenger messenger)
    {
        _saleService = saleService;
        _messenger = messenger;
        _shoppingCart = shoppingCart;
        _subTotal = CalculateSubTotal(_shoppingCart);
        _shoppingCartForDisplay = new List<CartItemDto>();
        LoadCartCommand.Execute(null);
    }

    [RelayCommand]
    private void LoadCart()
    {
        var pagedCart = ShoppingCart.Skip((Page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        ShoppingCartForDisplay = pagedCart;

        TotalPages = (int)Math.Ceiling(ShoppingCart.Count / (double)PageSize);
        HasNextPage = Page < TotalPages;
        HasPreviousPage = Page > 1;
    }

    [RelayCommand]
    private void NextPage()
    {
        if (!HasNextPage) return;
        Page++;
        LoadCart();
    }

    [RelayCommand]
    private void PreviousPage()
    {
        if (!HasPreviousPage) return;
        Page--;
        LoadCart();
    }

    [RelayCommand]
    private async Task CheckoutAsync()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        var order = new CreateOrderDto
        {
            ShoppingCart = ShoppingCart
        };

        var result = await _saleService.CreateOrderAsync(order);

        if (result is null)
        {
            ErrorMessage = "Unable to complete order at this time";
            return;
        }

        if (string.IsNullOrEmpty(result.ErrorMessage))
        {
            SuccessMessage = "Order completed successfully";
            ShoppingCart.Clear();
            _messenger.Send(new OrderCompletedMessage(result));
        }
        else
        {
            ErrorMessage = result.ErrorMessage;
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToPreviousPageMessage());
    }

    [RelayCommand]
    private void CancelOrder()
    {
        ShoppingCart.Clear();
        _messenger.Send(new NavigateBackToAllCategoriesOrderCanceledMessage());
    }

    [RelayCommand]
    private async Task RemoveItemAsync()
    {
        if (SelectedItem is not null)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Delete", "Delete this item?", ButtonEnum.YesNo, Icon.Warning, null, WindowStartupLocation.CenterOwner);

            var result = await box.ShowAsync();

            if (result == ButtonResult.Yes)
            {
                ShoppingCart.Remove(SelectedItem);
                LoadCart();
            }
        }
    }

    [RelayCommand]
    private async Task ClearCartAsync()
    {
        var box = MessageBoxManager
            .GetMessageBoxStandard("Empty", "Empty Cart?", ButtonEnum.YesNo, Icon.Warning, null,
            WindowStartupLocation.CenterOwner);

        var result = await box.ShowAsync();

        if (result == ButtonResult.Yes)
        {
            ShoppingCart.Clear();
            LoadCart();
        }
    }

    private decimal CalculateSubTotal(List<CartItemDto> items)
    {
        var subtotal = 0.0m;

        foreach (var item in items)
        {
            subtotal += ((decimal)item.ProductPrice! * item.Quantity);
        }

        return subtotal;
    }
}
