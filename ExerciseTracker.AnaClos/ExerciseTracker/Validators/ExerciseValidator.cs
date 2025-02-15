using ExerciseTracker.Models;
using System.Globalization;

namespace ExerciseTracker.Validators;

public class ExerciseValidator
{
    public static bool IntervalValidator(DateTime start, DateTime end)
    {
        TimeSpan timeSpan = end - start;
        double eightHours = 3600 * 8;
        return timeSpan.TotalSeconds > 0 && timeSpan.TotalSeconds <= eightHours ? true : false;
    }

    public static DateTime DateTimeValidator(string date)
    {
        DateTime dateTime = DateTime.MinValue;
        DateTime.TryParseExact(date, "yy/MM/dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out dateTime);
        return dateTime;
    }

    public static bool OrderValidator(Exercise lastShift, DateTime newStartTime)
    {
        return newStartTime - lastShift.DateEnd >= TimeSpan.Zero ? true : false;
    }
}