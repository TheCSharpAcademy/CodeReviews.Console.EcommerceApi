namespace ExerciseTracker.SpyrosZoupas.DAL.Repository;

public interface IRepository<T>
{
    T GetById(int id);
    IList<T> GetAll();
    void Insert(T entity);
    void Update(T entity);
    void Delete(int id);
}
