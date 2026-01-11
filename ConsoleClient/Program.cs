using System.Net;
using System.Text;
using System.Text.Json;
using ECommerceApp.ConsoleClient;
using ECommerceApp.ConsoleClient.Handlers;
using ECommerceApp.ConsoleClient.Helpers;
using ECommerceApp.ConsoleClient.Interfaces;
using ECommerceApp.ConsoleClient.Utilities;
using Spectre.Console;

namespace ECommerceApp.ConsoleClient;

/// <summary>
/// Main console client entry point.
/// Orchestrates menu navigation and delegates domain-specific logic to handlers.
/// Follows Dependency Inversion Principle using IConsoleMenuHandler abstraction.
/// Refactored to reduce cyclomatic complexity and eliminate code duplication.
/// </summary>
public static class Program
{
    private const string DefaultBaseUrl = "http://localhost:51680";

    public static async Task<int> Main(string[] args)
    {
        AnsiConsole.Write(new FigletText("ECommerce API").Color(Color.Green));

        var settings = new ClientSettings { BaseUrl = ResolveBaseUrl(args) };
        var http = CreateHttpClient(settings.BaseUrl);

        try
        {
            // Initialize handlers following Dependency Injection pattern
            var handlers = new Dictionary<string, IConsoleMenuHandler>
            {
                { "Products", new ProductMenuHandler() },
                { "Categories", new CategoryMenuHandler() },
                { "Sales", new SalesMenuHandler() }
            };

            while (true)
            {
                var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title($"[yellow]Base URL:[/] [blue]{settings.BaseUrl}[/]\nSelect an option:")
                    .PageSize(10)
                    .AddChoices(handlers.Keys.Concat(new[] { "Custom Request", "Settings", "Exit" }).ToList()));

                if (handlers.TryGetValue(choice, out var handler))
                {
                    await handler.ExecuteAsync(http);
                }
                else
                {
                    switch (choice)
                    {
                        case "Custom Request":
                            await CustomRequestAsync(http);
                            break;
                        case "Settings":
                            var newUrl = PromptBaseUrl(settings.BaseUrl);
                            if (newUrl != settings.BaseUrl)
                            {
                                settings.BaseUrl = newUrl;
                                http.Dispose();
                                http = CreateHttpClient(settings.BaseUrl);
                            }
                            break;
                        case "Exit":
                            return 0;
                    }
                }
            }
        }
        finally
        {
            http?.Dispose();
        }
    }

    private static string ResolveBaseUrl(string[] args)
    {
        // Priority: CLI arg --base-url, env ECOMMERCE_BASE_URL, default
        var fromArg = GetArgValue(args, "--base-url");
        if (!string.IsNullOrWhiteSpace(fromArg))
            return fromArg!;

        var env = Environment.GetEnvironmentVariable("ECOMMERCE_BASE_URL");
        if (!string.IsNullOrWhiteSpace(env))
            return env!;

        return DefaultBaseUrl;
    }

    private static string? GetArgValue(string[] args, string key)
    {
        for (int i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], key, StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 < args.Length)
                    return args[i + 1];
            }
            else if (args[i].StartsWith(key + "=", StringComparison.OrdinalIgnoreCase))
            {
                return args[i][(key.Length + 1)..];
            }
        }
        return null;
    }

    private static HttpClient CreateHttpClient(string baseUrl)
    {
        // For development: ignore SSL certificate validation errors for self-signed certificates
        var handler = new HttpClientHandler();

        // Always bypass certificate validation for localhost and 127.0.0.1 in development
        if (baseUrl.Contains("localhost", StringComparison.OrdinalIgnoreCase) ||
            baseUrl.Contains("127.0.0.1", StringComparison.OrdinalIgnoreCase))
        {
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        }

        var http = new HttpClient(handler, disposeHandler: true)
        {
            BaseAddress = new Uri(baseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
        http.DefaultRequestHeaders.Accept.Clear();
        http.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        return http;
    }

    private static string PromptBaseUrl(string current)
    {
        var input = AnsiConsole.Prompt(new TextPrompt<string>("Enter API base URL:")
            .DefaultValue(current)
            .Validate(url => Uri.TryCreate(url, UriKind.Absolute, out _)
                ? ValidationResult.Success()
                : ValidationResult.Error("Invalid URL")));
        return input;
    }

    private static async Task CustomRequestAsync(HttpClient http)
    {
        var method = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("HTTP Method")
            .AddChoices("GET", "POST", "PUT", "DELETE"));

        var path = ConsoleInputHelper.PromptRequired("Path (e.g., /api/product)");

        string? body = null;
        if (method is "POST" or "PUT")
        {
            body = AnsiConsole.Ask<string>("JSON body (leave empty for none):", "");
        }

        try
        {
            using var req = new HttpRequestMessage(new HttpMethod(method), path);
            if (!string.IsNullOrWhiteSpace(body))
            {
                req.Content = new StringContent(body!, Encoding.UTF8, "application/json");
            }
            await ApiClient.SendAndRenderAsync(http, req);
        }
        catch (Exception ex)
        {
            ConsoleInputHelper.DisplayError(ex.Message);
        }
    }

    private sealed record ClientSettings
    {
        public string BaseUrl { get; set; } = DefaultBaseUrl;
    }
}
