namespace ECommerceApp.ConsoleClient.Utilities;

/// <summary>
/// Encapsulates menu action handling to reduce cyclomatic complexity.
/// Follows Strategy Pattern for extensible menu operations.
/// </summary>
public class MenuActionHandler
{
    private readonly Dictionary<string, Func<HttpClient, Task>> _actions;

    public MenuActionHandler()
    {
        _actions = new();
    }

    public void Register(string action, Func<HttpClient, Task> handler)
    {
        _actions[action] = handler;
    }

    public async Task ExecuteAsync(string action, HttpClient http)
    {
        if (_actions.TryGetValue(action, out var handler))
        {
            await handler(http);
        }
    }

    public IEnumerable<string> GetActions() => _actions.Keys;
}
