namespace ECommerceApp.ConsoleClient.Interfaces;

/// <summary>
/// Defines the contract for handling console menu operations.
/// Follows Single Responsibility Principle - each handler manages one domain.
/// </summary>
public interface IConsoleMenuHandler
{
    /// <summary>
    /// Gets the display name for the menu option.
    /// </summary>
    string MenuName { get; }

    /// <summary>
    /// Executes the menu handler asynchronously.
    /// </summary>
    /// <param name="http">The HTTP client for API calls</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task ExecuteAsync(HttpClient http);
}
