using Microsoft.Data.SqlClient;

namespace ExerciseTrackerAPI.ErrorHandlers;

internal class ErrorHandler
{
    internal static void SqlError(SqlException ex)
    {
        Console.WriteLine("A database error occurred.");
        Console.WriteLine($"Error number: {ex.Number}");
        Console.WriteLine($"Error state: {ex.State}");
        Console.WriteLine($"Details: {ex.Message}");

        PressAnyKeyToContinue();
    }

    internal static void GeneralError(Exception ex)
    {
        Console.WriteLine("An error occurred.");
        Console.WriteLine($"Details: {ex.Message}");

        PressAnyKeyToContinue();
    }

    private static void PressAnyKeyToContinue()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
        Console.Clear();
    }

    internal static void EnvExit()
    {
        Console.WriteLine("\nPress any key to exit the app.");
        Console.ReadKey(true);
        Environment.Exit(0);
    }
}
