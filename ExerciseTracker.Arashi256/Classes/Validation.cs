namespace ExerciseTracker.Arashi256.Classes
{
    internal class Validation
    {
        public static bool ValidateDatesForDuration(DateTime start, DateTime end)
        {
            return start < end;
        }

        public static TimeSpan CalculateDuration(DateTime start, DateTime end)
        {
            return end - start;
        }
    }
}
