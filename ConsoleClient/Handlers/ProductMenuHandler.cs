using ECommerceApp.ConsoleClient.Helpers;
using ECommerceApp.ConsoleClient.Interfaces;
using ECommerceApp.ConsoleClient.Models;
using ECommerceApp.ConsoleClient.Utilities;
using Spectre.Console;

namespace ECommerceApp.ConsoleClient.Handlers;

/// <summary>
/// Handles product menu operations.
/// Encapsulates all product-related UI logic following Single Responsibility Principle.
/// </summary>
public partial class ProductMenuHandler : IConsoleMenuHandler
{
    public string MenuName => "Products";

    public async Task ExecuteAsync(HttpClient http)
    {
        var actions = new Dictionary<string, Func<HttpClient, Task>>
        {
            { "List", ListAsync },
            { "Get by Id", GetByIdAsync },
            { "By Category", GetByCategoryAsync },
            { "Create", CreateAsync },
            { "Update", UpdateAsync },
            { "Delete", DeleteAsync },
        };

        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Products[/] — Choose an action:")
                    .AddChoices(actions.Keys.Concat(new[] { "Back" }).ToList())
            );

            if (choice == "Back")
                return;

            if (actions.TryGetValue(choice, out var action))
                await action(http);
        }
    }

    private static async Task GetByIdAsync(HttpClient http)
    {
        // First, show a list for the user to select from
        var response = await ApiClient.FetchPaginatedAsync<ProductDto>(
            http,
            "/api/product?page=1&pageSize=32"
        );
        if (response?.Data == null || response.Data.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No products available[/]");
            return;
        }

        var selected = await TableRenderer.SelectFromPromptAsync(
            async (pageNum) =>
            {
                var pageResponse = await ApiClient.FetchPaginatedAsync<ProductDto>(
                    http,
                    $"/api/product?page={pageNum}&pageSize=32"
                );
                return pageResponse?.Data ?? new List<ProductDto>();
            },
            response.TotalCount,
            32,
            "Select a Product",
            product => product.Name
        );

        if (selected != null)
        {
            // Display the selected product directly from the list
            TableRenderer.DisplayTable(
                new[] { selected }.ToList(),
                "Product Details",
                0,
                "CategoryId"
            );
        }
    }

    private static async Task GetByCategoryAsync(HttpClient http)
    {
        int categoryId = ConsoleInputHelper.PromptPositiveInt("Category ID");
        var qs = new QueryStringBuilder().Add("page", "1").Add("pageSize", "50").Build();

        var response = await ApiClient.FetchPaginatedAsync<ProductDto>(
            http,
            $"/api/product/category/{categoryId}{qs}"
        );
        if (response?.Data != null && response.Data.Count > 0)
        {
            TableRenderer.DisplayTable(
                response.Data,
                $"Products in Category {categoryId}",
                0,
                "CategoryId"
            );
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]No products found in this category[/]");
        }
    }

    private static async Task ListAsync(HttpClient http)
    {
        var pageSize = ConsoleInputHelper.PromptPositiveInt("Items per page");

        var filters = AnsiConsole.Confirm("Apply filters?", false)
            ? PromptProductFilters()
            : ProductFilters.Empty;

        var query = new ProductListQuery
        {
            Page = 1,
            PageSize = pageSize,
            Search = filters.Search,
            MinPrice = filters.MinPrice,
            MaxPrice = filters.MaxPrice,
            CategoryId = filters.CategoryId,
            SortBy = filters.SortBy,
            SortDirection = filters.SortDirection,
        };

        await BuildAndExecuteListQueryWithPagination(http, query);
    }

    private static async Task BuildAndExecuteListQueryWithPagination(
        HttpClient http,
        ProductListQuery query
    )
    {
        while (true)
        {
            var response = await FetchProductPageAsync(http, query);
            if (!HasProductResults(response))
            {
                AnsiConsole.MarkupLine("[yellow]No products found[/]");
                break;
            }

            var pagination = new PaginationState
            {
                CurrentPage = query.Page,
                PageSize = query.PageSize,
                TotalCount = response!.TotalCount,
            };

            TableRenderer.DisplayTable(
                response.Data!,
                $"Products (Page {pagination.CurrentPage}/{pagination.TotalPages}, Total: {pagination.TotalCount})",
                pagination.IndexOffset,
                "CategoryId"
            );

            if (!HandlePaginationNavigation(pagination, query))
                break;
        }
    }

    private static bool HandlePaginationNavigation(
        PaginationState pagination,
        ProductListQuery query
    )
    {
        var navChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Navigation:")
                .AddChoices(pagination.GetNavigationChoices())
        );

        if (navChoice == "Back to Menu")
            return false;

        if (navChoice == "Next Page")
        {
            query.Page++;
        }
        else if (navChoice == "Previous Page")
        {
            query.Page--;
        }
        else if (navChoice == "Jump to Page")
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

        return true;
    }

    private static async Task CreateAsync(HttpClient http)
    {
        // Show list of categories to select from
        var categoriesResponse = await ApiClient.FetchPaginatedAsync<CategoryDto>(
            http,
            "/api/categories?page=1&pageSize=32"
        );
        if (categoriesResponse?.Data == null || categoriesResponse.Data.Count == 0)
        {
            AnsiConsole.MarkupLine(
                "[yellow]No categories available. Please create a category first.[/]"
            );
            return;
        }

        var selectedCategory = await TableRenderer.SelectFromPromptAsync(
            async (pageNum) =>
            {
                var pageResponse = await ApiClient.FetchPaginatedAsync<CategoryDto>(
                    http,
                    $"/api/categories?page={pageNum}&pageSize=32"
                );
                return pageResponse?.Data ?? new List<CategoryDto>();
            },
            categoriesResponse.TotalCount,
            32,
            "Select a Category",
            cat => cat.Name
        );

        if (selectedCategory == null)
            return;

        var name = ConsoleInputHelper.PromptRequired("Product Name");
        var description = ConsoleInputHelper.PromptRequired("Description");
        var price = ConsoleInputHelper.PromptPositiveDecimal("Price");
        var stock = ConsoleInputHelper.PromptNonNegativeInt("Stock");

        var product = new
        {
            name,
            description,
            price,
            stock,
            categoryId = selectedCategory.CategoryId,
            isActive = true,
        };

        var payload = new { payload = product };
        await ApiClient.PostAsync(http, "/api/product", payload);
    }

    private static async Task UpdateAsync(HttpClient http)
    {
        var response = await ApiClient.FetchPaginatedAsync<ProductDto>(
            http,
            "/api/product?page=1&pageSize=32"
        );
        if (!HasProductResults(response))
        {
            AnsiConsole.MarkupLine("[yellow]No products available[/]");
            return;
        }

        var selected = await SelectProductAsync(http, response!.TotalCount);
        if (selected == null)
            return;

        var current = await FetchProductDetailsAsync(http, selected.ProductId);
        if (current == null)
        {
            ConsoleInputHelper.DisplayError("Product not found");
            return;
        }

        DisplayCurrentValues(current);

        var categoryId = await DetermineCategoryAsync(http, current.CategoryId);
        var update = PromptProductUpdates(current, categoryId);

        await ApiClient.PutAsync(
            http,
            $"/api/product/{selected.ProductId}",
            BuildProductUpdatePayload(selected.ProductId, update)
        );
    }

    private static ProductFilters PromptProductFilters()
    {
        var search = ConsoleInputHelper.PromptOptional("Search");
        var minPrice = ConsoleInputHelper.PromptOptionalDecimal("Min Price");
        var maxPrice = ConsoleInputHelper.PromptOptionalDecimal("Max Price");
        var categoryId = ConsoleInputHelper.PromptOptionalInt("Category Id");
        var (sortByResult, sortDirectionResult) = PromptSortOptions(
            new[] { "(none)", "name", "price", "stock", "createdat", "category" }
        );

        return new ProductFilters
        {
            Search = search,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            CategoryId = categoryId,
            SortBy = sortByResult,
            SortDirection = sortDirectionResult,
        };
    }

    private static async Task<PaginatedResponse<ProductDto>?> FetchProductPageAsync(
        HttpClient http,
        ProductListQuery query
    )
    {
        var qs = new QueryStringBuilder()
            .Add("page", query.Page.ToString())
            .Add("pageSize", query.PageSize.ToString())
            .Add("search", query.Search)
            .Add("minPrice", query.MinPrice?.ToString())
            .Add("maxPrice", query.MaxPrice?.ToString())
            .Add("categoryId", query.CategoryId?.ToString())
            .Add("sortBy", query.SortBy == "(none)" || query.SortBy == null ? null : query.SortBy)
            .Add("sortDirection", query.SortDirection)
            .Build();

        return await ApiClient.FetchPaginatedAsync<ProductDto>(http, $"/api/product{qs}");
    }

    private static bool HasProductResults(PaginatedResponse<ProductDto>? response)
    {
        return response?.Data != null && response.Data.Count > 0;
    }

    private static async Task<ProductDto?> SelectProductAsync(HttpClient http, int totalCount)
    {
        return await TableRenderer.SelectFromPromptAsync(
            async pageNum =>
            {
                var pageResponse = await ApiClient.FetchPaginatedAsync<ProductDto>(
                    http,
                    $"/api/product?page={pageNum}&pageSize=32"
                );
                return pageResponse?.Data ?? new List<ProductDto>();
            },
            totalCount,
            32,
            "Select a Product to Update",
            product => product.Name
        );
    }

    private static async Task<ProductDto?> FetchProductDetailsAsync(HttpClient http, int productId)
    {
        var productResponse = await ApiClient.FetchEntityAsync<ProductDto>(
            http,
            $"/api/product/{productId}"
        );
        return productResponse?.Data;
    }

    private static async Task<int> DetermineCategoryAsync(HttpClient http, int currentCategoryId)
    {
        var categoriesResponse = await ApiClient.FetchPaginatedAsync<CategoryDto>(
            http,
            "/api/categories?page=1&pageSize=32"
        );
        if (!HasCategoryResults(categoriesResponse))
            return currentCategoryId;

        if (!AnsiConsole.Confirm("Change category?", false))
            return currentCategoryId;

        var selectedCategory = await PromptCategorySelectionAsync(
            http,
            categoriesResponse!.TotalCount
        );
        return selectedCategory?.CategoryId ?? currentCategoryId;
    }

    private static async Task<CategoryDto?> PromptCategorySelectionAsync(
        HttpClient http,
        int totalCount
    )
    {
        return await TableRenderer.SelectFromPromptAsync(
            pageNum => FetchCategoryPageAsync(http, pageNum),
            totalCount,
            32,
            "Select a Category",
            cat => cat.Name
        );
    }

    private static async Task<List<CategoryDto>> FetchCategoryPageAsync(
        HttpClient http,
        int pageNum
    )
    {
        var pageResponse = await ApiClient.FetchPaginatedAsync<CategoryDto>(
            http,
            $"/api/categories?page={pageNum}&pageSize=32"
        );
        return pageResponse?.Data ?? new List<CategoryDto>();
    }

    private static bool HasCategoryResults(PaginatedResponse<CategoryDto>? response)
    {
        return response?.Data != null && response.Data.Count > 0;
    }

    private static ProductUpdate PromptProductUpdates(ProductDto current, int categoryId)
    {
        var name = PromptOptionalField("Product Name", current.Name);
        var description = PromptOptionalField("Description", current.Description);
        var priceStr = AnsiConsole.Ask("Price (leave blank to keep):", string.Empty);
        var stockStr = AnsiConsole.Ask("Stock (leave blank to keep):", string.Empty);
        var isActiveStr = AnsiConsole.Ask<string>(
            "Active? (yes/no, leave blank to keep):",
            string.Empty
        );

        return new ProductUpdate
        {
            Name = name,
            Description = description,
            Price = TryParseDecimal(priceStr, current.Price),
            Stock = TryParseInt(stockStr, current.Stock),
            CategoryId = categoryId,
            IsActive = TryParseBool(isActiveStr, current.IsActive),
        };
    }

    private static object BuildProductUpdatePayload(int productId, ProductUpdate update)
    {
        return new
        {
            payload = new
            {
                productId,
                name = update.Name,
                description = update.Description,
                price = update.Price,
                stock = update.Stock,
                categoryId = update.CategoryId,
                isActive = update.IsActive,
            },
        };
    }

    private static decimal TryParseDecimal(string value, decimal fallback)
    {
        return decimal.TryParse(value, out var parsed) ? parsed : fallback;
    }

    private static int TryParseInt(string value, int fallback)
    {
        return int.TryParse(value, out var parsed) ? parsed : fallback;
    }

    private static bool TryParseBool(string value, bool fallback)
    {
        if (string.IsNullOrWhiteSpace(value))
            return fallback;

        return value.Equals("yes", StringComparison.OrdinalIgnoreCase)
            || value.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    private static async Task DeleteAsync(HttpClient http)
    {
        // Show list for selection
        var qs = new QueryStringBuilder().Add("page", "1").Add("pageSize", "32").Build();

        var response = await ApiClient.FetchPaginatedAsync<ProductDto>(http, $"/api/product{qs}");
        if (response?.Data == null || response.Data.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No products available[/]");
            return;
        }

        var selected = await TableRenderer.SelectFromPromptAsync(
            async (pageNum) =>
            {
                var pageResponse = await ApiClient.FetchPaginatedAsync<ProductDto>(
                    http,
                    $"/api/product?page={pageNum}&pageSize=32"
                );
                return pageResponse?.Data ?? new List<ProductDto>();
            },
            response.TotalCount,
            32,
            "Select a Product to Delete",
            product => product.Name
        );
        if (selected == null)
            return;

        if (!AnsiConsole.Confirm($"[red]Are you sure you want to delete '{selected.Name}'?[/]"))
            return;
        await ApiClient.DeleteAsync(http, $"/api/product/{selected.ProductId}");
    }

    private static void DisplayCurrentValues(ProductDto current)
    {
        AnsiConsole.MarkupLine($"[yellow]Current values:[/]");
        AnsiConsole.MarkupLine($"  Name: {current.Name}");
        AnsiConsole.MarkupLine($"  Description: {current.Description}");
        AnsiConsole.MarkupLine($"  Price: {current.Price:0.00}");
        AnsiConsole.MarkupLine($"  Stock: {current.Stock}");
        AnsiConsole.MarkupLine($"  Active: {current.IsActive}");
        AnsiConsole.MarkupLine($"  Category ID: {current.CategoryId}");
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
}
