using System.Text.Json;
using ECommerceApp.ConsoleClient.Helpers;
using ECommerceApp.ConsoleClient.Interfaces;
using ECommerceApp.ConsoleClient.Models;
using ECommerceApp.ConsoleClient.Utilities;
using Spectre.Console;

namespace ECommerceApp.ConsoleClient.Handlers;

/// <summary>
/// Handles sales menu operations.
/// Encapsulates all sales-related UI logic following Single Responsibility Principle.
/// Split into smaller methods to reduce cyclomatic complexity.
/// </summary>
public class SalesMenuHandler : IConsoleMenuHandler
{
    public string MenuName => "Sales";

    public async Task ExecuteAsync(HttpClient http)
    {
        var actions = new Dictionary<string, Func<HttpClient, Task>>
        {
            { "List", ListAsync },
            { "Get by Id", h => ApiClient.GetByIdAsync(h, "/api/sales/{id}", "Sale") },
            {
                "With Deleted Products (all)",
                h => ApiClient.GetAndRenderAsync(h, "/api/sales/with-deleted-products")
            },
            {
                "With Deleted Products (by Id)",
                h => ApiClient.GetByIdAsync(h, "/api/sales/{id}/with-deleted-products", "Sale")
            },
            { "Create", CreateAsync },
            { "Update", UpdateAsync },
            { "Delete", DeleteAsync },
        };

        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Sales[/] — Choose an action:")
                    .AddChoices(actions.Keys.Concat(new[] { "Back" }).ToList())
            );

            if (choice == "Back")
                return;

