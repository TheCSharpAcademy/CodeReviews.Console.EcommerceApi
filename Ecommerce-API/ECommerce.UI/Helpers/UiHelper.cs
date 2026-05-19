using ECommerce.Shared.Models;
using ECommerce.UI.Enums;
using ECommerce.UI.Interfaces;
using Spectre.Console;
using Spectre.Console.Rendering;
using static ECommerce.UI.Helpers.DisplayHelper;

namespace ECommerce.UI.Helpers;

internal class UiHelper(ITagService tagService)
{
    internal static Table BuildItemTable(PagedResponse<ItemDto> response)
    {
        var table = new Table().ShowRowSeparators();
        
        table.AddColumn($"[{White}]Title[/]");
        table.AddColumn($"[{White}]Artist[/]");
        table.AddColumn($"[{White}]Type / Format[/]");
        table.AddColumn($"[{White}]Genre[/]");
        table.AddColumn($"[{White}]Tags[/]");
        table.AddColumn($"[{White}]Price[/]");
        table.AddColumn($"[{White}]Item ID[/]");

        foreach (var item in response.Data)
        {
            var tags = string.Join(", ", item.Tags.Select(t => t.TagName));
            table.AddRow(item.Name, item.Artist, $"{item.Type} / {item.Format}", item.Genre, tags, $"{item.Price}", $"{item.ItemId}");
        }

        return table;
    }
    
    internal Table BuildTagTable(PagedResponse<TagDto> response)
    {
        var table = new Table();
        
        table.AddColumn($"[{White}]Title[/]");

        foreach (var tag in response.Data)
        {
            table.AddRow(tag.TagName);
        }

        return table;
    }
    
    internal static Table BuildSaleTable(PagedResponse<SaleDto> response)
    {
        var table = new Table();
        
        table.AddColumn($"[{White}]Sale ID[/]", col => col.Centered());
        table.AddColumn($"[{White}]Sold Items[/]", col => col.Centered());
        table.AddColumn($"[{White}]Total Price[/]", col => col.Centered());

        foreach (var sale in response.Data)
        {
            var soldItems = string.Join(", ", sale.SoldItems.Select(s => $"{s.Item.Name} x {s.Quantity}"));
            table.AddRow($"{sale.SaleId}", soldItems, $"{sale.TotalPrice}");
        }

        return table;
    }

    /// <summary>
    ///     Displays a caught exception to the user based on the type of exception caught.
    ///     Handles HttpResponseExceptions for status codes 404, 400-499, and 500+.
    ///     Handles ArgumentNull and generic Argument exceptions.
    ///     All other exceptions are labelled with a generic 'unexpected exception' label.
    /// </summary>
    /// <param name="ex">Caught exception</param>
    internal static void DisplayCaughtException(Exception ex)
    {
        Console.Clear();

        if (ex is HttpRequestException { StatusCode: not null } exception)
        {
            switch ((int)exception.StatusCode)
            {
                case var code when code is 404:
                    DisplayWarning("The content you requested could not be found, please check that the " +
                                   "content you want exists and is accessible. | 404 Not Found");
                    break;

                case var code when code is >= 400 and < 500:
                    DisplayWarning("A client side error has occurred while processing your request, " +
                                   "please check that you are sending a good request to the API containing valid details. | 4xx");
                    break;

                case var code when code is > 500:
                    DisplayWarning("A server side error has occurred while processing your request, " +
                                   "please check that the API is working and all entered details are correct. | 5xx");
                    break;
                default:
                    DisplayWarning("An unknown error has occurred while attempting to interact with the API. | ???");
                    break;
            }
        }
        else if (ex is ArgumentException argException)
        {
            if (argException is ArgumentNullException)
                DisplayWarning("One or more of the arguments you have entered was null, " +
                               "please try again with non-null details.");
            else
                DisplayWarning("An error has occurred with one or more of the arguments you have entered, " +
                               "please check that any details you enter are correct before trying again.");
        }
        else
        {
            DisplayWarning("An unexpected error has occurred during runtime, " +
                           "please retry later or report the problem if it persists.");
        }

        WaitForUser();
    }

    internal static void WaitForUser()
    {
        DisplayMessage("Please press enter to continue.");
        Console.ReadLine();
    }

    /// <summary>
    ///     Gets an argument from the user
    /// </summary>
    /// <param name="prompt">Display prompt shown when prompting the user</param>
    /// <param name="instructions">Any extra instructions, E.G asking for a specific format.</param>
    /// <returns>null if the user wishes to exit the process, or else the value returned form the prompt.</returns>
    internal static string? GetArgument(string prompt, string? instructions = null)
    {
        const string backOption = "back";

        Console.Clear();
        DisplayInfo("Enter 'Back' to leave this menu.");
        if (instructions != null) DisplayInfo(instructions);

        var value = DisplayQuestion(prompt);

        return string.Equals(value, backOption, StringComparison.OrdinalIgnoreCase) ? null : value;
    }

    /// <summary>
    /// Displays a pagination menu, handling edge cases for first and last pages
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="totalPages"></param>
    /// <returns>The chosen PaginationController selection</returns>
    internal static PaginationController DisplayPaginationController(int pageNumber, int totalPages)
    {
        IEnumerable<PaginationController> options;
        if (totalPages == 1)
        {
            options = Enum.GetValues(typeof(PaginationController))
                .Cast<PaginationController>()
                .Where(p => p == PaginationController.Back)
                .ToList();
        }
        else if (pageNumber >= totalPages && totalPages > 1)
        {
            options = Enum.GetValues(typeof(PaginationController))
                .Cast<PaginationController>()
                .Where(p => p != PaginationController.NextPage)
                .ToList();
        }
        else if (pageNumber <= 1)
        {
            options = Enum.GetValues(typeof(PaginationController))
                .Cast<PaginationController>()
                .Where(p => p != PaginationController.LastPage)
                .ToList();
        }
        else
        {
            options = Enum.GetValues(typeof(PaginationController))
                .Cast<PaginationController>()
                .ToList();
        }

        return DisplayMenu(options);
    }

    internal static PaginationControllerWithSelection DisplayPaginationControllerWithSelectionOption(int pageNumber,
        int totalPages)
    {
        IEnumerable<PaginationControllerWithSelection> options;
        if (totalPages == 1)
        {
            options = Enum.GetValues(typeof(PaginationControllerWithSelection))
                .Cast<PaginationControllerWithSelection>()
                .Where(p => p == PaginationControllerWithSelection.Back || p == PaginationControllerWithSelection.SelectProduct)
                .ToList();
        }
        else if (pageNumber >= totalPages && totalPages > 1)
        {
            options = Enum.GetValues(typeof(PaginationControllerWithSelection))
                .Cast<PaginationControllerWithSelection>()
                .Where(p => p != PaginationControllerWithSelection.NextPage)
                .ToList();
        }
        else if (pageNumber <= 1)
        {
            options = Enum.GetValues(typeof(PaginationControllerWithSelection))
                .Cast<PaginationControllerWithSelection>()
                .Where(p => p != PaginationControllerWithSelection.LastPage)
                .ToList();
        }
        else
        {
            options = Enum.GetValues(typeof(PaginationControllerWithSelection))
                .Cast<PaginationControllerWithSelection>()
                .ToList();
        }

        return DisplayMenu(options);
    }
}