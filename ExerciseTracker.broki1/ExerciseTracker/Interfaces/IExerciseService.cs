using ExerciseTracker.Models;

namespace ExerciseTracker.Interfaces;

internal interface IExerciseService
{
    public Exercise CreateNewExercise();

    public void ViewAllExercises();

    public Exercise GetExercise();

    public void UpdateExercise(Exercise exercise);

    public void DeleteExercise(Exercise exercise);
}