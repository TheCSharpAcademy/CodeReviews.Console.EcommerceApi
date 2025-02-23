using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly ExerciseEFDbContext ExerciseDbContext;

        public EFRepository(ExerciseEFDbContext exerciseDbContext)
        {
            ExerciseDbContext = exerciseDbContext;
        }

        public IQueryable<TEntity> GetAll()
        {
            try
            {
                return ExerciseDbContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                await ExerciseDbContext.AddAsync(entity);
                await ExerciseDbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null");
            }

            try
            {
                ExerciseDbContext.Update(entity);
                await ExerciseDbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }

    public async Task DeleteAsync(TEntity entity)
    {
        ExerciseDbContext.Remove(entity);
        await ExerciseDbContext.SaveChangesAsync();
    }
}