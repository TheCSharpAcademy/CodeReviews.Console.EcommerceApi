namespace App.Database;

public interface IRepositoryBase<T>
{
    public Task<List<T>> ListAll();
    public Task<T> GetById(int id);
    public Task<T> CreateOne(T entity);
    public Task<T> UpdateOne(T entity);
    public Task<bool?> DeleteOne(int id);
}