namespace ExerciseTracker.SpyrosZoupas.DAL.Repository;

public interface IRepositoryDapper<T> : IRepository<T>
{
    public void CreateTables();
}
