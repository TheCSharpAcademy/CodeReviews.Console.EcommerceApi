using ExerciseTracker.Brozda.Data;
using ExerciseTracker.Brozda.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Brozda.Repositories
{
    internal class ExerciseTracker
    {
        private readonly ExcerciseTrackerContext _dbContext;

        public ExerciseTracker(ExcerciseTrackerContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Exercise> Create(Exercise entity)
        {
            if (entity is null) { throw new ArgumentNullException(nameof(entity)); }

            await _dbContext.Exercises.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<List<Exercise>> GetAll()
        {
            return await _dbContext.Exercises.ToListAsync();
        }
        public async Task<Exercise?> GetById(int id)
        {
            return await _dbContext.Exercises.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Exercise?> Edit(Exercise updatedEntity)
        {
            var original = await _dbContext.Exercises.FindAsync(updatedEntity.Id);
            if (original is null)
            {
                return null;
            }
            original.MapFromUpdate(updatedEntity);

            await _dbContext.SaveChangesAsync();
            
            return original;
        }
        public async Task<bool> DeleteById(int id)
        {
            var affectedRows = await _dbContext.Exercises.Where(x => x.Id == id).ExecuteDeleteAsync();

            return affectedRows > 0;
        }
    }
}

