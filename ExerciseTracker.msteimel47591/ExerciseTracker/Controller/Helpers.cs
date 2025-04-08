using Microsoft.Data.Sqlite;
using Spectre.Console;
using System.Text.Json;
using ExerciseTracker.Models;

namespace ExerciseTracker.Controller;

internal class Helpers
{
    public static void PrintHeader()
    {
        Console.Clear();
        AnsiConsole.Write(new FigletText("Exercise Tracker").Color(Color.Blue));
    }

    public static void CreateDatabase()
    {
        string _connectionString = GetConnectionString();

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS exercises 
            (id INTEGER PRIMARY KEY, DateStart TEXT, DateEnd TEXT, 
            Duration TEXT, Comment TEXT)";
            command.ExecuteNonQuery();
        }
    }

    public static string GetConnectionString()
    {
        string basePath = AppContext.BaseDirectory;
        string projectPath = Directory.GetParent(basePath).Parent.Parent.Parent.FullName;
        string filePath = Path.Combine(projectPath, "appsettings.json");
        var config = JsonSerializer.Deserialize<Config>(File.ReadAllText(filePath));
        return config.DefaultConnection;
    }

    public static DateTime GetDateTime()
    {
        var dateTime = AnsiConsole.Ask<DateTime>("---");
        return dateTime;
    }

    public static string GetComment()
    {
        var comment = AnsiConsole.Ask<string>("Enter a comment: ");
        return comment;
    }

    public static bool ValidateDates(DateTime start, DateTime end)
    {
        if (start < end)
        {
            return true;
        }
        else
        {
            AnsiConsole.Markup("[red]The start date and time must be before the end date and time.[/]");
            AnsiConsole.Markup("[red]Press any key to try again.[/]\n");
            Console.ReadKey();
            return false;
        }
    }

    public static bool ValidateID(List<Exercise> exercises, int id)
    {
        if (exercises.Any(e => e.Id == id))
        {
            return true;
        }
        else
        {
            AnsiConsole.Markup("[red]Invalid ID.[/]");
            AnsiConsole.Markup("[red]Press any key to continue.[/]");
            Console.ReadKey();
            return false;
        }
    }

    public static void SeedDatabase()
    {
        string _connectionString = GetConnectionString();

        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();

            // Insert sample data
            command.CommandText = @"
            INSERT INTO exercises (DateStart, DateEnd, Duration, Comment) VALUES 
            ('2025-01-01T08:00:00', '2025-01-01T09:00:00', '01:00:00', 'Morning run'),
            ('2025-01-02T18:00:00', '2025-01-02T19:00:00', '01:00:00', 'Evening yoga'),
            ('2025-01-03T12:00:00', '2025-01-03T13:00:00', '01:00:00', 'Lunch break walk'),
            ('2025-01-04T07:00:00', '2025-01-04T08:00:00', '01:00:00', 'Early morning swim'),
            ('2025-01-05T17:00:00', '2025-01-05T18:00:00', '01:00:00', 'Afternoon cycling'),
            ('2025-01-06T06:00:00', '2025-01-06T07:00:00', '01:00:00', 'Morning meditation'),
            ('2025-01-07T19:00:00', '2025-01-07T20:00:00', '01:00:00', 'Evening strength training'),
            ('2025-01-08T13:00:00', '2025-01-08T14:00:00', '01:00:00', 'Midday cardio'),
            ('2025-01-09T09:00:00', '2025-01-09T10:00:00', '01:00:00', 'Morning hike'),
            ('2025-01-10T20:00:00', '2025-01-10T21:00:00', '01:00:00', 'Nighttime stretching')
        ";
            command.ExecuteNonQuery();
        }
    }
}
