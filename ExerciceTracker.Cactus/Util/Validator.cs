namespace ExerciseTracker.Cactus.Util
{
    public class Validator
    {
        public static bool IsValidDate(string dateStr, out DateTime date)
        {

            if (DateTime.TryParseExact(dateStr, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out date))
            {
                return true;
            }
            return false;
        }
    }
}
