using System.Net;
using System.Text;
using ECommerce.Shared;
using ECommerce.Shared.Models;
using ECommerce.UI.Enums;
using ECommerce.UI.Helpers;
using ECommerce.UI.Interfaces;
using Spectre.Console;
using static ECommerce.UI.Helpers.DisplayHelper;

namespace ECommerce.UI.UserInterface.AdministratorUi;

internal class ManageProductsUi(IItemService itemService, IVerificationService verificationService, ManageProductTagsUi tagsMenu)
{
    //------- Menu Methods -------
    internal async Task ManageProductsMenu()
    {
        while (true)
        {
            Console.Clear();
            var option = DisplayMenu<ManageProductsMenuOption>();

            switch (option)
            {
                case ManageProductsMenuOption.ReviewProducts:
                    await ReviewProductsMenu();
                    break;
                case ManageProductsMenuOption.SearchProducts:
                    await SearchProducts();
                    break;
                case ManageProductsMenuOption.CreateNewProduct:
                    await CreateProduct();
                    break;
                case ManageProductsMenuOption.DeleteProduct:
                    await DeleteProduct();
                    break;
                case ManageProductsMenuOption.Back:
                    return;
            }
        }
    }

    //------- CRUD Menus -------
    /// <summary>
    ///     Presents a list of products to the user and handles pagination
    /// </summary>
    /// <param name="searchTerm">optional search term to filter results</param>
    /// <param name="searchGenre">optional genre filter</param>
    /// <param name="searchTags">options tag filter</param>
    private async Task ReviewProductsMenu(string? searchTerm = null, string? searchGenre = null, List<TagDto>? searchTags = null)
    {
        var pageNumber = 1;
        while (true)
            try
            {
                Console.Clear();
                var response = await itemService.GetItemsAsync(pageNumber, searchTerm: searchTerm, searchGenre: searchGenre, tags: searchTags);
                
                var table = UiHelper.BuildItemTable(response);
                DisplayTable(table);

                var option = UiHelper.DisplayPaginationController(response.PageNumber, response.TotalPages);
                switch (option)
                {
                    case PaginationController.LastPage:
                        pageNumber = pageNumber == 1 ? 1 : pageNumber - 1;
                        break;
                    case PaginationController.NextPage:
                        pageNumber += 1;
                        break;
                    case PaginationController.Back:
                        return;
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    DisplayWarning("No results were found when searching, please try a different search or make sure the database isn't empty.");
                    UiHelper.WaitForUser();
                    return;
                }
                UiHelper.DisplayCaughtException(ex);
                return;
            }
    }

