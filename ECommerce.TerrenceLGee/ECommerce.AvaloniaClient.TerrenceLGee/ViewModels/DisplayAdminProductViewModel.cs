using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.OtherMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Product;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayAdminProductViewModel : ObservableObject
{
    private int _productId { get; }

    [ObservableProperty]
    private ProductAdminData? _product;

    private readonly IProductService _productService;
    private readonly IMessenger _messenger;

    public DisplayAdminProductViewModel(IProductService productService, int productId, IMessenger messenger)
    {
        _productService = productService;
        _productId = productId;
        _messenger = messenger;
    }

    public async Task GetProductAsync()
    {
        Product = await _productService.GetProductForAdminAsync(_productId);
        if (Product is null) GoBack();
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToPreviousPageMessage());
    }
}
