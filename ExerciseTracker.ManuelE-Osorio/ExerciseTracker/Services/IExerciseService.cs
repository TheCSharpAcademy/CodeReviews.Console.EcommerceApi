namespace ExerciseTracker.Services;

public interface IExerciseService<T> where T : class
{
    bool TryConnection();
    IEnumerable<T>? GetAll();
    T? GetById(int id);
    bool Insert(T model);
    bool Update(T model);
    bool Delete(T model);
}