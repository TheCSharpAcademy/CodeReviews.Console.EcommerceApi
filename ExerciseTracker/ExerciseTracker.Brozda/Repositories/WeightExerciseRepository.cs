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
    internal class WeightExerciseRepository : IExerciseRepository
    {
        private readonly ExerciseTrackerContext _dbContext;

        /// <summary>
        /// Initializes new instance of <see cref="WeightExerciseRepository"/>
        /// </summary>
        /// <param name="dbContext">Database context for EF core for Excercise Tracker application</param>
        public WeightExerciseRepository(ExerciseTrackerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Exercise> Create(Exercise entity)
        {
            await _dbContext.ExercisesWeight.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<List<Exercise>> GetAll()
        {
            return await _dbContext.ExercisesWeight
                .Include(s => s.Type)
                .ToListAsync();
        }

        public async Task<Exercise?> GetById(int id)
        {
            var entity = await _dbContext.ExercisesWeight.FindAsync(id);

            return entity is not null ? entity : null;
        }

        public async Task<Exercise?> Edit(Exercise updatedEntity)
        {
            var original = await _dbContext.ExercisesWeight.FindAsync(updatedEntity.Id);

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
            var affectedRows = await _dbContext.ExercisesWeight.Where(x => x.Id == id).ExecuteDeleteAsync();

            return affectedRows > 0;
        }

        public async Task<List<ExerciseType>> GetExerciseTypes()
        {
            return await _dbContext.ExerciseTypes.ToListAsync();
        }
    }
}