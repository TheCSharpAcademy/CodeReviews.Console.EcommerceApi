namespace ExerciseTracker.ASV.Services;

public interface IExerciseService
{
    public Task DeleteWorkoutAsync();
    public Task EditWorkoutAsync();

    public Task CreateWorkoutAsync();
    public Task DisplayAllWorkoutsAsync();
    public string GetSelection();
}