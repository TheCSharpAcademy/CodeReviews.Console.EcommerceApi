using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;
using System;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class DisplayUpdatedCategoryViewModel : ViewModelBase
{
    public CategoryAdminData Category { get; }
    private readonly IMessenger _messenger;

    public DisplayUpdatedCategoryViewModel(CategoryAdminData category, IMessenger messenger)
    {
        Category = category;
        _messenger = messenger;
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToViewCategoriesForUpdateCategoryMessage());
    }

    [RelayCommand]
    private void GoBackToCategoryPage()
    {
        _messenger.Send(new NavigateBackToCategoryPageMessage());
    }
}
