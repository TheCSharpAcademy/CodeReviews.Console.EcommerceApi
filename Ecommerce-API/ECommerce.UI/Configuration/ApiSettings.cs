using System.Text.Json;

namespace ECommerce.UI.Configuration;

internal class ApiSettings
{
    internal static string BaseUrl { get; } = GetBaseUri();

    //------- Helper Methods -------
    private static string GetBaseUri()
    {
        var solutionRoot = GetSolutionDirectory();
        var launchSettingsPath = Path.Combine(
            solutionRoot,
            "ECommerce.API",
            "Properties",
            "launchSettings.json");

        if (File.Exists(launchSettingsPath))
        {
            var json = File.ReadAllText(launchSettingsPath);
            using var doc = JsonDocument.Parse(json);
            var uri = doc.RootElement
                .GetProperty("profiles")
                .GetProperty("http")
                .GetProperty("applicationUrl")
                .GetString();

            if (string.IsNullOrEmpty(uri))
                throw new Exception("Could not find the application url within the Api's launchSettings.json profile");

            var baseUrl = uri.Split(';').First();
            return $"{baseUrl}/api/";
        }

        throw new Exception("Could not find the Api's launchSettings.json profile");
    }

    private static string GetSolutionDirectory()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory != null)
        {
            if (directory.GetFiles("*.sln").Any())
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not find the solution directory");
    }
}