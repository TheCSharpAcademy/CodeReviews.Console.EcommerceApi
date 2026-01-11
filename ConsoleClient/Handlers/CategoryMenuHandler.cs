using ECommerceApp.ConsoleClient.Helpers;
using ECommerceApp.ConsoleClient.Interfaces;
using ECommerceApp.ConsoleClient.Models;
using ECommerceApp.ConsoleClient.Utilities;
using Spectre.Console;

namespace ECommerceApp.ConsoleClient.Handlers;

/// <summary>
/// Handles category menu operations.
/// Encapsulates all category-related UI logic following Single Responsibility Principle.
/// </summary>
public class CategoryMenuHandler : IConsoleMenuHandler
{
    public string MenuName => "Categories";

    public async Task ExecuteAsync(HttpClient http)
    {
        var actions = new Dictionary<string, Func<HttpClient, Task>>
        {
            { "List", ListAsync },
            { "Get by Id", GetByIdAsync },
            { "Get by Name", GetByNameAsync },
            { "Create", CreateAsync },
            { "Update", UpdateAsync },
            { "Delete", DeleteAsync },
        };

        while (true)
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Categories[/] — Choose an action:")
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

        string? search = null;
        string? sortBy = null;
        string? sortDirection = null;

        if (applyFilters)
        {
            search = ConsoleInputHelper.PromptOptional("Search");
            var (sortByResult, sortDirectionResult) = PromptSortOptions(
                new[] { "(none)", "name", "createdat" }
            );
            sortBy = sortByResult;
            sortDirection = sortDirectionResult;
        }

        var query = new CategoryListQuery
        {
            Page = 1,
            PageSize = pageSize,
            Search = search,
            SortBy = sortBy,
            SortDirection = sortDirection,
        };

        await BuildAndExecuteListQueryWithPagination(http, query);
    }

    private static async Task BuildAndExecuteListQueryWithPagination(
        HttpClient http,
        CategoryListQuery query
    )
    {
        while (true)
        {
            var response = await FetchCategoryPageAsync(http, query);
            if (!HasCategoryResults(response))
            {
                AnsiConsole.MarkupLine("[yellow]No categories found[/]");
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
                $"Categories (Page {pagination.CurrentPage}/{pagination.TotalPages}, Total: {pagination.TotalCount})",
                pagination.IndexOffset
            );

            if (!HandlePaginationNavigation(query, pagination))
                break;
        }
    }

    private static async Task<PaginatedResponse<CategoryDto>?> FetchCategoryPageAsync(
        HttpClient http,
        CategoryListQuery query
    )
    {
        var qs = new QueryStringBuilder()
            .Add("page", query.Page.ToString())
            .Add("pageSize", query.PageSize.ToString())
            .Add("search", query.Search)
            .Add("sortBy", query.SortBy == "(none)" || query.SortBy == null ? null : query.SortBy)
            .Add("sortDirection", query.SortDirection)
            .Build();

        return await ApiClient.FetchPaginatedAsync<CategoryDto>(http, $"/api/categories{qs}");
    }

    private static bool HandlePaginationNavigation(
        CategoryListQuery query,
        PaginationState pagination
    )
    {
        var navChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Navigation:")
                .AddChoices(pagination.GetNavigationChoices())
        );

