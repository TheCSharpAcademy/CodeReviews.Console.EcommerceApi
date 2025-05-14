namespace ExerciseTracker.SpyrosZoupas.DAL.Repository;

public interface IRepository<T>
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Insert(T entity);
    void Update(T entity);
    void Delete(T entity);
}
