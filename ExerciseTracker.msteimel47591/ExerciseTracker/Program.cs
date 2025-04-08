using ExerciseTracker.Controller;
using ExerciseTracker.View;

namespace ExerciseTracker;

internal class Program
{
    static void Main(string[] args)
    {
        Helpers.CreateDatabase();

        Console.WriteLine("Do you want to seed the database? (Enter 1 for yes)");
        string reply = Console.ReadLine();

        if (reply == "1")
        {
            Helpers.SeedDatabase();
        }

        UserInterface.DisplayMainMenu();
    }
}
