namespace ExerciseTrackerConsoleApp;

internal class MainMenu
{
    internal static async Task StartUI()
    {
        Console.Clear();
        Console.WriteLine("Main Options:");
        Console.WriteLine("1. Customer Info Menu");
        Console.WriteLine("2. Exercise Info Menu");
        Console.WriteLine("3. User Menu");

        var option = Console.ReadLine();

        switch (option)
        {
            case "1":
                await CustomerInfoInput.CustomerUI();
                break;

            case "2":
                await ExerciseInput.ExerciseUI();
                break;
            case "3":
                await UserInput.UserUI();
                break;

            default:
                Console.WriteLine("Invalid option, please try again.");
                break;
        }
    }
}
