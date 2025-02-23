using System.Globalization;
using ExerciseTracker;

public class Validation
{
    public static bool ValidateStartDate(string startDate)
    {
        var parsedDate = DateTime.TryParseExact(startDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        if (parsedDate == true) return true;
        else return false;
    }

    internal static bool ValidateEndDate(string endDate, string startDate)
    {
        var parsedDate = DateTime.TryParseExact(endDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        if (parsedDate == true && DateTime.Parse(endDate) > DateTime.Parse(startDate)) return true;
        else return false;
    }

    internal static bool ValidateId(string userInput, IEnumerable<Exercise> exercises)
    {
        bool parsed = int.TryParse(userInput, out int id);
        if (!parsed) return false;
        else
        {
            foreach (var exercise in exercises)
            {
                if (exercise.Id == id) return true;
            }
            return false;
        }
    }
}