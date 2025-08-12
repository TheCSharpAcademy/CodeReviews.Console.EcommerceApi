using System.Globalization;

namespace ExerciseUI
{
    public class Validation
    {
        public static DateTime CheckDateTime(string date)
        {
            string format = "yyyy-MM-dd HH:mm:ss";

            if (DateTime.TryParseExact(date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return DateTime.Parse(date);
            }

            throw new Exception("Invalid DateTime format.");
        }
        public static bool CheckStartEndTime(DateTime start, DateTime end)
        {
            if (end - start > TimeSpan.Zero)
            {
                return true;
            }

            throw new Exception("end time shouldn't come before start time.");
        }
    }
}
