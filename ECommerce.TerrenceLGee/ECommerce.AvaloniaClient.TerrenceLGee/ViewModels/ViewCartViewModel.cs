using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ViewCartViewModel : ObservableObject
{
    [ObservableProperty]
    private static List<CartItemDto> _cart;

    [ObservableProperty]
    private static List<CartItemDto> _cartForDisplay;

    private readonly IMessenger _messenger;

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
    private string? _errorMessage;

    public ViewCartViewModel(List<CartItemDto> cart, IMessenger messenger)
    {
        _cart = cart;
        _messenger = messenger;
        _cartForDisplay = new List<CartItemDto>();
        LoadCartCommand.Execute(null);
    }

    [RelayCommand]
    private void LoadCart()
    {
        var pagedCart = Cart.Skip((Page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        CartForDisplay = pagedCart;

        TotalPages = (int)Math.Ceiling(Cart.Count / (double)PageSize);
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
    private void GoBack()
    {
        _messenger.Send(new NavigateBackFromViewCart());
    }

    [RelayCommand]
    private void ClearCart()
    {
        Cart.Clear();
        LoadCart();
    }

    [RelayCommand]
    private void Checkout()
    {
        _messenger.Send(new CheckoutMessage(Cart));
    }

    [RelayCommand]
    private void UpdateQuantity()
    {
        ErrorMessage = null;

        if (SelectedItem is not null)
        {
            var itemToUpdate = Cart.FirstOrDefault(ci => ci.ProductId == SelectedItem.ProductId);
            if (itemToUpdate is not null)
            {
                itemToUpdate.Quantity = SelectedItem.Quantity;
                itemToUpdate.TotalAmount = SelectedItem.Quantity * SelectedItem.ProductPrice;
                Cart.Remove(SelectedItem);
                Cart.Add(itemToUpdate);
                LoadCart();
            }
            else
            {
                ErrorMessage = $"Unable to update quantity for {SelectedItem.ProductName}";
            }
        }
    }

    [RelayCommand]
    private async Task RemoveItemAsync()
    {
        ErrorMessage = null;

        if (SelectedItem is not null)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Delete", "Delete this item?", ButtonEnum.YesNo, Icon.Warning, null,
                WindowStartupLocation.CenterOwner);

            var result = await box.ShowAsync();

            if (result == ButtonResult.Yes)
            {
                Cart.Remove(SelectedItem);
                LoadCart();
            }
            else
            {
                ErrorMessage = $"Unable to delete {SelectedItem.ProductName}";
            }
        }
    }
}
