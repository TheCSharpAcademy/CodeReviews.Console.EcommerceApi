using System.Linq.Expressions;
using Ecommerce.Core.Interfaces.Common;
using Ecommerce.Core.Interfaces.Repositories;
using Ecommerce.Core.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T>
    where T : class, IBaseEntity
{
    private readonly AppDbContext _context = context;

    public async Task<List<T>> GetAllAsync(params Expression<Func<T, object?>>[] includeProperties)
    {
        if (includeProperties.Length == 0) 
            return await _context.Set<T>().ToListAsync();
        
        var query = _context.Set<T>().AsQueryable();
        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);
        return await query.ToListAsync();
    }

    public async Task<List<T>> GetAllAsync(PaginationParams paginationParams, params Expression<Func<T, object?>>[] includeProperties)
    {
        var query = _context.Set<T>().AsQueryable();
        if (includeProperties.Length > 0)
        {
             foreach (var includeProperty in includeProperties)
                 query = query.Include(includeProperty);
        }
        return await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync();
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object?>>[] includeProperties)
    {
        var query = _context.Set<T>().AsQueryable();
        if (includeProperties.Length > 0)
        {
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);
        }
        return await query.Where(predicate).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id, params Expression<Func<T, object?>>[] includeProperties)
    {
        if (includeProperties.Length == 0) return await _context.Set<T>().FindAsync(id);
        var query = _context.Set<T>().AsQueryable();
        foreach (var includeProperty in includeProperties)
            query = query.Include(includeProperty);
        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }

    public void Add(T entity)
    {
        _context.Add(entity);
    }

    public void Update(T entity)
    {
        _context.Update(entity);
    }

    public void Delete(ISoftDeletable entity)
    {
        entity.IsDeleted = true;
        entity.DeletedAt = DateTime.UtcNow;
        _context.Update(entity);
    }
}