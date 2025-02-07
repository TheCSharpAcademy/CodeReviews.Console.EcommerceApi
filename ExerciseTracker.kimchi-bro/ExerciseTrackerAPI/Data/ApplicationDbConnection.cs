using ExerciseTrackerAPI.ErrorHandlers;
using System.Text.RegularExpressions;

namespace ExerciseTrackerAPI.Data;

internal partial class ApplicationDbConnection
{
    public static string? ConnectionString { get; set; }
    public static string? ServerConnection { get; set; }
    public static string? DatabaseName { get; set; }

    internal static void Initialize(WebApplicationBuilder builder)
    {
        ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

        ServerConnection = Server().Replace(ConnectionString, "");

        Match match = FindDbName().Match(ConnectionString);
        if (match.Success)
        {
            DatabaseName = match.Groups[1].Value;
        }
        else
        {
            Console.WriteLine("Failed to retrieve database name from connection string.");
            ErrorHandler.EnvExit();
        }

        if (!DbNameCheck().IsMatch(DatabaseName ?? ""))
        {
            Console.WriteLine("Invalid database name.");
            ErrorHandler.EnvExit();
        }
    }

    [GeneratedRegex(@"Database=([^;]+)")]
    private static partial Regex FindDbName();

    [GeneratedRegex(@"^[a-zA-Z0-9_]+$")]
    private static partial Regex DbNameCheck();

    [GeneratedRegex(@"Database=[^;]+;")]
    private static partial Regex Server();
}
