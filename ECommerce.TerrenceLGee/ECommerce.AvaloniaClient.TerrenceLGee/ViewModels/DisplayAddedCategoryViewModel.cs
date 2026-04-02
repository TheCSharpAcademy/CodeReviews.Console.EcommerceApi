using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayAddedCategoryViewModel : ViewModelBase
{
    public CategoryAdminData Category { get; }
    private readonly IMessenger _messenger;


    public DisplayAddedCategoryViewModel(CategoryAdminData category, IMessenger messenger)
    {
        Category = category;
        _messenger = messenger;
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToAddCategoryMessage());
    }

    [RelayCommand]
    private void GoBackToPreviousPage()
    {
        _messenger.Send(new NavigateBackToCategoryPageMessage());
    }
}
