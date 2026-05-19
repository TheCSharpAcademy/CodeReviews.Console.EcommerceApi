using System.Net;
using ECommerce.Shared.Models;
using ECommerce.UI.Enums;
using ECommerce.UI.Helpers;
using ECommerce.UI.Interfaces;
using Spectre.Console;
using static ECommerce.UI.Helpers.DisplayHelper;

namespace ECommerce.UI.UserInterface.AdministratorUi;

internal class ManageSalesUi(ISaleService saleService, IVerificationService verificationService)
{
    //------- Menu Methods -------
    internal async Task ManageSales()
    {
        while (true)
        {
            Console.Clear();
            var option = DisplayMenu<ManageSalesMenu>();

            switch (option)
            {
                case ManageSalesMenu.ReviewSales:
                    await ReviewSales();
                    break;
                case ManageSalesMenu.CreateNewSale:
                    await CreateNewSale();
                    break;
                case ManageSalesMenu.DeleteSale:
                    await DeleteSale();
                    break;
                case ManageSalesMenu.Back:
                    return;
            }
        }
    }

    //------- CRUD Menus -------
    private async Task ReviewSales()
    {
        var pageNumber = 1;
        while (true)
            try
            {
                Console.Clear();
                var response = await saleService.GetSalesAsync(pageNumber);
                var table = UiHelper.BuildSaleTable(response);
                
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

    private async Task<SaleDto?> SelectSale()
    {
        var pageNumber = 1;
        while (true)
        {
            try
            {
                var response = await saleService.GetSalesAsync(pageNumber);
                
                var table = UiHelper.BuildSaleTable(response);
                DisplayTable(table);
                
                var option = UiHelper.DisplayPaginationControllerWithSelectionOption(response.PageNumber, response.TotalPages);
                switch (option)
                {
                    case PaginationControllerWithSelection.LastPage:
                        pageNumber = pageNumber == 1 ? 1 : pageNumber - 1;
                        continue;
                    case PaginationControllerWithSelection.NextPage:
                        pageNumber++;
                        continue;
                    case PaginationControllerWithSelection.SelectProduct:
                        break;
                    case PaginationControllerWithSelection.Back:
                        return null;
                }

                Dictionary<string, SaleDto> dictionary = new();
                foreach (var sale in response.Data)
                {
                    dictionary.Add($"Sale ID {sale.SaleId} | Price {sale.TotalPrice}", sale);
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

    private async Task CreateNewSale()
    {
        Dictionary<int, int> idQuantityPair = [];
        while (true)
        {
            Console.Clear();
            var id = UiHelper.GetArgument("Please enter the ID of the sold item:");
            if (id is null) return;

            var quantity = UiHelper.GetArgument("Please enter the quantity of the sold item:");
            if (quantity is null) return;

            if (!verificationService.TryParseValidQuantity(quantity, out var parsedQuantity)
                || !int.TryParse(id, out var parsedId))
            {
                DisplayWarning("Please enter a valid number that is greater than or equal to 1.");
                UiHelper.WaitForUser();
                continue;
            }

            idQuantityPair.Add(parsedId, parsedQuantity);

            if (!await AnsiConsole.ConfirmAsync("Would you like to add another item to the sale?"))
                break;
        }

        try
        {
            await saleService.PostSaleAsync(idQuantityPair);
            DisplaySuccess("Successfully created a new sale record.");
            UiHelper.WaitForUser();
        }
        catch (HttpRequestException ex)
        {
            UiHelper.DisplayCaughtException(ex);
        }
    }

    private async Task DeleteSale()
    {
        while (true)
        {
            var sale = await SelectSale();
            if (sale is null) return;

            if (!await AnsiConsole.ConfirmAsync($"Are you sure you want to delete this sale?"))
            {
                DisplaySuccess("Deletion was cancelled.");
                UiHelper.WaitForUser();
                return;
            }

            try
            {
                await saleService.DeleteSaleAsync(sale.SaleId);
                DisplaySuccess("Successfully deleted the sale.");
                UiHelper.WaitForUser();
            }
            catch (HttpRequestException ex)
            {
                UiHelper.DisplayCaughtException(ex);
            }

            return;
        }
    }
}