namespace STUDY.ConsoleProjects.ExerciseTrackerFour.Service;
public interface IUserInput
{
    (DateTime startTime, DateTime endTime, TimeSpan duration, string comments) GetUserInputForExcerciseEntry();

    (DateTime newStartTime, DateTime newEndTime, TimeSpan newDuration, string newComments, int exerciseId) GetUserInputForUpdatedExcerciseEntry();
    int GetUserInputToDelete();
}