        return navChoice switch
        {
            "Back to Menu" => false,
            "Next Page" => IncrementPage(query),
            "Previous Page" => DecrementPage(query),
            "Jump to Page" => JumpToPage(query, pagination),
            _ => true,
        };
    }

    private static bool JumpToPage(CategoryListQuery query, PaginationState pagination)
    {
        int targetPage = ConsoleInputHelper.PromptPositiveInt(
            $"Enter page number (1-{pagination.TotalPages})"
        );
        if (targetPage >= 1 && targetPage <= pagination.TotalPages)
        {
            query.Page = targetPage;
            return true;
        }

        AnsiConsole.MarkupLine(
            $"[red]Invalid page number. Valid range: 1-{pagination.TotalPages}[/]"
        );
        return true;
    }

    private static bool IncrementPage(CategoryListQuery query)
    {
        query.Page++;
        return true;
    }

    private static bool DecrementPage(CategoryListQuery query)
    {
        query.Page--;
        return true;
    }

    private static bool HasCategoryResults(PaginatedResponse<CategoryDto>? response)
    {
        return response?.Data != null && response.Data.Count > 0;
    }

    private static async Task GetByIdAsync(HttpClient http)
    {
        var response = await ApiClient.FetchPaginatedAsync<CategoryDto>(
            http,
            "/api/categories?page=1&pageSize=32"
        );
        if (response?.Data == null || response.Data.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No categories available[/]");
            return;
        }

        var selected = await TableRenderer.SelectFromPromptAsync(
            async (pageNum) =>
            {
                var pageResponse = await ApiClient.FetchPaginatedAsync<CategoryDto>(
                    http,
                    $"/api/categories?page={pageNum}&pageSize=32"
                );
                return pageResponse?.Data ?? new List<CategoryDto>();
            },
            response.TotalCount,
            32,
            "Select a Category",
            cat => cat.Name
        );

        if (selected != null)
        {
            TableRenderer.DisplayTable(new[] { selected }.ToList(), "Category Details");
        }
    }

    private static async Task GetByNameAsync(HttpClient http)
    {
        var name = ConsoleInputHelper.PromptRequired("Name");
        var response = await ApiClient.FetchEntityAsync<CategoryDto>(
            http,
            $"/api/categories/name/{Uri.EscapeDataString(name)}"
        );
        if (response?.Data != null)
        {
            TableRenderer.DisplayTable(new[] { response.Data }.ToList(), "Category Details");
        }
        else
        {
            AnsiConsole.MarkupLine("[yellow]Category not found[/]");
        }
    }

    private static async Task CreateAsync(HttpClient http)
    {
        var name = ConsoleInputHelper.PromptRequired("Category Name");
        var description = ConsoleInputHelper.PromptRequired("Description");

        var category = new { name, description };
        var payload = new { payload = category };
        await ApiClient.PostAsync(http, "/api/categories", payload);
    }

    private static async Task UpdateAsync(HttpClient http)
    {
        var response = await ApiClient.FetchPaginatedAsync<CategoryDto>(
            http,
            "/api/categories?page=1&pageSize=32"
        );
        if (!HasCategoryResults(response))
        {
            AnsiConsole.MarkupLine("[yellow]No categories available[/]");
            return;
        }

        var selected = await SelectCategoryAsync(http, response!.TotalCount);
        if (selected == null)
            return;

        var current = await FetchCategoryDetailsAsync(http, selected.CategoryId);
        if (current == null)
        {
            ConsoleInputHelper.DisplayError("Category not found");
            return;
        }

        DisplayCurrentCategory(current);
        var (name, description) = PromptCategoryUpdates(current);
        await ApiClient.PutAsync(
            http,
            $"/api/categories/{selected.CategoryId}",
            BuildCategoryUpdatePayload(selected.CategoryId, name, description)
        );
    }

    private static async Task<CategoryDto?> FetchCategoryDetailsAsync(
        HttpClient http,
        int categoryId
    )
    {
        var categoryResponse = await ApiClient.FetchEntityAsync<CategoryDto>(
            http,
            $"/api/categories/{categoryId}"
        );
        return categoryResponse?.Data;
    }

    private static async Task<CategoryDto?> SelectCategoryAsync(HttpClient http, int totalCount)
    {
        return await TableRenderer.SelectFromPromptAsync(
            async pageNum =>
            {
                var pageResponse = await ApiClient.FetchPaginatedAsync<CategoryDto>(
                    http,
                    $"/api/categories?page={pageNum}&pageSize=32"
                );
                return pageResponse?.Data ?? new List<CategoryDto>();
            },
            totalCount,
            32,
            "Select a Category to Update",
            cat => cat.Name
        );
    }

    private static void DisplayCurrentCategory(CategoryDto current)
    {
        AnsiConsole.MarkupLine("[yellow]Current values:[/]");
        AnsiConsole.MarkupLine($"  Name: {current.Name}");
        AnsiConsole.MarkupLine($"  Description: {current.Description}");
    }

    private static (string? Name, string? Description) PromptCategoryUpdates(CategoryDto current)
    {
        var name = PromptOptionalField("Category Name", current.Name);
        var description = PromptOptionalField("Description", current.Description);
        return (name, description);
    }

    private static object BuildCategoryUpdatePayload(
        int categoryId,
        string? name,
        string? description
    )
    {
        return new
        {
            payload = new
            {
                categoryId,
                name,
                description,
            },
        };
    }

    private static async Task DeleteAsync(HttpClient http)
    {
        var response = await ApiClient.FetchPaginatedAsync<CategoryDto>(
            http,
            "/api/categories?page=1&pageSize=32"
        );
        if (response?.Data == null || response.Data.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No categories available[/]");
            return;
        }

        var selected = await TableRenderer.SelectFromPromptAsync(
            async (pageNum) =>
            {
                var pageResponse = await ApiClient.FetchPaginatedAsync<CategoryDto>(
                    http,
                    $"/api/categories?page={pageNum}&pageSize=32"
                );
                return pageResponse?.Data ?? new List<CategoryDto>();
            },
            response.TotalCount,
            32,
            "Select a Category to Delete",
            cat => cat.Name
        );

        if (selected == null)
            return;

        if (!AnsiConsole.Confirm($"[red]Are you sure you want to delete '{selected.Name}'?[/]"))
            return;

        await ApiClient.DeleteAsync(http, $"/api/categories/{selected.CategoryId}");
    }

    private static string? PromptOptionalField(string label, string? currentValue)
    {
        var input = AnsiConsole.Ask($"{label} (leave blank to keep):", string.Empty);
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

    private sealed record CategoryDto
    {
        public int CategoryId { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
