namespace ExerciseTracker.wkktoria;

public static class Validation
{
    public static bool IsStartDateBeforeEndDate(DateTime startDate, DateTime endDate)
    {
        return startDate < endDate;
    }

    public static bool IsValidDuration(TimeSpan duration)
    {
        return duration.TotalHours < 24;
    }
}