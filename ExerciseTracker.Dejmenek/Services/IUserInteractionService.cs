using ExerciseTracker.Dejmenek.Enums;
using ExerciseTracker.Dejmenek.Models;

namespace ExerciseTracker.Dejmenek.Services;
public interface IUserInteractionService
{
    string GetDateTime();
    string GetComment();
    void GetUserInputToContinue();
    Exercise GetExercise(List<Exercise> exercises);
    MenuOptions GetMenuOption();
    bool GetConfirmation(string title);
}
