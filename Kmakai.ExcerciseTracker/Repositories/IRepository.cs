using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kmakai.ExerciseTracker.Repositories;

public interface IRepository<T> where T : class
{
    public Task<T> GetAsync(int id);
    public Task<IEnumerable<T>> GetAllAsync();
    public Task<T> AddAsync(T entity);
    public Task<T> UpdateAsync(T entity);
    public Task<T> DeleteAsync(int id);
}
