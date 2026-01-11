using System.Reflection;
using Spectre.Console;

namespace ECommerceApp.ConsoleClient.Utilities;

/// <summary>
/// Utility for rendering data collections as formatted tables in the console.
/// Abstracts away object IDs by using 1-based index numbers.
/// </summary>
public static class TableRenderer
{
    /// <summary>
    /// Renders a collection of items as a table and returns the selected item.
    /// Uses 1-based index numbers instead of IDs for user-friendly selection.
    /// </summary>
    /// <typeparam name="T">The type of items to display</typeparam>
    /// <param name="items">The items to display in the table</param>
    /// <param name="title">The table title</param>
    /// <param name="excludeColumns">Column names to exclude from display</param>
    /// <returns>The selected item, or null if cancelled</returns>
    public static T? SelectFromTable<T>(
        IList<T> items,
        string title,
        int indexOffset = 0,
        params string[] excludeColumns
    )
        where T : class
    {
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No items to display[/]");
            return null;
        }

        var table = new Table { Title = new TableTitle(title) };
        table.AddColumn("[bold]#[/]");

        var properties = GetDisplayProperties<T>(excludeColumns);
        foreach (var prop in properties)
        {
            table.AddColumn($"[bold]{prop.Name}[/]");
        }

        for (int i = 0; i < items.Count; i++)
        {
            var cells = new List<string> { (indexOffset + i + 1).ToString() };
            foreach (var prop in properties)
            {
                var value = prop.GetValue(items[i]);
                cells.Add(FormatValue(value));
            }
            table.AddRow(cells.ToArray());
        }

        AnsiConsole.Write(table);

