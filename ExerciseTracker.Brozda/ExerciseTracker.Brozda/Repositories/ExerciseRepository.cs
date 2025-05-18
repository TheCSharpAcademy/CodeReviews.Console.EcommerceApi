using ExerciseTracker.Brozda.Data;
using ExerciseTracker.Brozda.Models;
using ExerciseTracker.Brozda.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.Brozda.Repositories
{
    /// <summary>
    /// Provides CRUD operations for <see cref="Exercise"/> entities using Entity Framework Core.
    /// Implements <see cref="IExerciseRepository"/>
    /// </summary>
    internal class ExerciseRepository : IExerciseRepository
    {
        private readonly ExerciseTrackerContext _dbContext;

        /// <summary>
        /// Initializes new instance of <see cref="ExerciseRepository"/>
        /// </summary>
        /// <param name="dbContext">Database context for EF core for Excercise Tracker application</param>
        public ExerciseRepository(ExerciseTrackerContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Exercise> Create(Exercise entity)
        {
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

