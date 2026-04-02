using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Category;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class UpdateCategoryViewModel : ObservableValidator
{
    private readonly ICategoryService _categoryService;
    public CategoryAdminSummaryData Category { get; set; }
    private readonly IMessenger _messenger;

    public UpdateCategoryViewModel(
        ICategoryService categoryService,
        CategoryAdminSummaryData category,
        IMessenger messenger)
    {
        _categoryService = categoryService;
        Category = category;
        _messenger = messenger;
        _name = Category.Name;
        _description = Category.Description;
    }

    [ObservableProperty]
    [Required(ErrorMessage = "Category name is required.")]
    [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
    [NotifyPropertyChangedFor(nameof(NameErrors))]
    private string _name;

    public string? NameErrors => GetErrors(nameof(Name))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    [MaxLength(500, ErrorMessage = "Category description cannot exceed 500 characters.")]
    [NotifyPropertyChangedFor(nameof(DescriptionErrors))]
    private string? _description;

    public string? DescriptionErrors => GetErrors(nameof(Description))
        .FirstOrDefault()?.ErrorMessage;

    [ObservableProperty]
    public string? _successMessage;

    [ObservableProperty]
    public string? _errorMessage;

    [RelayCommand]
    public async Task UpdateCategoryAsync()
    {
        SuccessMessage = null;
        ErrorMessage = null;

        ClearErrors();

        ValidateProperty(Name, nameof(Name));
        ValidateProperty(Description, nameof(Description));

        if (HasErrors)
        {
            return;
        }

        var category = new UpdateCategoryDto
        {
            Id = Category.Id,
            Name = Name,
            Description = Description
        };


        var data = await _categoryService.UpdateCategoryAsync(category);

        if (data is null)
        {
            ErrorMessage = $"Unable to update category {Category.Id} at this time";
            return;
        }

        if (string.IsNullOrEmpty(data.ErrorMessage))
        {
            ClearCategoryUpdate();
            SuccessMessage = $"Category {Category.Id} updated successfully";
            _messenger.Send(new CategoryUpdatedMessage(data));
        }
        else
        {
            ErrorMessage = data.ErrorMessage;
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        _messenger.Send(new NavigateBackToViewCategoriesForUpdateCategoryMessage());
    }

    private void ClearCategoryUpdate()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}
