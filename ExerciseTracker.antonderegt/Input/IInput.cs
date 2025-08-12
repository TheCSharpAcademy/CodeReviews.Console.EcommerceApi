using static ExerciseTracker.Models.Enums;

namespace ExerciseTracker.Input;

public interface IInput
{
    MainMenuOption GetMenuOption();
    (DateTime, DateTime) GetDates();
    string GetComments();
    ExerciseType GetType();
    int GetId(string type);
}