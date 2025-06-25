namespace ExerciseTracker.Repository;

public interface IRepository<T>
{
   T GetById(int id);
   IEnumerable<T> GetAll();
   Task Insert(T entity);
   Task Update(T entity);
   Task Delete(T entity);
   
}