            if (actions.TryGetValue(choice, out var action))
                await action(http);
        }
    }

    private static async Task ListAsync(HttpClient http)
    {
        var pageSize = ConsoleInputHelper.PromptPositiveInt("Items per page");

        // Ask if user wants to apply filters
        var applyFilters = AnsiConsole.Confirm("Apply filters?", false);

        DateTime? start = null;
        DateTime? end = null;
        string? customerName = null;
        string? customerEmail = null;
        string? sortBy = null;
        string? sortDirection = null;

        if (applyFilters)
        {
            start = ConsoleInputHelper.PromptOptionalDate("Start Date (yyyy-MM-dd)");
            end = ConsoleInputHelper.PromptOptionalDate("End Date (yyyy-MM-dd)");
            customerName = ConsoleInputHelper.PromptOptional("Customer Name");
            customerEmail = ConsoleInputHelper.PromptOptional("Customer Email");
            var (sortByResult, sortDirectionResult) = PromptSortOptions(
                new[] { "(none)", "saledate", "totalamount", "customername" }
            );
            sortBy = sortByResult;
            sortDirection = sortDirectionResult;
        }

        var query = new SaleListQuery
        {
            Page = 1,
            PageSize = pageSize,
            StartDate = start,
            EndDate = end,
            CustomerName = customerName,
            CustomerEmail = customerEmail,
            SortBy = sortBy,
            SortDirection = sortDirection,
        };

        await BuildAndExecuteListQueryWithPagination(http, query);
    }

    private static async Task BuildAndExecuteListQueryWithPagination(
        HttpClient http,
        SaleListQuery query
    )
    {
        while (true)
        {
            var qs = BuildSaleQueryString(query);
            var response = await ApiClient.FetchPaginatedAsync<SaleDto>(http, $"/api/sales{qs}");

            if (response?.Data == null || response.Data.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No sales found[/]");
                break;
            }

            var pagination = new PaginationState
            {
                CurrentPage = query.Page,
                PageSize = query.PageSize,
                TotalCount = response.TotalCount,
            };

            TableRenderer.DisplayTable(
                response.Data,
                $"Sales (Page {pagination.CurrentPage}/{pagination.TotalPages}, Total: {pagination.TotalCount})",
                pagination.IndexOffset
            );

            if (!HandlePaginationNavigation(query, pagination))
                break;
        }
    }

    private static string BuildSaleQueryString(SaleListQuery query)
    {
        return new QueryStringBuilder()
            .Add("page", query.Page.ToString())
            .Add("pageSize", query.PageSize.ToString())
            .Add("startDate", query.StartDate?.ToString("yyyy-MM-dd"))
            .Add("endDate", query.EndDate?.ToString("yyyy-MM-dd"))
            .Add("customerName", query.CustomerName)
            .Add("customerEmail", query.CustomerEmail)
            .Add("sortBy", query.SortBy == "(none)" || query.SortBy == null ? null : query.SortBy)
            .Add("sortDirection", query.SortDirection)
            .Build();
    }

    private static bool HandlePaginationNavigation(SaleListQuery query, PaginationState pagination)
    {
        var navChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Navigation:")
                .AddChoices(pagination.GetNavigationChoices())
        );

        switch (navChoice)
        {
            case "Back to Menu":
                return false;
            case "Next Page":
                query.Page++;
                break;
            case "Previous Page":
                query.Page--;
                break;
            case "Jump to Page":
                HandleJumpToPage(query, pagination);
                break;
        }

        return true;
    }

    private static void HandleJumpToPage(SaleListQuery query, PaginationState pagination)
    {
        int targetPage = ConsoleInputHelper.PromptPositiveInt(
            $"Enter page number (1-{pagination.TotalPages})"
        );
        if (targetPage >= 1 && targetPage <= pagination.TotalPages)
            query.Page = targetPage;
        else
            AnsiConsole.MarkupLine(
                $"[red]Invalid page number. Valid range: 1-{pagination.TotalPages}[/]"
            );
    }

    private static async Task CreateAsync(HttpClient http)
    {
        var customerName = ConsoleInputHelper.PromptRequired("Customer Name");
        var customerEmail = ConsoleInputHelper.PromptRequired("Customer Email");
        var customerAddress = ConsoleInputHelper.PromptRequired("Customer Address");
        var saleDate = AnsiConsole.Prompt(
            new TextPrompt<DateTime>("Sale Date (yyyy-MM-dd):").DefaultValue(DateTime.Now)
        );

        var saleItems = await PromptSaleItemsAsync(http);
        if (saleItems.Count == 0)
        {
            ConsoleInputHelper.DisplayError("Must add at least one sale item");
            return;
        }

        var sale = new
        {
            customerName,
            customerEmail,
            customerAddress,
            saleDate,
            saleItems,
        };
        var payload = new { payload = sale };
        await ApiClient.PostAsync(http, "/api/sales", payload);
    }

    private static async Task UpdateAsync(HttpClient http)
    {
        var response = await ApiClient.FetchPaginatedAsync<SaleDto>(
            http,
            "/api/sales?page=1&pageSize=32"
        );
        if (!HasSaleResults(response))
        {
            AnsiConsole.MarkupLine("[yellow]No sales available[/]");
            return;
        }

        var selected = await SelectSaleAsync(http, response!.TotalCount);
        if (selected == null)
            return;

        var current = await FetchSaleDetailsAsync(http, selected.SaleId);
        if (current == null)
        {
            ConsoleInputHelper.DisplayError("Sale not found");
            return;
        }

        DisplayCurrentSaleValues(current);
        var update = PromptSaleUpdate(current);

        await ApiClient.PutAsync(
            http,
            $"/api/sales/{selected.SaleId}",
            BuildSaleUpdatePayload(selected.SaleId, update)
        );
    }

    private static async Task DeleteAsync(HttpClient http)
    {
        var response = await ApiClient.FetchPaginatedAsync<SaleDto>(
            http,
            "/api/sales?page=1&pageSize=32"
        );
        if (response?.Data == null || response.Data.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No sales available[/]");
            return;
        }

        var selected = await TableRenderer.SelectFromPromptAsync(
            async (pageNum) =>
            {
                var pageResponse = await ApiClient.FetchPaginatedAsync<SaleDto>(
                    http,
                    $"/api/sales?page={pageNum}&pageSize=32"
                );
                return pageResponse?.Data ?? new List<SaleDto>();
            },
            response.TotalCount,
            32,
            "Select a Sale to Delete",
            sale => $"{sale.SaleDate:yyyy-MM-dd HH:mm} - {sale.CustomerName}"
        );

        if (selected == null)
            return;

        if (
            !AnsiConsole.Confirm(
                $"[red]Are you sure you want to delete the sale from '{selected.SaleDate:yyyy-MM-dd HH:mm}' for '{selected.CustomerName}'?[/]"
            )
        )
            return;

        await ApiClient.DeleteAsync(http, $"/api/sales/{selected.SaleId}");
    }

    private static async Task<List<object>> PromptSaleItemsAsync(HttpClient http)
    {
        var saleItems = new List<object>();
        AnsiConsole.MarkupLine("[yellow]Enter Sale Items[/]");

        while (AnsiConsole.Confirm("Add a sale item?"))
        {
            var selectedProduct = await SelectProductForSaleAsync(http);
            if (selectedProduct == null)
                continue;

            var quantity = ConsoleInputHelper.PromptPositiveInt("Quantity");
            saleItems.Add(new { productId = selectedProduct.ProductId, quantity });
        }

        return saleItems;
    }

    private static bool HasSaleResults(PaginatedResponse<SaleDto>? response)
    {
        return response?.Data != null && response.Data.Count > 0;
    }

    private static async Task<SaleDto?> SelectSaleAsync(HttpClient http, int totalCount)
    {
        return await TableRenderer.SelectFromPromptAsync(
            async pageNum =>
            {
                var pageResponse = await ApiClient.FetchPaginatedAsync<SaleDto>(
                    http,
                    $"/api/sales?page={pageNum}&pageSize=32"
                );
                return pageResponse?.Data ?? new List<SaleDto>();
            },
            totalCount,
            32,
            "Select a Sale to Update",
            sale => $"{sale.SaleDate:yyyy-MM-dd HH:mm} - {sale.CustomerName}"
        );
    }

    private static async Task<SaleDto?> FetchSaleDetailsAsync(HttpClient http, int saleId)
    {
        var saleResponse = await ApiClient.FetchEntityAsync<SaleDto>(http, $"/api/sales/{saleId}");
        return saleResponse?.Data;
    }

    private static SaleUpdate PromptSaleUpdate(SaleDto current)
    {
        var customerName = PromptOptionalField("Customer Name", current.CustomerName);
        var customerEmail = PromptOptionalField("Email", current.CustomerEmail);
        var saleDateStr = AnsiConsole.Ask<string>(
            "Sale Date (leave blank to keep, format: yyyy-MM-dd):",
            string.Empty
        );

        return new SaleUpdate
        {
            CustomerName = customerName,
            CustomerEmail = customerEmail,
            SaleDate = string.IsNullOrWhiteSpace(saleDateStr)
                ? current.SaleDate
                : DateTime.Parse(saleDateStr),
        };
    }

    private static object BuildSaleUpdatePayload(int saleId, SaleUpdate update)
    {
        return new
        {
            payload = new
            {
                saleId,
                customerName = update.CustomerName,
                customerEmail = update.CustomerEmail,
                saleDate = update.SaleDate,
            },
        };
    }

    private static async Task<ProductDto?> SelectProductForSaleAsync(HttpClient http)
    {
        var productsResponse = await ApiClient.FetchPaginatedAsync<ProductDto>(
            http,
            "/api/product?page=1&pageSize=32"
        );
        if (productsResponse?.Data == null || productsResponse.Data.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No products available[/]");
            return null;
        }

        return await TableRenderer.SelectFromPromptAsync(
            async pageNum =>
            {
                var pageResponse = await ApiClient.FetchPaginatedAsync<ProductDto>(
                    http,
                    $"/api/product?page={pageNum}&pageSize=32"
                );
                return pageResponse?.Data ?? new List<ProductDto>();
            },
            productsResponse.TotalCount,
            32,
            "Select a Product",
            product => product.Name
        );
    }

    private static void DisplayCurrentSaleValues(SaleDto current)
    {
        AnsiConsole.MarkupLine($"[yellow]Current values:[/]");
        AnsiConsole.MarkupLine($"  Customer Name: {current.CustomerName}");
        AnsiConsole.MarkupLine($"  Email: {current.CustomerEmail}");
        AnsiConsole.MarkupLine($"  Sale Date: {current.SaleDate:yyyy-MM-dd}");
        AnsiConsole.MarkupLine($"  Total: {current.TotalAmount:0.00}");
    }

    private static string? PromptOptionalField(string label, string? currentValue)
    {
        var input = AnsiConsole.Ask<string>($"{label} (leave blank to keep):", string.Empty);
        return string.IsNullOrWhiteSpace(input) ? currentValue : input;
    }

    private static (string SortBy, string? SortDirection) PromptSortOptions(string[] options)
    {
        var sortBy = AnsiConsole.Prompt(
            new SelectionPrompt<string>().Title("Sort By (optional)").AddChoices(options)
        );

        string? sortDirection = null;
        if (sortBy != "(none)")
        {
            sortDirection = AnsiConsole.Prompt(
                new SelectionPrompt<string>().Title("Sort Direction").AddChoices("asc", "desc")
            );
        }

        return (sortBy, sortDirection);
    }

    private sealed record SaleUpdate
    {
        public string? CustomerName { get; init; }
        public string? CustomerEmail { get; init; }
        public DateTime SaleDate { get; init; }
    }

    private sealed record SaleDto
    {
        public int SaleId { get; init; }
        public DateTime SaleDate { get; init; }
        public string? CustomerName { get; init; }
        public string? CustomerEmail { get; init; }
        public string? CustomerAddress { get; init; }
        public decimal TotalAmount { get; init; }
    }

    private sealed record ProductDto
    {
        public int ProductId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public bool IsActive { get; init; }
        public int CategoryId { get; init; }
    }

    private sealed record SaleListQuery
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
