using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Helpers;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.Shared.TerrenceLGee.Parameters.CategoryParameters;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class ViewCategoriesForAdminViewModel : ObservableObject
{
    private readonly ICategoryService _categoryService;
    private readonly IMessenger _messenger;
    public ObservableCollection<CategoryAdminSummaryData> Categories { get; } = [];


    [ObservableProperty]
    private bool _isLoading;
    [ObservableProperty]
    private CategoryAdminSummaryData? _selectedCategory;

    [ObservableProperty]
    private int _page = 1;
    [ObservableProperty]
    private int _pageSize = 10;
    [ObservableProperty]
    private int _totalPages;
    [ObservableProperty]
    private bool _hasPreviousPage;
    [ObservableProperty]
    private bool _hasNextPage;
    [ObservableProperty]
    private string? _searchByDescription;

    async partial void OnSearchByDescriptionChanged(string? value) => await FilterHelper.OnFilterChangedAsync(Page, LoadCategoriesAsync);

    public ViewCategoriesForAdminViewModel(ICategoryService categoryService, IMessenger messenger)
    {
        _categoryService = categoryService;
        _messenger = messenger;
        LoadCategoriesCommand.Execute(null);
    }

    [RelayCommand]
    private async Task LoadCategoriesAsync()
    {
        Page = 1;
        await FetchCategoriesAsync();
    }

    private async Task FetchCategoriesAsync()
    {
        IsLoading = false;

        var queryParams = new CategoryQueryParams
        {
            Page = Page,
            PageSize = PageSize,
            Description = SearchByDescription
        };

        var result = await _categoryService.GetCategoriesForAdminAsync(queryParams);

        if (result is not null)
        {
            Categories.Clear();

            foreach (var category in result.Data)
            {
                Categories.Add(category);
            }

            TotalPages = result.TotalPages;
            HasNextPage = Page < TotalPages;
            HasPreviousPage = Page > 1;
        }

        IsLoading = false;
    }

    [RelayCommand]
    private async Task NextPageAsync()
    {
        if (!HasNextPage) return;
        Page++;
        await FetchCategoriesAsync();
    }

    [RelayCommand]
    private async Task PreviousPageAsync()
    {
        if (!HasPreviousPage) return;
        Page--;
        await FetchCategoriesAsync();
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToCategoryPageMessage());
    }

    [RelayCommand]
    private void ClearFilters()
    {
        SearchByDescription = null;
    }

    partial void OnSelectedCategoryChanged(CategoryAdminSummaryData? value)
    {
        if (value is not null)
        {
            _messenger.Send(new CategorySelectedForAdminMessage(value.Id));
        }
    }
}
