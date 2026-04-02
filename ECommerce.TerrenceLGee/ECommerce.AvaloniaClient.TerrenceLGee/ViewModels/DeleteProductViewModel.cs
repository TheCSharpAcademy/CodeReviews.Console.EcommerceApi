using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.OtherMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DeleteProductViewModel : ProductsAdminBaseViewModel
{
    private readonly IProductService _productService;
    private readonly IMessenger _messenger;

    public DeleteProductViewModel(IProductService productService, IMessenger messenger)
    {
        _productService = productService;
        _messenger = messenger;
        LoadProductsCommand.Execute(null);
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToPreviousPageMessage());
    }


    private async Task DeleteProductAsync(ProductAdminData value)
    {
        if (value is not null)
        {
            var box = MessageBoxManager
                .GetMessageBoxStandard("Delete", $"Delete {value.Name}?", ButtonEnum.YesNo, Icon.Warning,
                null, WindowStartupLocation.CenterOwner);

            var result = await box.ShowAsync();

            if (result == ButtonResult.Yes)
            {
                var (success, data) = await _productService.DeleteProductAsync(value.Id);

                if (success)
                {
                    SelectedProduct = null;
                    box = MessageBoxManager
                        .GetMessageBoxStandard("Success", $"{data}", ButtonEnum.Ok, Icon.Success,
                        null, WindowStartupLocation.CenterOwner);

                    result = await box.ShowAsync();
                }
                else
                {
                    SelectedProduct = null;
                    box = MessageBoxManager
                        .GetMessageBoxStandard("Error", $"{data}", ButtonEnum.Ok, Icon.Error, null,
                        WindowStartupLocation.CenterOwner);

                    result = await box.ShowAsync();

                }
                await LoadProductsAsync();
            }
        }
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
        Dispatcher.UIThread.InvokeAsync(
            () => DeleteProductAsync(product));
    }
}
