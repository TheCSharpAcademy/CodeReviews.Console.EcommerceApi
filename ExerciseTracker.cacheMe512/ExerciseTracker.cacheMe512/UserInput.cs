using System.Globalization;

namespace ExerciseTracker.cacheMe512;

internal static class UserInput
{
    public static Exercise GetExerciseInput()
    {
        var exercise = new Exercise();

        exercise.DateStart = GetValidatedDateTime("Enter start date/time (yyyy-MM-dd HH:mm): ");

        while (true)
        {
            exercise.DateEnd = GetValidatedDateTime("Enter end date/time (yyyy-MM-dd HH:mm): ");
            if (exercise.DateEnd <= exercise.DateStart)
            {
                Console.WriteLine("End date/time must be after start date/time. Please try again.");
            }
            else
            {
                break;
            }
        }

        exercise.Duration = exercise.DateEnd - exercise.DateStart;
        exercise.Sets = GetValidatedInt("Enter number of sets: ");
        exercise.Reps = GetValidatedInt("Enter number of reps: ");
        exercise.Weight = GetValidatedDouble("Enter weight: ");

        return exercise;
    }

    private static DateTime GetValidatedDateTime(string prompt)
    {
        DateTime dateTime;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            var validationResult = Validation.ValidateDateTime(input);
            if (!validationResult.Successful)
            {
                Console.WriteLine(validationResult.Message);
                continue;
            }
            if (DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }
            else
            {
                Console.WriteLine("Unexpected error parsing date/time. Please try again.");
            }
        }
    }

    private static int GetValidatedInt(string prompt)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (!int.TryParse(input, out value))
            {
                Console.WriteLine("Invalid integer. Please try again.");
                continue;
            }
            var validationResult = Validation.ValidatePositiveInt(value);
            if (!validationResult.Successful)
            {
                Console.WriteLine(validationResult.Message);
                continue;
            }
            return value;
        }
    }

    private static double GetValidatedDouble(string prompt)
    {
        double value;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            var validationResult = Validation.ValidatePositiveDouble(input);
            if (!validationResult.Successful)
            {
                Console.WriteLine(validationResult.Message);
                continue;
            }
            if (double.TryParse(input, out value))
            {
                return value;
            }
            else
            {
                Console.WriteLine("Invalid number. Please try again.");
            }
        }
    }
}

