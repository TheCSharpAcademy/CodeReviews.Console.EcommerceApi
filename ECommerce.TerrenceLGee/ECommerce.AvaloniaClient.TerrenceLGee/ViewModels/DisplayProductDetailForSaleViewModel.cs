using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayProductDetailForSaleViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private readonly IMessenger _messenger;
    private readonly int _categoryId;

    [ObservableProperty]
    private ProductData _product;
    [ObservableProperty]
    private static List<CartItemDto> _shoppingCart;
    [ObservableProperty]
    private decimal _quantity = 1;
    [ObservableProperty]
    public decimal _maxQuantity = 5000;



    public DisplayProductDetailForSaleViewModel(
        IProductService productService,
        List<CartItemDto> shoppingCart,
        ProductData product,
        int categoryId,
        IMessenger messenger)
    {
        _productService = productService;
        _product = product;
        _shoppingCart = shoppingCart;
        _categoryId = categoryId;
        _messenger = messenger;
    }

    [RelayCommand]
    private async Task AddToCartAsync()
    {
        var item = ShoppingCart
            .FirstOrDefault(ci => ci.ProductId == Product.Id);

        if (item is null)
        {
            ShoppingCart.Add(new CartItemDto
            {
                ProductId =
                Product.Id,
                Quantity = (int)Quantity,
                ProductName = Product.Name,
                TotalAmount = ((int)Quantity * Product.UnitPrice),
                ProductPrice = Product.UnitPrice
            });
        }
        else
        {
            ShoppingCart.Remove(item);
            var quantity = item.Quantity + (int)Quantity;
            var id = item.ProductId;
            var name = item.ProductName;
            var totalPrice = quantity * Product.UnitPrice;
            ShoppingCart.Add(new CartItemDto
            {
                ProductId = id,
                Quantity = quantity,
                ProductName = name,
                TotalAmount = totalPrice,
                ProductPrice = item.ProductPrice
            });
        }

        var box = MessageBoxManager
            .GetMessageBoxStandard("Added", "Item Added To Cart", ButtonEnum.Ok, Icon.Success,
            null, WindowStartupLocation.CenterOwner);

        await box.ShowAsync();
    }

    [RelayCommand]
    private async Task RemoveFromCartAsync()
    {
        var itemToRemove = ShoppingCart.Where(ci => ci.ProductId == Product.Id && ci.Quantity == (int)Quantity)
            .FirstOrDefault();

        if (itemToRemove is not null)
        {
            ShoppingCart.Remove(itemToRemove);
            var box = MessageBoxManager
                .GetMessageBoxStandard("Removed", "Item Removed From Cart", ButtonEnum.Ok, Icon.Success,
                null, WindowStartupLocation.CenterOwner);

            await box.ShowAsync();
        }
        else
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Error", "Unable To Remove Item From Cart", ButtonEnum.Ok, Icon.Success,
                null, WindowStartupLocation.CenterOwner);

            await box.ShowAsync();
        }
    }

    [RelayCommand]
    private void GoBackToProducts()
    {
        _messenger.Send(new NavigateBackToProductsFromSelectedProductMessage(_categoryId, ShoppingCart));
    }

    [RelayCommand]
    private void ViewCart()
    {
        _messenger.Send(new ViewCartMessage(ShoppingCart));
    }

    [RelayCommand]
    private void Checkout()
    {
        _messenger.Send(new CheckoutMessage(ShoppingCart));
    }
}
