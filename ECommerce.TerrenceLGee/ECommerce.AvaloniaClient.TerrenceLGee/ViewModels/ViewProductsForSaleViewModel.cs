using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Helpers;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ViewProductsForSaleViewModel : ObservableObject
{
    private readonly IProductService _productService;
    private readonly IMessenger _messenger;
    private readonly int _categoryId;

    [ObservableProperty]
    private static List<CartItemDto> _shoppingCart;

    public ObservableCollection<ProductData> Products { get; } = [];

    [ObservableProperty]
    private bool _isLoading;
    [ObservableProperty]
    private ProductData? _selectedProduct;

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
    private decimal? _minUnitPrice;
    [ObservableProperty]
    private decimal? _maxUnitPrice;
    [ObservableProperty]
    private int? _minStockQuantity;
    [ObservableProperty]
    private int? _maxStockQuantity;
    [ObservableProperty]
    private int? _minDiscountPercentage;
    [ObservableProperty]
    private int? _maxDiscountPercentage;
    [ObservableProperty]
    private string? _description;


    public ViewProductsForSaleViewModel(
        IProductService productService,
        int categoryId,
        List<CartItemDto> shoppingCart,
        IMessenger messenger)
    {
        _productService = productService;
        _categoryId = categoryId;
        _shoppingCart = shoppingCart;
        _messenger = messenger;
        LoadProductsCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadProductsAsync()
    {
        Page = 1;
        await FetchProductsAsync();
    }

    private async Task FetchProductsAsync()
    {
        IsLoading = true;

        var queryParams = new ProductQueryParams
        {
            Page = Page,
            PageSize = PageSize,
            CategoryId = _categoryId,
            MinUnitPrice = MinUnitPrice,
            MaxUnitPrice = MaxUnitPrice,
            MinStockQuantity = MinStockQuantity,
            MaxStockQuantity = MaxStockQuantity,
            MinDiscountPercentage = MinDiscountPercentage,
            MaxDiscountPercentage = MaxDiscountPercentage,
            Description = Description,
            InStock = true
        };

        var result = await _productService.GetProductsAsync(queryParams);

        if (result is not null)
        {
            Products.Clear();

            foreach (var product in result.Data)
            {
                Products.Add(product);
            }

            TotalPages = result.TotalPages;
            HasNextPage = Page < TotalPages && result.TotalItemsRetrieved >= PageSize;
            HasPreviousPage = Page > 1;
        }

        IsLoading = false;
    }

    [RelayCommand]
    private async Task NextPageAsync()
    {
        if (!HasNextPage) return;
        Page++;
        await FetchProductsAsync();
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        Page--;
        await FetchProductsAsync();
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToAllCategoriesForSale());
    }

    [RelayCommand]
    private void ClearFilters()
    {
        MinUnitPrice = null;
        MaxUnitPrice = null;
        MinStockQuantity = null;
        MaxStockQuantity = null;
        MaxDiscountPercentage = null;
        MinDiscountPercentage = null;
        Description = null;
    }

    partial void OnSelectedProductChanged(ProductData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new ProductSelectedForSaleMessage(value, _categoryId, ShoppingCart));
        }
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

    async partial void OnMinUnitPriceChanged(decimal? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMaxUnitPriceChanged(decimal? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMinStockQuantityChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMaxStockQuantityChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMinDiscountPercentageChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMaxDiscountPercentageChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnDescriptionChanged(string? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);
    
}
