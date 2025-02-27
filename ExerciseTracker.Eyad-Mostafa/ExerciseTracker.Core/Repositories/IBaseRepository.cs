namespace ExerciseTracker.Core.Repositories;

public interface IBaseRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    T Add(T entity);
    T? Update(int id, T entity);
    T? Delete(int id);
}
