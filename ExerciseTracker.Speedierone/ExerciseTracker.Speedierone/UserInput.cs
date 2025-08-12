using System.Globalization;

namespace ExerciseTracker.Speedierone
{
    public class UserInput
    {
        internal static string CheckDate()
        {
            string dateString = Console.ReadLine();
            string format = "dd/MM/yyyy";

            while (!DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine($"Invalid format. Please enter in format {format}.");
                dateString = Console.ReadLine();
            }
            return dateString;
        }

        public static bool CheckEndDate(string startDate, string endDate)
        {
            try
            {
                var startDateParsed = DateTime.Parse(startDate);
                var endDateParsed = DateTime.Parse(endDate);

                if (startDateParsed > endDateParsed)
                {
                    Console.WriteLine("End date is before start date. Press any button to continue");
                    return false;
                }
                if (endDateParsed > startDateParsed.AddDays(1))
                {
                    Console.WriteLine("End date must be less then 1 day from start date.");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message );
                return false;
            }
        }
        public static string CheckTime()
        {
            string timeString = Console.ReadLine();
            string format = "HH:mm";

            while (!DateTime.TryParseExact(timeString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                Console.WriteLine($"Invalid format. Please enter in format {format}");
                timeString = Console.ReadLine();
            }
            return timeString;
        }

        public TimeSpan GetDuration(DateTime startTime, DateTime endTime)
        {
            TimeSpan duration = endTime - startTime;
            return duration;
        }

        public DateTime CombinedDateTime(string date, string time)
        {
            try
            {
                var userDate = date;
                var userTime = time;

                DateTime parsedDate = DateTime.Parse(userDate);
                DateTime parsedTime = DateTime.Parse(userTime);

                DateTime combinedDateTime = new DateTime(
                    parsedDate.Year,
                    parsedDate.Month,
                    parsedDate.Day,
                    parsedTime.Hour,
                    parsedTime.Minute,
                    0);

                return combinedDateTime;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
