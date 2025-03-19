using ExerciseTracker.ASV.Models;

namespace ExerciseTracker.ASV.Repositories;

public interface IExerciseRepository
{
    public Task<List<ExerciseData>> GetExercises();
    public Task<ExerciseData> GetExerciseById(int id);
    public Task<bool> DeleteExercise(int id);
    public Task<bool> PutExercise(ExerciseData exerciseData);
    public Task<bool> PostExercise(ExerciseData exerciseData);
}