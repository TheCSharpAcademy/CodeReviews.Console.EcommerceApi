using edvaudin.ExerciseTracker.Models;

namespace edvaudin.ExerciseTracker.Repositories;

internal interface IExerciseRepository
{
    public IEnumerable<Exercise> GetExercises();
    public bool TryGetExerciseById(int id, out Exercise? exercise);
    public void AddExercise(Exercise exercise);
    public void UpdateExercise(Exercise exerciseToUpdate, Exercise exercise);
    public void DeleteExercise(Exercise exercise);
}