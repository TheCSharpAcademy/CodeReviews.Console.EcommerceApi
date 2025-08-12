namespace ExerciseTracker.Forser.UI
{
    internal interface IUserInterface
    {
        DateTime GetEndTime(DateTime startTime);
        int GetExerciseId(int id);
        DateTime GetStartTime();
    }
}