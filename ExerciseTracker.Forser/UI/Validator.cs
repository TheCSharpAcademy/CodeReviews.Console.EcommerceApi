using System.Globalization;

namespace ExerciseTracker.Forser.UI
{
    internal class Validator
    {
        internal static bool ValidateDateTime(string exerciseDate)
        {
            bool dateValid = false;

            if (DateTime.TryParseExact(exerciseDate, "dd-MM-yy HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                dateValid = true;
            }

            return dateValid;
        }
        internal static bool AreDatesValid(DateTime startDate,  DateTime endDate)
        {
            if (startDate < endDate)
            {
                return true;
            }
            else
            {
                AnsiConsole.WriteLine("End of exercise can't be before start of exercise");
                return false;
            }
        }
    }
}