namespace ExerciseTracker.Repositories;

interface IRepository<T>
{
    List<T> GetAll();
    T? GetById(int id);
    void Add(T entity);
    void Delete(T entity);
    void Update(T entity);
}