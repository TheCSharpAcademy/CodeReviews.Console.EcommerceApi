using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Helpers;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayAdminCategoryDetailViewModel : ObservableObject
{
    public int CategoryId { get; }
    [ObservableProperty]
    public CategoryAdminData? _category;
    public ObservableCollection<ProductAdminData> Products { get; } = [];
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private ProductAdminData? _selectedProduct;


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
    private string? _description;

    public DisplayAdminCategoryDetailViewModel(
        ICategoryService categoryService,
        IProductService productService,
        int categoryId,
        IMessenger messenger)
    {
        _categoryService = categoryService;
        _productService = productService;
        CategoryId = categoryId;
        _messenger = messenger;
        LoadProductsCommand.Execute(null);
    }

    public async Task GetCategoryAsync()
    {
        Category = await _categoryService.GetCategoryForAdminAsync(CategoryId);
        if (Category is null) GoBack();
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
            CategoryId = CategoryId,
            MinUnitPrice = MinUnitPrice,
            MaxUnitPrice = MaxUnitPrice,
            MinStockQuantity = MinStockQuantity,
            MaxStockQuantity = MaxStockQuantity,
            MinDiscountPercentage = MinDiscountPercentage,
            MaxDiscountPercentage = MaxDiscountPercentage,
            Description = Description
        };

        var result = await _productService.GetProductsForAdminAsync(queryParams);

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
    private async Task ClearFiltersAsync()
    {
        MinUnitPrice = null;
        MaxUnitPrice = null;
        MinStockQuantity = null;
        MaxStockQuantity = null;
        MinDiscountPercentage = null;
        MaxDiscountPercentage = null;
        Description = null;
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToAllAdminCategoriesMessage());
    }

    partial void OnSelectedProductChanged(ProductAdminData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new CategoryProductSelectedForAdminMessage(value.Id, CategoryId));
        }
    }

    async partial void OnMinUnitPriceChanged(decimal? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMaxUnitPriceChanged(decimal? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMinStockQuantityChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMaxStockQuantityChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMinDiscountPercentageChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnMaxDiscountPercentageChanged(int? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);

    async partial void OnDescriptionChanged(string? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadProductsAsync);
}
