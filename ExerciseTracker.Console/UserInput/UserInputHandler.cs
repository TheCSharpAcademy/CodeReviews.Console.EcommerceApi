using ExerciseTracker.Niasua.Models;

namespace ExerciseTracker.Niasua.UserInput;

public static class UserInputHandler
{
    public static Exercise GetExerciseInput()
    {
        Console.WriteLine("Enter exercise start date and time (yyyy-mm-dd hh:mm)");
        DateTime start = ReadDateTime();

        Console.WriteLine("Enter exercise start date and time (yyyy-mm-dd hh:mm)");
        DateTime end = ReadDateTime();

        Console.WriteLine("Enter any comments: ");
        string? comments = Console.ReadLine();

        return new Exercise
        {
            DateStart = start,
            DateEnd = end,
            Comments = comments
        };
    }

    private static DateTime ReadDateTime()
    {
        while (true)
        {
            var input = Console.ReadLine();

            if (DateTime.TryParse(input, out var result))
                return result;

            Console.WriteLine("Invalid format. Please use format like: yyyy-mm-dd hh:mm");
        }
    }
}
