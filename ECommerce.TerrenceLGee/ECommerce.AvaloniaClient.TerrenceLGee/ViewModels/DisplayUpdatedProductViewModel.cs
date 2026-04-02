using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.ProductMessages;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayUpdatedProductViewModel : ViewModelBase
{
    public ProductAdminData Product { get; }
    private readonly IMessenger _messenger;

    public DisplayUpdatedProductViewModel(ProductAdminData product, IMessenger messenger)
    {
        Product = product;
        _messenger = messenger;
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToUpdateProductMessage());
    }

    [RelayCommand]
    private void GoBackToProductPage()
    {
        _messenger.Send(new NavigateBackToProductPageMessage());
    }
}