    private async Task<ItemDto?> SelectProduct(string? searchTerm = null, string? searchGenre = null,
        List<TagDto>? searchTags = null)
    {
        var pageNumber = 1;
        while (true)
        {
            try
            {
                Console.Clear();
                var response = await itemService.GetItemsAsync(pageNumber, searchTerm: searchTerm,
                    searchGenre: searchGenre, tags: searchTags);
                
                var table = UiHelper.BuildItemTable(response);
                DisplayTable(table);
                
                var option = UiHelper.DisplayPaginationControllerWithSelectionOption(response.PageNumber, response.TotalPages);
                switch (option)
                {
                    case PaginationControllerWithSelection.LastPage:
                        pageNumber = pageNumber == 1 ? 1 : pageNumber - 1;
                        continue;
                    case PaginationControllerWithSelection.NextPage:
                        pageNumber += 1; 
                        continue;
                    case PaginationControllerWithSelection.SelectProduct:
                        break;
                    case PaginationControllerWithSelection.Back:
                        return null;
                }

                Dictionary<string, ItemDto> dictionary = new();
                foreach (var item in response.Data)
                {
                    dictionary.Add($"{item.Name} - {item.Artist} | {item.Price}", item);
                }

                var selection = DisplayPrompt(dictionary.Keys.ToList());
                return dictionary[selection];
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    DisplayWarning("No results were found when searching, please try a different search or make sure the database isn't empty.");
                    UiHelper.WaitForUser();
                    return null;
                }
                UiHelper.DisplayCaughtException(ex);
                return null;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="returnProduct">Decides if the method should return the selected product or not</param>
    /// <returns>The selected product, or null if returnProduct is set to false</returns>
    private async Task<ItemDto?> SearchProducts(bool returnProduct = false)
    {
        while (true)
        {
            Console.Clear();

            var enumNames = Enum.GetNames<SearchController>().ToList();
            var options = DisplayMultiPrompt(enumNames, requireChoice: false);

            List<SearchController> selectedFilters;
            try
            {
                selectedFilters = options
                    .Select(s => Enum.Parse<SearchController>(s))
                    .ToList();
            }
            catch (Exception)
            {
                DisplayWarning("Please select to either search by a given search term, or filter by genre.");
                UiHelper.WaitForUser();
                continue;
            }

            string? searchTerm = null;
            string? searchGenre = null;
            List<TagDto>? searchTags = null;

            if (selectedFilters.Contains(SearchController.SearchByTerm))
            {
                Console.Clear();
                searchTerm = DisplayQuestion("Please enter a term to search for:");
            }

            if (selectedFilters.Contains(SearchController.FilterByTags))
            {
                switch (DisplayMenu<SearchTagsController>())
                {
                    case SearchTagsController.SearchForSpecificTag:
                        searchTags = await tagsMenu.SearchTags(true);
                        break;
                    
                    case SearchTagsController.BrowseAllTags:
                        searchTags = await tagsMenu.SelectTag(allowMultiSelection: true);
                        break;
                    
                    case SearchTagsController.Back:
                        break;
                }
                
                if (searchTags is null || searchTags.Count == 0) return null;
            }

            if (selectedFilters.Contains(SearchController.FilterByGenre))
            {
                Console.Clear();
                searchGenre = DisplayQuestion("Please enter a genre to filter by:");
            }

            var item = await SelectProduct(searchTerm, searchGenre, searchTags);
            if (returnProduct) return item;

            if (!await AnsiConsole.ConfirmAsync("Would you like to perform another search?"))
                return null;
        }
    }

    private async Task CreateProduct()
    {
        ItemFormat format;
        ItemType type;
        decimal price;
        var enteredDetails = new StringBuilder();

        var title = UiHelper.GetArgument("Please enter the item title:");
        if (title is null) return;
        enteredDetails.Append($"Title: {title} ");

        var artist = UiHelper.GetArgument("Please enter the artist's name:", enteredDetails.ToString());
        if (artist is null) return;
        enteredDetails.Append($"Artist: {artist} ");

        var genre = UiHelper.GetArgument("Please enter the genre:", enteredDetails.ToString());
        if (genre is null) return;
        enteredDetails.Append($"Genre: {genre} ");

        var tagOption = DisplayMenu<TagAdditionMethodForItem>();
        List<TagDto>? selectedTags = null;

        switch (tagOption)
        {
            case TagAdditionMethodForItem.SearchForAnExistingTag:
                selectedTags = await tagsMenu.SearchTags(returnSearchTags: true);
                break;
            case TagAdditionMethodForItem.CreateNewTag:
                var response = UiHelper.GetArgument("Please enter tag names separated by a ','");
                if (response is null) return;
                selectedTags = response.Split(',')
                    .Select(name => new TagDto { TagName = name.Trim() })
                    .ToList();
                break;
            case TagAdditionMethodForItem.CreateWithoutTags:
                break;
        }

        string tags = selectedTags is not null
            ? string.Join(", ", selectedTags.Select(t => t.TagName))
            : string.Empty;
        if (string.IsNullOrWhiteSpace(tags)) tags = "No Tags";
        enteredDetails.Append($"Tags: {tags} ");

        format = DisplayMenu<ItemFormat>();
        enteredDetails.Append($"Format: {format} ");
            
        type = DisplayMenu<ItemType>();
        enteredDetails.Append($"Type: {type} ");

        while (true)
        {
            var unparsedPrice = UiHelper.GetArgument("Please enter the item's price:", enteredDetails.ToString());
            if (unparsedPrice is null) return;

            if (!verificationService.TryValidateItemPrice(unparsedPrice, out price, out var errorMessage))
            {
                DisplayWarning(errorMessage ?? "Please enter a valid price for this product.");
                UiHelper.WaitForUser();
                continue;
            }

            break;
        }

        try
        {
            await itemService.PostItemAsync(format, type, title, artist, price, genre, tags);
            DisplaySuccess("Successfully created product.");
            UiHelper.WaitForUser();
        }
        catch (HttpRequestException ex)
        {
            UiHelper.DisplayCaughtException(ex);
        }
    }

    private async Task DeleteProduct()
    {
        while (true)
        {
            try
            {
                var item = await SearchProducts(returnProduct: true);
                if (item is null) return;
                
                if (await AnsiConsole.ConfirmAsync("Are you sure you want to delete this item?"))
                {
                    await itemService.DeleteItemAsync(item.ItemId);
                    DisplaySuccess("Successfully deleted product.");
                }
                else
                {
                    DisplaySuccess("Deletion was cancelled..");
                }

                UiHelper.WaitForUser();
                return;
            }
            catch (Exception ex)
            {
                UiHelper.DisplayCaughtException(ex);
                return;
            }
        }
    }
}