namespace edvaudin.ExerciseTracker.Input;

internal interface IUserInput
{
    DateTime GetEndTime(DateTime startTime);
    int GetId();
    string GetOption();
    DateTime GetStartTime();
}