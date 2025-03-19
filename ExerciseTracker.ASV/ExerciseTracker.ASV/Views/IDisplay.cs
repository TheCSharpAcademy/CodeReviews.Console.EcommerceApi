using ExerciseTracker.ASV.Models;

namespace ExerciseTracker.ASV.Views;

public interface IDisplay
{
    void DisplayWorkouts(List<ExerciseData> exercises, string[] columns, string title);
    public string GetSelection(string title, string[] choices);
}