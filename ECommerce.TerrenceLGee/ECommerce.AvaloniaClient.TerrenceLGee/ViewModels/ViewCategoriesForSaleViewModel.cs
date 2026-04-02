using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Helpers;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.SaleMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ViewCategoriesForSaleViewModel : ObservableObject
{
    private readonly ICategoryService _categoryService;
    private readonly IShoppingCartService _shoppingCartService;
    private readonly IMessenger _messenger;
    public ObservableCollection<CategorySummaryData> Categories { get; } = [];

    [ObservableProperty]
    private static List<CartItemDto> _shoppingCart;

    [ObservableProperty]
    private bool _isLoading;
    [ObservableProperty]
    private CategorySummaryData? _selectedCategory;

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
    private string? _searchByDescription;

    public ViewCategoriesForSaleViewModel(
        ICategoryService categoryService, 
        IShoppingCartService shoppingCartService, 
        IMessenger messenger)
    {
        _categoryService = categoryService;
        _shoppingCartService = shoppingCartService;
        _messenger = messenger;
        _shoppingCart = ShoppingCartService.ShoppingCart;
        LoadCategoriesCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadCategoriesAsync()
    {
        Page = 1;

        await FetchCategoriesAsync();
    }

    private async Task FetchCategoriesAsync()
    {
        IsLoading = true;

        var queryParams = new CategoryQueryParams
        {
            Page = Page,
            PageSize = PageSize,
            Description = SearchByDescription
        };

        var result = await _categoryService.GetCategoriesAsync(queryParams);

        if (result is not null)
        {
            Categories.Clear();

            foreach (var category in result.Data)
            {
                Categories.Add(category);
            }

            TotalPages = result.TotalPages;
            HasNextPage = Page < TotalPages;
            HasPreviousPage = Page > 1;
        }

        IsLoading = false;
    }

    [RelayCommand]
    private async Task NextPageAsync()
    {
        if (!HasNextPage) return;
        Page++;
        await FetchCategoriesAsync();
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        Page--;
        await FetchCategoriesAsync();
    }

    async partial void OnSearchByDescriptionChanged(string? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadCategoriesAsync);

    partial void OnSelectedCategoryChanged(CategorySummaryData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new CategorySelectedForSaleMessage(value.Id, ShoppingCart));
        }
    }

    [RelayCommand]
    private void ClearFilters()
    {
        SearchByDescription = null;
    }

    [RelayCommand]
    private void Checkout()
    {
        _messenger.Send(new CheckoutMessage(ShoppingCart));
    }

    [RelayCommand]
    private void ViewCart()
    {
        _messenger.Send(new ViewCartMessage(ShoppingCart));
    }
}
