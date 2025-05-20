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
        public async Task<ExerciseDto> Create(ExerciseDto entity)
        {
            Exercise toAdd = entity.MapFromDto();

            await _dbContext.ExercisesWeight.AddAsync(toAdd);
            await _dbContext.SaveChangesAsync();

            return toAdd.MapToDto();
        }
        public async Task<List<ExerciseDto>> GetAll()
        {
            return await _dbContext.ExercisesWeight
                .Include(s => s.Type)
                .Select(s => s.MapToDto())
                .ToListAsync();
                

        }
        public async Task<ExerciseDto?> GetById(int id)
        {
            var entity = await _dbContext.ExercisesWeight.FindAsync(id);
            
            return entity is not null ? entity.MapToDto() : null; 
        }
        public async Task<ExerciseDto?> Edit(ExerciseDto updatedEntity)
        {
            var original = await _dbContext.ExercisesWeight.FindAsync(updatedEntity.Id);

            if (original is null)
            {
                return null;
            }

            original.MapFromUpdate(updatedEntity.MapFromDto());

            await _dbContext.SaveChangesAsync();

            return original.MapToDto();
        }
        public async Task<bool> DeleteById(int id)
        {
            var affectedRows = await _dbContext.ExercisesWeight.Where(x => x.Id == id).ExecuteDeleteAsync();

            return affectedRows > 0;
        }
    }
}

