using ExerciseTracker.jollejonas.Enums;
using ExerciseTracker.jollejonas.Models;

namespace ExerciseTracker.jollejonas.UserInput;

public interface IUserInput
{
    DateTime GetDateTime();
    MenuOptions GetMenuOption();
    string GetExerciseComments();
    Exercise GetExercise(List<Exercise> exercises);
    bool GetConfirmation(string message);
}
