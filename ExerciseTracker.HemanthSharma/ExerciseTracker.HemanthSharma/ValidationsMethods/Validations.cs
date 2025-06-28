using System.Globalization;

namespace ExerciseTracker.Study.ValidationsMethods
{
    public class Validations
    {
        public static bool ValidDate(string Datestring)
        {
            return DateTime.TryParseExact(Datestring, "dd-MM-yyyy", null, DateTimeStyles.None, out DateTime ShiftDate);
        }
        public static bool ValidTime(string Timestring)
        {
            return DateTime.TryParseExact(Timestring, "HH:mm", null, DateTimeStyles.None, out DateTime ShiftDate);
        }
    }
}
