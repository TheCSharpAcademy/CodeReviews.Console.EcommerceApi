using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using ECommerce.AvaloniaClient.TerrenceLGee.Messages.CategoryMessages;
using ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Category;
using ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.ViewModels;

public partial class AddCategoryViewModel : ObservableValidator
{
    private readonly ICategoryService _categoryService;
    private readonly IMessenger _messenger;

    public AddCategoryViewModel(ICategoryService categoryService, IMessenger messenger)
    {
        _categoryService = categoryService;
        _messenger = messenger;
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
    private async Task AddCategoryAsync()
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

        var category = new CreateCategoryDto
        {
            Name = Name,
            Description = Description
        };


        var data = await _categoryService.AddCategoryAsync(category);

        if (data is null)
        {
            ErrorMessage = "Unable to add category at this time";
            return;
        }

        if (string.IsNullOrEmpty(data.ErrorMessage))
        {
            ClearCategoryAdd();
            SuccessMessage = "Category added successfully";
            _messenger.Send(new CategoryAddedMessage(data));
        }
        else
        {
            ErrorMessage = data.ErrorMessage;
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        _messenger.Send(new NavigateBackToCategoryPageMessage());
    }

    private void ClearCategoryAdd()
    {
        Name = string.Empty;
        Description = string.Empty;
    }
}
