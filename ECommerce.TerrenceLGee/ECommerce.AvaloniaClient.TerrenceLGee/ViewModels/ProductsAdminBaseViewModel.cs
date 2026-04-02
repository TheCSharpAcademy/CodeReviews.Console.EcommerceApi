using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public abstract partial class ProductsAdminBaseViewModel : ObservableValidator
{
    public ObservableCollection<ProductAdminData> Products { get; } = [];

    [ObservableProperty]
    private ProductAdminData? _selectedProduct;

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
    private bool _isLoading;

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
    private string? _categoryName;
    [ObservableProperty]
    private string? _description;
    [ObservableProperty]
    private bool? _inStock;
    [ObservableProperty]
    private bool? _isDeleted;

    private async Task FetchProductsAsync()
    {
        IsLoading = true;

        var result = await GetProductsAsync();

        if (result is not null)
        {
            Products.Clear();

            foreach (var product in result.Data)
            {
                Products.Add(product);
            }

            TotalPages = result.TotalPages;
            HasNextPage = Page < TotalPages;
            HasPreviousPage = Page > 1;
        }

        IsLoading = false;
    }

    [RelayCommand]
    protected async Task LoadProductsAsync()
    {
        Page = 1;
        await FetchProductsAsync();
    }

    [RelayCommand]
    protected async Task NextPageAsync()
    {
        if (!HasNextPage) return;
        Page++;
        await FetchProductsAsync();
    }

    [RelayCommand]
    protected async Task PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        Page--;
        await FetchProductsAsync();
    }

    [RelayCommand]
    protected async Task ClearFiltersAsync()
    {
        MinUnitPrice = null;
        MaxUnitPrice = null;
        MinStockQuantity = null;
        MaxStockQuantity = null;
        MinDiscountPercentage = null;
        MaxDiscountPercentage = null;
        CategoryName = null;
        Description = null;
        IsDeleted = false;
    }

    protected abstract Task<ProductsAdminRoot?> GetProductsAsync();
    protected abstract void OnProductSelected(ProductAdminData product);

    partial void OnSelectedProductChanged(ProductAdminData? value)
    {
        if (value is not null)
        {
            OnProductSelected(value);
        }
    }
    async partial void OnMinUnitPriceChanged(decimal? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMaxUnitPriceChanged(decimal? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMinStockQuantityChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMaxStockQuantityChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMinDiscountPercentageChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMaxDiscountPercentageChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnCategoryNameChanged(string? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnDescriptionChanged(string? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnInStockChanged(bool? value)
    {
        Page = 1;
        await FetchProductsAsync();
    }

    async partial void OnIsDeletedChanged(bool? value)
    {
        Page = 1;
        await FetchProductsAsync();
    }
}
