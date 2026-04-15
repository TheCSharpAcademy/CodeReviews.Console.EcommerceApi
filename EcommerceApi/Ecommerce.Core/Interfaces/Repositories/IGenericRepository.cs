using System.Linq.Expressions;
using Ecommerce.Core.Interfaces.Common;
using Ecommerce.Core.Utilities;

namespace Ecommerce.Core.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetAllAsync(params Expression<Func<T, object?>>[] includeProperties);
    Task<List<T>> GetAllAsync(PaginationParams paginationParams, params Expression<Func<T, object?>>[] includeProperties);
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object?>>[] includeProperties);
    Task<T?> GetByIdAsync(int id, params Expression<Func<T, object?>>[] includes);
    void Add(T entity);
    void Update(T entity);
    void Delete(ISoftDeletable entity);
}