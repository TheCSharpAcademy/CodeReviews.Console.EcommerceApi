using ExerciseTracker.ASV.Models;

namespace ExerciseTracker.ASV.UserInput;

public interface IInput
{
    public ExerciseData GetWorkoutDetails();
    int GetWorkoutId();
}