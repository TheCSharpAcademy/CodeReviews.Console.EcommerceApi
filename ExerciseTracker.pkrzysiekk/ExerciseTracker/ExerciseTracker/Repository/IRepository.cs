namespace ExerciseTracker.Repository;

public interface IRepository<T>
{
    Task<T?> GetById(int id);

    Task<IEnumerable<T>> GetAll();

    Task Insert(T entity);

    Task Update(T entity);

    Task Delete(T entity);

    Task SaveChanges();
}