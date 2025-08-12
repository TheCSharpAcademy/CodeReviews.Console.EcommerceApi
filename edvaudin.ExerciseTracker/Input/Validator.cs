using System.Globalization;

namespace edvaudin.ExerciseTracker.Input;

internal static class Validator
{
    public static bool IsValidOption(string input)
    {
        string[] validOptions = { "v", "a", "d", "u", "0" };
        foreach (string validOption in validOptions)
        {
            if (input == validOption)
            {
                return true;
            }
        }
        return false;
    }
    public static bool IsValidDateInput(string input)
    {
        return DateTime.TryParseExact(input, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }

    public static bool IsDateAfterStartTime(string input, DateTime startTime)
    {
        return DateTime.ParseExact(input, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None) > startTime;
    }

    public static DateTime ConvertToDate(string time)
    {
        return DateTime.ParseExact(time, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
    }
}
