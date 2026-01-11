using System;
using System.Net;
using System.Text;
using System.Text.Json;
using ECommerceApp.ConsoleClient.Helpers;
using Spectre.Console;

namespace ECommerceApp.ConsoleClient.Utilities;

/// <summary>
/// Provides API communication utilities.
/// Centralizes HTTP requests, response parsing, and error handling.
/// Follows Single Responsibility Principle.
/// </summary>
public static class ApiClient
{
    /// <summary>
    /// Sends a GET request to the specified path and renders the response.
    /// </summary>
    public static async Task GetAndRenderAsync(HttpClient http, string path)
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, path);
        await SendAndRenderAsync(http, req);
    }

    /// <summary>
    /// Sends an HTTP request and displays the response.
    /// </summary>
    public static async Task SendAndRenderAsync(HttpClient http, HttpRequestMessage req)
    {
        await AnsiConsole
            .Status()
            .Spinner(Spinner.Known.Dots)
            .StartAsync(
                "Calling API...",
                async _ =>
                {
                    using var res = await http.SendAsync(req);
                    var body = await res.Content.ReadAsStringAsync();
                    var header = $"{(int)res.StatusCode} {res.ReasonPhrase}";
                    var ct = res.Content.Headers.ContentType?.MediaType ?? "";

                    AnsiConsole.Write(new Rule($"[bold]HTTP {req.Method}[/] {req.RequestUri}"));
                    AnsiConsole.MarkupLine(
                        $"Status: [bold]{header}[/]  Content-Type: [italic]{ct}[/]"
                    );

                    var truncatedBody = ConsoleInputHelper.Truncate(body, 8000);
                    var escapedBody = Markup.Escape(truncatedBody);

                    if (ConsoleInputHelper.IsJson(ct, body))
                    {
                        try
                        {
                            AnsiConsole.Write(new Panel(escapedBody).Header("Response").Expand());
                        }
                        catch
                        {
                            AnsiConsole.Write(new Panel(escapedBody).Header("Response").Expand());
                        }
                    }
                    else
                    {
                        AnsiConsole.Write(new Panel(escapedBody).Header("Response").Expand());
                    }
                }
            );
    }

    // ...existing code...
    /// <summary>
    /// Fetches a single entity by ID and deserializes it.
    /// </summary>
    public static async Task<ApiResponse<T>?> FetchEntityAsync<T>(HttpClient http, string path)
    {
        using var getReq = new HttpRequestMessage(HttpMethod.Get, path);
        using var getRes = await http.SendAsync(getReq);
        if (!getRes.IsSuccessStatusCode)
        {
            ConsoleInputHelper.DisplayError($"Failed to fetch entity: {getRes.StatusCode}");
            return null;
        }

        var json = await getRes.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<ApiResponse<T>>(json, options);
    }

    /// <summary>
    /// Fetches a paginated list and deserializes it.
    /// </summary>
    public static async Task<PaginatedResponse<T>?> FetchPaginatedAsync<T>(
        HttpClient http,
        string path
    )
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, path);
        using var res = await http.SendAsync(req);
        if (!res.IsSuccessStatusCode)
        {
            ConsoleInputHelper.DisplayError($"Failed to fetch data: {res.StatusCode}");
            return null;
        }

        var json = await res.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<PaginatedResponse<T>>(json, options);
    }

    /// <summary>
    /// Sends a GET request with an ID parameter.
    /// </summary>
    public static async Task GetByIdAsync(HttpClient http, string template, string label)
    {
        int id = ConsoleInputHelper.PromptPositiveInt($"{label} Id");
        var path = template.Replace("{id}", id.ToString());
        await SendAndRenderAsync(http, new HttpRequestMessage(HttpMethod.Get, path));
    }

    /// <summary>
    /// Sends a POST request with JSON body.
    /// </summary>
    public static async Task PostAsync(HttpClient http, string path, object payload)
    {
        var json = JsonSerializer.Serialize(payload);
        using var req = new HttpRequestMessage(HttpMethod.Post, path)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };
        await SendAndRenderAsync(http, req);
    }

    /// <summary>
    /// Sends a PUT request with JSON body.
    /// </summary>
    public static async Task PutAsync(HttpClient http, string path, object payload)
    {
        var json = JsonSerializer.Serialize(payload);
        using var req = new HttpRequestMessage(HttpMethod.Put, path)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
        };
        await SendAndRenderAsync(http, req);
    }

    /// <summary>
    /// Sends a DELETE request.
    /// </summary>
    public static async Task DeleteAsync(HttpClient http, string path)
    {
        using var req = new HttpRequestMessage(HttpMethod.Delete, path);
        await SendAndRenderAsync(http, req);
    }
}

/// <summary>
/// Paginated response wrapper for deserialization in ConsoleClient.
/// </summary>
public sealed record PaginatedResponse<T>
{
    public bool RequestFailed { get; init; }
    public HttpStatusCode ResponseCode { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public List<T>? Data { get; init; }
    public int CurrentPage { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
    public int IndexOffset => (CurrentPage - 1) * PageSize;
}

/// <summary>
/// API response wrapper for deserialization.
/// </summary>
public sealed record ApiResponse<T>
{
    public bool RequestFailed { get; init; }
    public HttpStatusCode ResponseCode { get; init; }
    public string ErrorMessage { get; init; } = string.Empty;
    public T? Data { get; init; }
}
