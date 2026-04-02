using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplaySelectedProductFromAdminCategoryDetailViewModel : ObservableObject
{
    public int ProductId { get; }
    public int CategoryId { get; }

    [ObservableProperty]
    public ProductAdminData? _product;

    private readonly IProductService _productService;
    private readonly IMessenger _messenger;

    public DisplaySelectedProductFromAdminCategoryDetailViewModel(
        IProductService productService, 
        int productId, 
        int categoryId, 
        IMessenger messenger)
    {
        _productService = productService;
        ProductId = productId;
        CategoryId = categoryId;
        _messenger = messenger;
    }

    public async Task GetProductAsync()
    {
        Product = await _productService.GetProductForAdminAsync(ProductId);
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToAdminCategoryDetailView(CategoryId));
    }
}
