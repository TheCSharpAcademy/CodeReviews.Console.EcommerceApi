using ConsoleTableExt;
using ExerciseTrackerAPI;

namespace ExerciseTrackerConsoleApp;

internal class UserSignedInInput
{
    internal static async Task UserMenu(int customerId)
    {
        var apiClient = new ApiClient();

        Console.Clear();
        Console.WriteLine("User Options:");
        Console.WriteLine("1. New Exercise");
        Console.WriteLine("2. See Past Exercises");
        Console.WriteLine("3. Log Out");

        while (true)
        {
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    await NewExercise(apiClient, customerId);
                    break;

                case "2":
                    await SeePastExercises(apiClient, customerId);
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

    private static async Task SeePastExercises(ApiClient apiClient, int customerId)
    {
        var exercises = await apiClient.GetExercisesByCustomerIdAsync(customerId);

        ConsoleTableBuilder
           .From(exercises)
           .WithFormat(ConsoleTableBuilderFormat.Alternative)
           .ExportAndWriteLine(TableAligntment.Center);

        Console.WriteLine("Hit Enter to continue");
        Console.ReadLine();
        Console.Clear();
        await UserMenu(customerId);
    }

    private static async Task NewExercise(ApiClient apiClient, int customerId)
    {
        var customer = await apiClient.GetCustomerByIdAsync(customerId);

        string name = $"{customer.FirstName} {customer.LastName}";
        Console.Write("Hit Enter To start exercise time");
        Console.ReadLine();
        DateTime dateStart = DateTime.Now;

        Console.Write("Hit Enter to end exercise time");
        Console.ReadLine();
        DateTime dateEnd = DateTime.Now;

        int repetitions;

        while (true)
        {
            Console.Write("Enter the number of repetitions: ");
            if (!int.TryParse(Console.ReadLine(), out repetitions))
            {
                Console.WriteLine("Invalid number format. Please try again.");
            }
            else
            {
                break;
            }
        }

        Console.Write("Enter the comments: ");
        string comments = Console.ReadLine();
        await apiClient.AddExerciseAsync(name, dateStart, dateEnd, repetitions, customerId, comments);
        Console.WriteLine("Hit Enter to continue");
        Console.ReadLine();
        await UserMenu(customerId);
    }
}
