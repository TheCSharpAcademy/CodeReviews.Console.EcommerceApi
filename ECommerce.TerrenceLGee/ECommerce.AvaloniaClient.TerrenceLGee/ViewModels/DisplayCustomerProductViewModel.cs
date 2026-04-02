using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.OtherMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayCustomerProductViewModel : ObservableObject
{
    private readonly int _productId;
    private readonly IProductService _productService;
    private readonly IMessenger _messenger;

    [ObservableProperty]
    private ProductData? _product;

    public DisplayCustomerProductViewModel(IProductService productService, int productId, IMessenger messenger)
    {
        _productService = productService;
        _productId = productId;
        _messenger = messenger;
    }

    public async Task GetProductAsync()
    {
        Product = await _productService.GetProductAsync(_productId);
        if (Product is null) GoBack();
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToPreviousPageMessage());
    }
}
