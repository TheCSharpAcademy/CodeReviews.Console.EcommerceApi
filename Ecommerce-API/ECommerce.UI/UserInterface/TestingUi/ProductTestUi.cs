using ECommerce.UI.Enums;
using ECommerce.UI.Helpers;
using ECommerce.UI.Interfaces;
using Spectre.Console;
using static ECommerce.UI.Helpers.DisplayHelper;

namespace ECommerce.UI.UserInterface.TestingUi;

internal class ProductTestUi(IItemService itemService, CheckoutUi checkoutUi)
{
    /// <summary>
    ///     Presents a list of products to the user and handles pagination
    /// </summary>
    /// <param name="searchTerm">optional search term to filter results</param>
    /// <param name="searchGenre">optional genre filter</param>
    internal async Task ReviewProductsMenu(string? searchTerm = null, string? searchGenre = null)
    {
        var pageNumber = 1;
        while (true)
            try
            {
                Console.Clear();
                var response =
                    await itemService.GetItemsAsync(pageNumber, searchTerm: searchTerm, searchGenre: searchGenre);
                var table = UiHelper.BuildItemTable(response);


                DisplayTable(table);

                var option = DisplayMenu<PaginationControllerWithAddToCart>();
                switch (option)
                {
                    case PaginationControllerWithAddToCart.LastPage:
                        pageNumber = pageNumber == 1 ? 1 : pageNumber - 1;
                        break;
                    case PaginationControllerWithAddToCart.NextPage:
                        pageNumber += 1;
                        break;
                    case PaginationControllerWithAddToCart.AddToCart:
                        await checkoutUi.AddItemToCartAsync(response);
                        break;
                    case PaginationControllerWithAddToCart.Back:
                        return;
                }
            }
            catch (HttpRequestException ex)
            {
                UiHelper.DisplayCaughtException(ex);
                return;
            }
    }

    internal async Task SearchProducts()
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
            catch (Exception ex)
            {
                DisplayWarning("Please select to either search by a given search term, or filter by genre.");
                UiHelper.WaitForUser();
                continue;
            }

            string? searchTerm = null;
            string? searchGenre = null;

            if (selectedFilters.Contains(SearchController.SearchByTerm))
            {
                Console.Clear();
                searchTerm = DisplayQuestion("Please enter a term to search for:");
            }

            if (selectedFilters.Contains(SearchController.FilterByGenre))
            {
                Console.Clear();
                searchGenre = DisplayQuestion("Please enter a genre to filter by:");
            }

            await ReviewProductsMenu(searchTerm, searchGenre);

            if (!await AnsiConsole.ConfirmAsync("Would you like to perform another search?"))
                return;
        }
    }
}