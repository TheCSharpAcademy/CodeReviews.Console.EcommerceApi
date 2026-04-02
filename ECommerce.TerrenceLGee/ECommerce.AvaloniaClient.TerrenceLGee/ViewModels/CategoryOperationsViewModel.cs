using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class CategoryOperationsViewModel : ViewModelBase
{
    private readonly IMessenger _messenger;

    public CategoryOperationsViewModel(IMessenger messenger)
    {
        _messenger = messenger;
    }

    [RelayCommand]
    private void AddCategory()
    {
        _messenger.Send(new AddCategoryMessage());
    }

    [RelayCommand]
    private void UpdateCategory()
    {
        _messenger.Send(new UpdateCategoryMessage());
    }

    [RelayCommand]
    private void ViewCategories()
    {
        _messenger.Send(new NavigateBackToAllAdminCategoriesMessage());
    }
}
