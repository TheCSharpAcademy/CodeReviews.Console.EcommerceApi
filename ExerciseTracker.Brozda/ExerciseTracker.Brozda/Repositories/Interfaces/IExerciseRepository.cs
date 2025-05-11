using ExerciseTracker.Brozda.Models;

namespace ExerciseTracker.Brozda.Repositories.Interfaces
{
    internal interface IExerciseRepository
    {
        Task<Exercise> Create(Exercise entity);
        Task<bool> DeleteById(int id);
        Task<Exercise?> Edit(Exercise updatedEntity);
        Task<List<Exercise>> GetAll();
        Task<Exercise?> GetById(int id);
    }
}