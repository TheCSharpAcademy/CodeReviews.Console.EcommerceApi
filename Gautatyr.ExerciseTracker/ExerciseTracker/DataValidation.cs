using ExerciseTracker.Controllers;

namespace ExerciseTracker;

public static class DataValidation
{
    public static DateTime GetDateTimeInput()
    {
        var input = Console.ReadLine();

        while (!DateTime.TryParse(input, out _))
        {
            Console.WriteLine("\nError: Invalid input, try again\n");
            input = Console.ReadLine();
        }

        return DateTime.Parse(input);
    }

    public static double GetNumberInput()
    {
        var input = Console.ReadLine();

        while (!double.TryParse(input, out _))
        {
            Console.WriteLine("\nError: Invalid input, try again\n");
            input = Console.ReadLine();
        }

        return double.Parse(input);
    }

    public static int GetIntInput()
    {
        var input = Console.ReadLine();

        while (!int.TryParse(input, out _))
        {
            Console.WriteLine("\nError: Invalid input, try again\n");
            input = Console.ReadLine();
        }

        return int.Parse(input);
    }

    public static int GetRunIdInput(IRunController controller)
    {
        var id = GetIntInput();

        while (controller.GetRunByIdAsync(id).Result == null)
        {
            Console.WriteLine("\nError: Invalid input, try again\n");
            id = GetIntInput();
        }

        return id;
    }
}