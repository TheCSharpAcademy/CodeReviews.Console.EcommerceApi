using ConsoleTableExt;
using ExerciseTrackerAPI;
using static ExerciseTrackerAPI.ApiClient;

namespace ExerciseTrackerConsoleApp;

internal class ExerciseInput
{
    static bool deleteExercise;
    static bool getExerciseId;
    public static bool getCustomers;

    internal static async Task ExerciseUI()
    {
        var apiClient = new ApiClient();

        Console.Clear();
        Console.WriteLine("Exercise Options:");
        Console.WriteLine("1. List all Exercises");
        Console.WriteLine("2. Get a exercise by ID");
        Console.WriteLine("3. Delete an existing exercise");
        Console.WriteLine("4. See exercises by customer");
        Console.WriteLine("5. Delete all exercises by customer id");
        Console.WriteLine("6. Go Back");

        while (true)
        {
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    await ListExercises(apiClient);
                    break;

                case "2":
                    getExerciseId = true;
                    await ListExercises(apiClient);
                    await GetExerciseById(apiClient);
                    break;

                case "3":
                    deleteExercise = true;
                    await ListExercises(apiClient);
                    await DeleteExercise(apiClient);
                    break;

                case "4":
                    getCustomers = true;
                    await CustomerInfoInput.ListCustomersAsync(apiClient);
                    await GetExercisesByCustomerId(apiClient);
                    break;

                case "5":
                    getCustomers = true;
                    await CustomerInfoInput.ListCustomersAsync(apiClient);
                    await DeleteExerciseByCustomerId(apiClient);
                    break;

                case "6":
                    await MainMenu.StartUI();
                    break;

                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }

    private static async Task DeleteExerciseByCustomerId(ApiClient apiClient)
    {
        getExerciseId = false;
        Console.WriteLine("Enter the ID of the customer to delete exercises (or 'b' to go back to Menu):");
        string input = Console.ReadLine();

        if (input.Equals("b", StringComparison.OrdinalIgnoreCase))
        {
            Console.Clear();
            await ExerciseUI();
        }

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("Invalid ID, please try again.");
            await DeleteExercise(apiClient);
        }

        bool success = await apiClient.DeleteExercisesByCustomerIdAsync(id);

        if (success)
        {
            Console.WriteLine($"Exercises with customer ID: {id} deleted.");
        }
        else
        {
            Console.WriteLine($"No exercises found for customer with ID: {id}.");
        }

        Console.WriteLine("Hit Enter to continue");
        Console.ReadLine();
        await ExerciseUI();
    }

    private static async Task GetExercisesByCustomerId(ApiClient apiClient)
    {
        getCustomers = false;
        Console.WriteLine("Enter the ID of the customer to retrieve exercises (or 'b' to go back to Menu):");
        string input = Console.ReadLine();

        if (input.Equals("b", StringComparison.OrdinalIgnoreCase))
        {
            Console.Clear();
            await ExerciseUI();
            return;
        }

        if (!int.TryParse(input, out int customerId))
        {
            Console.WriteLine("Invalid ID, please try again.");
            await GetExercisesByCustomerId(apiClient);
            return;
        }

        var exercises = await apiClient.GetExercisesByCustomerIdAsync(customerId);

        if (exercises == null)
        {
            Console.WriteLine("No exercises found for the customer.");
            Console.WriteLine("Hit Enter to continue");
            Console.ReadLine();
            await ExerciseUI();

        }

        ConsoleTableBuilder
            .From(exercises)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine(TableAligntment.Center);

        Console.WriteLine("Hit Enter to continue");
        Console.ReadLine();
        await ExerciseUI();
    }

    private static async Task DeleteExercise(ApiClient apiClient)
    {
        deleteExercise = false;
        Console.WriteLine("Enter the ID of the exercise to delete (or 'b' to go back to Menu):");
        string input = Console.ReadLine();

        if (input.Equals("b", StringComparison.OrdinalIgnoreCase))
        {
            Console.Clear();
            await ExerciseUI();
        }

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("Invalid ID, please try again.");
            await DeleteExercise(apiClient);
        }

        var exercise = await apiClient.GetExerciseByIdAsync(id);

        if (exercise == null)
        {
            Console.WriteLine("No Exercise ID Found, please try again");
            Console.WriteLine("Hit Enter to continue");
            Console.ReadLine();
            await ExerciseUI();
        }

        Console.WriteLine($"Deleting exercise with ID {exercise.Id}...");
        await apiClient.DeleteExerciseAsync(id);
        Console.WriteLine($"Exercise with ID {exercise.Id} deleted.");
        Console.WriteLine("Hit Enter to continue");
        Console.ReadLine();
        await ExerciseUI();

    }

    private static async Task GetExerciseById(ApiClient apiClient)
    {
        getExerciseId = false;
        Console.WriteLine("Enter the ID of the exercise (or 'b' to go back to Menu):");
        string input = Console.ReadLine();

        if (input.Equals("b", StringComparison.OrdinalIgnoreCase))
        {
            Console.Clear();
            await ExerciseUI();
        }

        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("Invalid ID, please try again.");
            Console.WriteLine("Hit Enter to continue");
            Console.ReadLine();
            await GetExerciseById(apiClient);
        }

        var exercise = await apiClient.GetExerciseByIdAsync(id);

        if (exercise == null)
        {
            Console.WriteLine("Exercise not found.");
            Console.WriteLine("Hit Enter to continue");
            Console.ReadLine();
            await GetExerciseById(apiClient);
        }

        Console.WriteLine($"Getting exercise with ID {exercise.Id}...");
        var tableData = new List<Exercise> { exercise };

        ConsoleTableBuilder
            .From(tableData)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine(TableAligntment.Center);

        Console.WriteLine("Hit Enter to continue");
        Console.ReadLine();
        await ExerciseUI();
    }

    private static async Task ListExercises(ApiClient apiClient)
    {
        Console.WriteLine("Listing all exercises...");
        var exercises = await apiClient.GetAllExercisesAsync();

        ConsoleTableBuilder
            .From(exercises)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine(TableAligntment.Center);

        if (!deleteExercise && !getExerciseId)
        {
            Console.WriteLine("Hit Enter to continue");
            Console.ReadLine();
            await ExerciseUI();
        }
    }
}
