using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.ProductMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class UpdateProductViewModel : ObservableValidator
{
    private readonly IProductService _productService;
    public ProductAdminData Product { get; set; }
    private readonly IMessenger _messenger;

    public UpdateProductViewModel(
        IProductService productService,
        ProductAdminData product,
        IMessenger messenger)
    {
        _productService = productService;
        Product = product;
        _messenger = messenger;
        _name = Product.Name;
        _description = Product.Description;
        _stockQuantity = Product.StockQuantity;
        _discountPercentage = Product.DiscountPercentage;
        _isDeleted = Product.IsDeleted;
        _isInStock = Product.IsInStock;
        _imageUrl = Product.ImageUrl;
    }

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
    public int _stockQuantity;

    public string? StockQuantityErrors => GetErrors(nameof(StockQuantity))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [Required(ErrorMessage = "Discount percentage is required.")]
    [Range(0, 100, ErrorMessage = "Discount percentage must be between 0% and 100%.")]
    [NotifyPropertyChangedFor(nameof(DiscountPercentageErrors))]
    public int _discountPercentage;

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
    private async Task UpdateProductAsync()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        ClearErrors();

        ValidateProperty(Name, nameof(Name));
        ValidateProperty(Description, nameof(Description));
        ValidateProperty(StockQuantity, nameof(StockQuantity));
        ValidateProperty(DiscountPercentage, nameof(DiscountPercentage));
        ValidateProperty(ImageUrl, nameof(ImageUrl));

        if (HasErrors)
        {
            return;
        }

        var product = new UpdateProductDto
        {
            Id = Product.Id,
            CategoryId = Product.CategoryId,
            Name = Name,
            Description = Description,
            StockQuantity = StockQuantity,
            DiscountPercentage = DiscountPercentage,
            IsDeleted = IsDeleted,
            IsInStock = IsInStock,
            ImageUrl = ImageUrl
        };

        var data = await _productService.UpdateProductAsync(product);

        if (data is null)
        {
            ErrorMessage = $"Unable to update product {Product.Id} in category {Product.CategoryId}";
            return;
        }

        if (string.IsNullOrEmpty(data.ErrorMessage))
        {
            ClearUpdateProduct();
            SuccessMessage = $"Product {Product.Id} in category {Product.CategoryId} successfully updated";
            _messenger.Send(new ProductUpdatedMessage(data));
        }
        else
        {
            ErrorMessage = data.ErrorMessage;
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        _messenger.Send(new NavigateBackToAllAdminProductsFromUpdateView());
    }

    private void ClearUpdateProduct()
    {
        Name = string.Empty;
        Description = string.Empty;
        StockQuantity = 0;
        DiscountPercentage = 0;
        IsDeleted = false;
        IsInStock = false;
    }
}
