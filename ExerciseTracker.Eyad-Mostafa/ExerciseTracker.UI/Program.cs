
namespace ExerciseTracker.UI;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Welcome to the Exersice Tracker!");
        await ShowMenu();
    }

    static async Task ShowMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Exercise Tracker Menu ---");
            Console.WriteLine("1. Get Exercises");
            Console.WriteLine("2. Add Exercise");
            Console.WriteLine("3. Update Exercise");
            Console.WriteLine("4. Delete Exercise");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();
            Console.Clear();

            switch (choice)
            {
                case "1":
                    await Menu.GetExercises();
                    break;
                case "2":
                    await Menu.AddExercise();
                    break;
                case "3":
                    await Menu.UpdateExercise();
                    break;
                case "4":
                    await Menu.DeleteExercise();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
