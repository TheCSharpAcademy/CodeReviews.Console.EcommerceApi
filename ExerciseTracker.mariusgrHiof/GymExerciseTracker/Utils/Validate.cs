using GymExerciseTracker.Services;
using System.Globalization;

namespace GymExerciseTracker.Utils;
public class Validate
{
    static private readonly IExerciseService _exerciseService;


    public static bool IsValidNumber(string number)
    {
        return int.TryParse(number, out _);
    }

    public static bool IsValidId(string inputId)
    {
        if (!IsValidNumber(inputId)) return false;


        int id = int.Parse(inputId);

        var session = _exerciseService.GetGymSession(id);

        return session != null;
    }

    public static bool IsValidString(string name)
    {
        return !string.IsNullOrWhiteSpace(name);

    }

    public static bool IsValidateDate(string date)
    {
        return DateTime.TryParseExact(date, "dd/MM/yyyy HH:mm", new CultureInfo("nb-NO"), DateTimeStyles.None, out _);
    }

    public static bool IsValidDateRange(DateTime DateStart, DateTime DateEnd)
    {
        TimeSpan timeSpan = DateEnd - DateStart;

        return timeSpan.Ticks > 0;

    }
}

