using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.ProductMessages;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ProductOperationsViewModel : ViewModelBase
{
    private readonly IMessenger _messenger;

    public ProductOperationsViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }

    [RelayCommand]
    private void AddProduct()
    {
        _messenger.Send(new AddProductMessage());
    }

    [RelayCommand]
    private void UpdateProduct()
    {
        _messenger.Send(new UpdateProductMessage());
    }

    [RelayCommand]
    private void DeleteProduct()
    {
        _messenger.Send(new DeleteProductMessage());
    }

    [RelayCommand]
    private void RestoreProduct()
    {
        _messenger.Send(new RestoreProductMessage());
    }

    [RelayCommand]
    private void ViewProducts()
    {
        _messenger.Send(new NavigateBackToAllAdminProductsMessage());
    }
}
