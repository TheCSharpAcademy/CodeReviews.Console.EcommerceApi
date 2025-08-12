namespace ExerciseTracker.samggannon.Validation;

internal class ConsoleMessages
{
    internal static void DevelopersNote()
    {
        Console.Clear();
        Console.WriteLine("============================================================");
        Console.WriteLine("                        Developer's Note                     ");
        Console.WriteLine("============================================================");
        Console.WriteLine();
        Console.WriteLine("Welcome to the Exercise Tracker Console Application!");
        Console.WriteLine();
        Console.WriteLine("DISCLAIMER:");
        Console.WriteLine("This console application is not intended to be a comprehensive or foolproof exercise tracker/logger.");
        Console.WriteLine("It provides basic functionalities and serves as a lightweight application for tracking exercise sessions.");
        Console.WriteLine();
        Console.WriteLine("Please note that the application is not robust in terms of error handling. Some inputs, such as exercise type,");
        Console.WriteLine("have been hardcoded for simplicity.");
        Console.WriteLine();
        Console.WriteLine("Purpose:");
        Console.WriteLine("The primary focus of this project was to explore data layer technologies. In this application:");
        Console.WriteLine("1. Cardio sessions implement an Entity Framework (EF) Core repository.");
        Console.WriteLine("2. Resistance training sessions implement ADO.NET technology.");
        Console.WriteLine();
        Console.WriteLine("Potential Enhancements:");
        Console.WriteLine("This application could be extended with additional features, such as:");
        Console.WriteLine("- Generating reports that include the number of workouts and total time spent exercising.");
        Console.WriteLine("- Logging specific details for different types of exercises (e.g., run sessions with miles logged, strength training with reps and sets).");
        Console.WriteLine("- Adding support for various types of exercises (e.g., push-ups, curls) with detailed tracking.");
        Console.WriteLine();
        Console.WriteLine("Thank you for using the Exercise Tracker Console Application. Press [enter] to return to the main menu.");
        Console.WriteLine();
        Console.WriteLine("============================================================");
        Console.ReadLine();
    }
}
