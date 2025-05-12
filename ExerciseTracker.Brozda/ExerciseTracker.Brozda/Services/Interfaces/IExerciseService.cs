using ExerciseTracker.Brozda.Models;

namespace ExerciseTracker.Brozda.Services.Interfaces
{
    internal interface IExerciseService
    {
        Task<RepositoryResult<Exercise>> CreateAsync(Exercise dto);
        Task<RepositoryResult<bool>> DeleteAsync(int id);
        Task<RepositoryResult<Exercise>> EditAsync(int id, Exercise updatedEntity);
        Task<RepositoryResult<Exercise>> GetByIdAsync(int id);
        Task<RepositoryResult<List<Exercise>>> ViewAllAsync();
    }
}