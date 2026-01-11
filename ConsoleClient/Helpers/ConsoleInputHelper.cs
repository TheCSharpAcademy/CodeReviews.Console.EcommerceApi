using Spectre.Console;

namespace ECommerceApp.ConsoleClient.Helpers;

/// <summary>
/// Provides common UI and input utilities to eliminate code duplication.
/// Follows DRY principle - centralized prompt and validation logic.
/// </summary>
public static class ConsoleInputHelper
{
    /// <summary>
    /// Prompts for an optional string input.
    /// </summary>
    public static string? PromptOptional(string label)
    {
        var v = AnsiConsole.Ask<string>($"{label} (optional):", string.Empty);
        return string.IsNullOrWhiteSpace(v) ? null : v;
    }

    /// <summary>
    /// Prompts for an optional integer input.
    /// </summary>
    public static int? PromptOptionalInt(string label)
    {
        var v = AnsiConsole.Ask<string>($"{label} (optional):", string.Empty);
        return int.TryParse(v, out var i) ? i : null;
    }

    /// <summary>
    /// Prompts for an optional decimal input.
    /// </summary>
    public static decimal? PromptOptionalDecimal(string label)
    {
        var v = AnsiConsole.Ask<string>($"{label} (optional):", string.Empty);
        return decimal.TryParse(v, out var d) ? d : null;
    }

    /// <summary>
    /// Prompts for an optional date input (yyyy-MM-dd format).
    /// </summary>
    public static DateTime? PromptOptionalDate(string label)
    {
        var v = AnsiConsole.Ask<string>($"{label}", string.Empty);
        return DateTime.TryParse(v, out var dt) ? dt : null;
    }

    /// <summary>
    /// Prompts for a required positive integer.
    /// </summary>
    public static int PromptPositiveInt(string label, string errorMessage = "Must be > 0")
    {
        return AnsiConsole.Prompt(new TextPrompt<int>($"{label}:")
            .Validate(i => i > 0 ? ValidationResult.Success() : ValidationResult.Error(errorMessage)));
    }

    /// <summary>
    /// Prompts for a required positive decimal.
    /// </summary>
    public static decimal PromptPositiveDecimal(string label, string errorMessage = "Must be > 0")
    {
        return AnsiConsole.Prompt(new TextPrompt<decimal>($"{label}:")
            .Validate(d => d > 0 ? ValidationResult.Success() : ValidationResult.Error(errorMessage)));
    }

    /// <summary>
    /// Prompts for a required non-negative integer (≥ 0).
    /// </summary>
    public static int PromptNonNegativeInt(string label, int defaultValue = 0, string errorMessage = "Must be >= 0")
    {
        return AnsiConsole.Prompt(new TextPrompt<int>($"{label}:")
            .DefaultValue(defaultValue)
            .Validate(i => i >= 0 ? ValidationResult.Success() : ValidationResult.Error(errorMessage)));
    }

    /// <summary>
    /// Prompts for a paginated result with standard page and page size inputs.
    /// </summary>
    public static (int Page, int PageSize) PromptPagination()
    {
        int page = AnsiConsole.Prompt(new TextPrompt<int>("Page")
            .DefaultValue(1)
            .Validate(i => i > 0 ? ValidationResult.Success() : ValidationResult.Error("Page must be > 0")));

        int pageSize = AnsiConsole.Prompt(new TextPrompt<int>("Page Size (1-100)")
            .DefaultValue(10)
            .Validate(i => i is >= 1 and <= 100
                ? ValidationResult.Success()
                : ValidationResult.Error("1-100")));

        return (page, pageSize);
    }

    /// <summary>
    /// Prompts for a required string input.
    /// </summary>
    public static string PromptRequired(string label, string errorMessage = "Cannot be empty")
    {
        return AnsiConsole.Prompt(new TextPrompt<string>($"{label}:")
            .Validate(s => !string.IsNullOrWhiteSpace(s)
                ? ValidationResult.Success()
                : ValidationResult.Error(errorMessage)));
    }

    /// <summary>
    /// Displays an error message in red.
    /// </summary>
    public static void DisplayError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{Markup.Escape(message)}[/]");
    }

    /// <summary>
    /// Displays a success message in green.
    /// </summary>
    public static void DisplaySuccess(string message)
    {
        AnsiConsole.MarkupLine($"[green]{Markup.Escape(message)}[/]");
    }

    /// <summary>
    /// Truncates text to maximum length.
    /// </summary>
    public static string Truncate(string s, int max)
    {
        return s.Length > max ? s[..max] + "..." : s;
    }

    /// <summary>
    /// Checks if content type indicates JSON.
    /// </summary>
    public static bool IsJson(string contentType, string body)
    {
        if (!string.IsNullOrWhiteSpace(contentType) &&
            contentType.Contains("json", StringComparison.OrdinalIgnoreCase))
            return true;

        if (string.IsNullOrWhiteSpace(body))
            return false;

        var t = body.TrimStart();
        return t.StartsWith("{") || t.StartsWith("[");
    }
}
