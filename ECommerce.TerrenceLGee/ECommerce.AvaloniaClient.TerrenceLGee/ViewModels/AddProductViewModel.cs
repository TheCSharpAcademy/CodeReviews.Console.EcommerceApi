using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Helpers;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.OtherMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.ProductMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class AddProductViewModel : ObservableValidator
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private readonly IMessenger _messenger;
    public ObservableCollection<CategoryAdminSummaryData> Categories { get; } = [];

    public AddProductViewModel(ICategoryService categoryService, IProductService productService, IMessenger messenger)
    {
        _categoryService = categoryService;
        _productService = productService;
        _messenger = messenger;
        LoadCategoriesCommand.Execute(null);
    }

    [ObservableProperty]
    private int _categoryId;

    [ObservableProperty]
    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
    [NotifyPropertyChangedFor(nameof(NameErrors))]
    private string _name;

    public string? NameErrors => GetErrors(nameof(Name))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [MaxLength(1000, ErrorMessage = "Product description cannot exceed 1000 characters.")]
    [NotifyPropertyChangedFor(nameof(DescriptionErrors))]
    private string? _description;

    public string? DescriptionErrors => GetErrors(nameof(Description))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Product stock quantity is required.")]
    [Range(0, 5000, ErrorMessage = "Product stock quantity must be between 0 and the maximum capacity of our warehouse which is 5000.")]
    [NotifyPropertyChangedFor(nameof(StockQuantityErrors))]
    private int _stockQuantity;

    public string? StockQuantityErrors => GetErrors(nameof(StockQuantity))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Product unit price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Product unit price must be greater than $0.00.")]
    [NotifyPropertyChangedFor(nameof(UnitPriceErrors))]
    private decimal _unitPrice;

    public string? UnitPriceErrors => GetErrors(nameof(UnitPrice))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Discount percentage is required.")]
    [Range(0, 100, ErrorMessage = "Discount percentage must be between 0% and 100%.")]
    [NotifyPropertyChangedFor(nameof(DiscountPercentageErrors))]
    private int _discountPercentage;

    public string? DiscountPercentageErrors => GetErrors(nameof(DiscountPercentage))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Url]
    [NotifyPropertyChangedFor(nameof(ImageUrlErrors))]
    private string? _imageUrl;

    public string? ImageUrlErrors => GetErrors(nameof(ImageUrl))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    private bool _isDeleted;

    [ObservableProperty]
    private bool _isInStock;

    [ObservableProperty]
    private string? _successMessage;

    [ObservableProperty]
    private string? _errorMessage;

    [RelayCommand]
    public async Task AddProductAsync()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        ClearErrors();

        ValidateProperty(Name, nameof(Name));
        ValidateProperty(Description, nameof(Description));
        ValidateProperty(StockQuantity, nameof(StockQuantity));
        ValidateProperty(UnitPrice, nameof(UnitPrice));
        ValidateProperty(DiscountPercentage, nameof(DiscountPercentage));
        ValidateProperty(ImageUrl, nameof(ImageUrl));

        if (HasErrors)
        {
            return;
        }

        var product = new CreateProductDto
        {
            CategoryId = CategoryId,
            Name = Name,
            Description = Description,
            StockQuantity = StockQuantity,
            UnitPrice = UnitPrice,
            DiscountPercentage = DiscountPercentage,
            IsDeleted = IsDeleted,
            IsInStock = IsInStock,
            ImageUrl = ImageUrl
        };

        var data = await _productService.AddProductAsync(product);

        if (data is null)
        {
            ErrorMessage = "Unable to add product at this time";
            return;
        }

        if (string.IsNullOrEmpty(data.ErrorMessage))
        {
            ClearProductAdd();
            SuccessMessage = "Product added successfully";
            _messenger.Send(new ProductAddedMessage(data));
        }
        else
        {
            ErrorMessage = data.ErrorMessage;
        }
    }

    private void ClearProductAdd()
    {
        Name = string.Empty;
        Description = string.Empty;
        StockQuantity = 0;
        UnitPrice = 0.0m;
        DiscountPercentage = 0;
        IsDeleted = false;
        IsInStock = false;
    }

    [ObservableProperty]
    private bool _isLoading;
    [ObservableProperty]
    private CategoryAdminSummaryData? _selectedCategory;

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

        var result = await _categoryService.GetCategoriesForAdminAsync(queryParams);

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

    [RelayCommand]
    private void ClearFilters()
    {
        SearchByDescription = null;
    }

    async partial void OnSearchByDescriptionChanged(string? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadCategoriesAsync);

    partial void OnSelectedCategoryChanged(CategoryAdminSummaryData? value)
    {
        if (value is not null)
        {
            CategoryId = value.Id;
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToProductPageMessage());
    }

}
