using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.ProductMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ViewProductsForAdminViewModel : ProductsAdminBaseViewModel
{
    private readonly IProductService _productService;
    private readonly IMessenger _messenger;

    public ViewProductsForAdminViewModel(IProductService productService, IMessenger messenger)
    {
        _productService = productService;
        _messenger = messenger;
        LoadProductsCommand.Execute(null);
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToProductPageMessage());
    }

    protected override async Task<ProductsAdminRoot?> GetProductsAsync()
    {
        var queryParams = new ProductQueryParams
        {
            Page = Page,
            PageSize = PageSize,
            MinUnitPrice = MinUnitPrice,
            MaxUnitPrice = MaxUnitPrice,
            MinStockQuantity = MinStockQuantity,
            MaxStockQuantity = MaxStockQuantity,
            MinDiscountPercentage = MinDiscountPercentage,
            MaxDiscountPercentage = MaxDiscountPercentage,
            CategoryName = CategoryName,
            Description = Description,
            InStock = InStock,
            IsDeleted = IsDeleted
        };

        return await _productService.GetProductsForAdminAsync(queryParams);
    }

    protected override void OnProductSelected(ProductAdminData product)
    {
        _messenger.Send(new ProductSelectedForAdminMessage(product.Id));
    }
}