        // Prompt for selection
        var choices = Enumerable
            .Range(indexOffset + 1, items.Count)
            .Select(i => i.ToString())
            .Concat(new[] { "Cancel" })
            .ToList();

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("Select an item:").AddChoices(choices)
        );

        if (choice == "Cancel" || !int.TryParse(choice, out int selectedIndex))
            return null;

        return items[selectedIndex - indexOffset - 1];
    }

    /// <summary>
    /// Creates a selection prompt that shows index and item name together.
    /// More user-friendly than selecting by number alone.
    /// Supports pagination for lists with more than 32 items.
    /// </summary>
    public static T? SelectFromPrompt<T>(
        IList<T> items,
        string title,
        int indexOffset = 0,
        string nameProperty = "Name"
    )
        where T : class
    {
        var formatter = BuildNameFormatter<T>(nameProperty);
        return SelectWithPaging(items, title, indexOffset, formatter);
    }

    /// <summary>
    /// Creates a selection prompt with a custom display function for formatting items.
    /// Allows flexible display of item information (e.g., date + customer name for sales).
    /// </summary>
    public static T? SelectFromPrompt<T>(
        IList<T> items,
        string title,
        int indexOffset,
        Func<T, string> displayFormatter
    )
        where T : class
    {
        return SelectWithPaging(items, title, indexOffset, displayFormatter);
    }

    /// <summary>
    /// Displays a collection of items as a formatted table without selection.
    /// </summary>
    public static void DisplayTable<T>(
        IList<T> items,
        string title,
        int indexOffset = 0,
        params string[] excludeColumns
    )
        where T : class
    {
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No items to display[/]");
            return;
        }

        var table = new Table { Title = new TableTitle(title) };
        table.AddColumn("[bold]#[/]");

        var properties = GetDisplayProperties<T>(excludeColumns);
        foreach (var prop in properties)
        {
            table.AddColumn($"[bold]{prop.Name}[/]");
        }

        for (int i = 0; i < items.Count; i++)
        {
            var cells = new List<string> { (indexOffset + i + 1).ToString() };
            foreach (var prop in properties)
            {
                var value = prop.GetValue(items[i]);
                cells.Add(FormatValue(value));
            }
            table.AddRow(cells.ToArray());
        }

        AnsiConsole.Write(table);
    }

    /// <summary>
    /// Gets properties suitable for display, excluding specified columns and internal fields.
    /// Automatically excludes ID columns (anything ending with "Id").
    /// Returns properties in the desired display order: Name, Description, Price, Stock, IsActive.
    /// </summary>
    private static List<PropertyInfo> GetDisplayProperties<T>(string[] excludeColumns)
        where T : class
    {
        var desiredOrder = new[]
        {
            "Name",
            "Description",
            "Price",
            "Stock",
            "IsActive",
            "CustomerName",
            "SaleDate",
            "TotalAmount",
            "CustomerEmail",
            "CustomerAddress",
        };

        var allProps = typeof(T)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p =>
                p.CanRead
                && !excludeColumns.Contains(p.Name, StringComparer.OrdinalIgnoreCase)
                && !p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase)
            )
            .Where(p =>
                p.PropertyType.IsPrimitive
                || p.PropertyType == typeof(string)
                || p.PropertyType == typeof(decimal)
                || p.PropertyType == typeof(DateTime)
                || p.PropertyType == typeof(DateTime?)
            )
            .ToList();

        // Sort by desired order, then add any remaining properties alphabetically
        var props = new List<PropertyInfo>();
        foreach (var propName in desiredOrder)
        {
            var prop = allProps.FirstOrDefault(p =>
                p.Name.Equals(propName, StringComparison.OrdinalIgnoreCase)
            );
            if (prop != null)
            {
                props.Add(prop);
                allProps.Remove(prop);
            }
        }

        // Add any remaining properties in alphabetical order
        props.AddRange(allProps.OrderBy(p => p.Name));

        return props;
    }

    /// <summary>
    /// Async selection prompt with dynamic pagination support.
    /// Fetches pages from the API on demand as the user navigates.
    /// </summary>
    public static async Task<T?> SelectFromPromptAsync<T>(
        Func<int, Task<List<T>>> fetchPageAsync,
        int totalCount,
        int pageSize,
        string title,
        Func<T, string> displayFormatter
    )
        where T : class
    {
        if (totalCount == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No items to display[/]");
            return null;
        }

        return await SelectWithDynamicPaging(
            fetchPageAsync,
            totalCount,
            pageSize,
            title,
            displayFormatter
        );
    }

    private static Func<T, string> BuildNameFormatter<T>(string nameProperty)
        where T : class
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var nameProp = properties.FirstOrDefault(p =>
            p.Name.Equals(nameProperty, StringComparison.OrdinalIgnoreCase)
        );

        return item => nameProp?.GetValue(item)?.ToString() ?? "Unknown";
    }

    private static T? SelectWithPaging<T>(
        IList<T> items,
        string title,
        int indexOffset,
        Func<T, string> displayFormatter
    )
        where T : class
    {
        if (items.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No items to display[/]");
            return null;
        }

        const int pageSize = 32;
        int currentPage = 0;
        int totalPages = (items.Count + pageSize - 1) / pageSize;

        while (true)
        {
            var (startIndex, endIndex) = GetPageBounds(currentPage, pageSize, items.Count);
            var choices = BuildChoices(
                items,
                displayFormatter,
                indexOffset,
                startIndex,
                endIndex,
                currentPage,
                totalPages
            );
            var choice = PromptSelection(title, choices, currentPage + 1, totalPages);

            var action = ParseNavigation(choice);
            if (action == PagingAction.Cancel)
                return null;

            if (HandleNavigation(ref currentPage, totalPages, action))
                continue;

            var selectedIndex = TryExtractIndex(choice, indexOffset);
            if (
                selectedIndex.HasValue
                && selectedIndex.Value >= 0
                && selectedIndex.Value < items.Count
            )
                return items[selectedIndex.Value];

            return null;
        }
    }

    private static async Task<T?> SelectWithDynamicPaging<T>(
        Func<int, Task<List<T>>> fetchPageAsync,
        int totalCount,
        int pageSize,
        string title,
        Func<T, string> displayFormatter
    )
        where T : class
    {
        // Use 0-based page index internally to align with BuildChoices/HandleNavigation logic.
        int currentPageIndex = 0;
        int totalPages = (totalCount + pageSize - 1) / pageSize;
        var currentPageItems = new List<T>();

        while (true)
        {
            if (currentPageItems.Count == 0)
            {
                currentPageItems = await fetchPageAsync(currentPageIndex + 1);
                if (currentPageItems.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No items on this page[/]");
                    return null;
                }
            }

            int indexOffset = currentPageIndex * pageSize;
            var choices = BuildChoices(
                currentPageItems,
                displayFormatter,
                indexOffset,
                0,
                currentPageItems.Count,
                currentPageIndex,
                totalPages
            );
            var choice = PromptSelection(title, choices, currentPageIndex + 1, totalPages);

            var action = ParseNavigation(choice);
            if (action == PagingAction.Cancel)
                return null;

            if (HandleNavigation(ref currentPageIndex, totalPages, action))
            {
                currentPageItems.Clear();
                continue;
            }

            var localIndex = TryExtractIndex(choice, indexOffset);
            if (localIndex.HasValue)
            {
                if (localIndex.Value >= 0 && localIndex.Value < currentPageItems.Count)
                    return currentPageItems[localIndex.Value];
            }

            return null;
        }
    }

    private static (int Start, int End) GetPageBounds(int currentPage, int pageSize, int totalItems)
    {
        int startIndex = currentPage * pageSize;
        int endIndex = Math.Min(startIndex + pageSize, totalItems);
        return (startIndex, endIndex);
    }

    private static List<string> BuildChoices<T>(
        IList<T> items,
        Func<T, string> displayFormatter,
        int indexOffset,
        int startIndex,
        int endIndex,
        int currentPage,
        int totalPages
    )
        where T : class
    {
        var choices = new List<string>();

        for (int i = startIndex; i < endIndex; i++)
        {
            var index = indexOffset + i + 1;
            choices.Add($"{index} - {displayFormatter(items[i])}");
        }

        if (totalPages > 1)
        {
            choices.Add("---");
            if (currentPage > 0)
                choices.Add("< Previous Page");
            if (currentPage < totalPages - 1)
                choices.Add("Next Page >");
        }

        choices.Add("Cancel");
        return choices;
    }

    private static string PromptSelection(
        string title,
        List<string> choices,
        int currentPage,
        int totalPages
    )
    {
        string pageIndicator = totalPages > 1 ? $" (Page {currentPage}/{totalPages})" : "";
        // Spectre.Console requires page size >= 3; guard small choice lists (e.g., 1 item + Cancel).
        var promptPageSize = Math.Max(3, Math.Min(40, choices.Count));
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[green]{title}{pageIndicator}[/]")
                .PageSize(promptPageSize)
                .MoreChoicesText("[grey](Use arrow keys to navigate)[/]")
                .AddChoices(choices)
        );
    }

    private static PagingAction ParseNavigation(string choice)
    {
        return choice switch
        {
            "Cancel" => PagingAction.Cancel,
            "---" => PagingAction.Cancel,
            "< Previous Page" => PagingAction.Previous,
            "Next Page >" => PagingAction.Next,
            _ => PagingAction.Select,
        };
    }

    private static bool HandleNavigation(ref int currentPage, int totalPages, PagingAction action)
    {
        if (action == PagingAction.Previous && currentPage > 0)
        {
            currentPage--;
            return true;
        }

        if (action == PagingAction.Next && currentPage < totalPages - 1)
        {
            currentPage++;
            return true;
        }

        return false;
    }

    private static int? TryExtractIndex(string choice, int indexOffset)
    {
        var firstPart = choice.Split('-')[0].Trim();
        if (int.TryParse(firstPart, out int selectedDisplayIndex))
        {
            return selectedDisplayIndex - indexOffset - 1;
        }

        return null;
    }

    private enum PagingAction
    {
        Select,
        Previous,
        Next,
        Cancel,
    }

    /// <summary>
    /// Formats a value for table display.
    /// </summary>
    private static string FormatValue(object? value)
    {
        if (value == null)
            return "[dim]—[/]";

        if (value is bool boolVal)
            return boolVal ? "[green]Yes[/]" : "[red]No[/]";

        if (value is DateTime dateVal)
            return dateVal.ToString("yyyy-MM-dd HH:mm");

        if (value is decimal decimalVal)
            return decimalVal.ToString("N2");

        var str = value.ToString() ?? "";
        return str.Length > 50 ? str[..47] + "..." : str;
    }
}
