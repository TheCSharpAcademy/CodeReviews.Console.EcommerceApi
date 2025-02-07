using Spectre.Console;

internal class ErrorInfoHelpers
{
    internal static void Http(HttpRequestException ex)
    {
        AnsiConsole.MarkupLine($"{ex.Message}");
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }

    internal static void General(Exception ex)
    {
        AnsiConsole.MarkupLine("[red]An unexpected error occurred.[/]");
        AnsiConsole.MarkupLine($"Error details: [yellow]{ex.Message}[/]");
        DisplayInfoHelpers.PressAnyKeyToContinue();
    }
}
