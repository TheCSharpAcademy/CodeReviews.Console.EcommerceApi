namespace ECommerceApp.ConsoleClient.Utilities;

/// <summary>
/// Builder for constructing query strings with null-safe parameter handling.
/// </summary>
public class QueryStringBuilder
{
    private readonly Dictionary<string, string> _parameters = new();

    /// <summary>
    /// Adds a parameter to the query string.
    /// Null values are ignored to avoid building unnecessary parameters.
    /// </summary>
    /// <param name="key">The parameter key</param>
    /// <param name="value">The parameter value (null values are ignored)</param>
    /// <returns>The builder instance for method chaining</returns>
    public QueryStringBuilder Add(string key, string? value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            _parameters[key] = Uri.EscapeDataString(value);
        }
        return this;
    }

    /// <summary>
    /// Builds the query string.
    /// </summary>
    /// <returns>The formatted query string (e.g., "?key1=value1&key2=value2"), or empty string if no parameters</returns>
    public string Build()
    {
        if (_parameters.Count == 0)
            return string.Empty;

        var query = string.Join("&", _parameters.Select(p => $"{p.Key}={p.Value}"));
        return $"?{query}";
    }
}
