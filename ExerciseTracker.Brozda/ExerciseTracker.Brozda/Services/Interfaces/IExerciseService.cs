using ExerciseTracker.Brozda.Models;

namespace ExerciseTracker.Brozda.Services.Interfaces
{
    /// <summary>
    /// Defines a contract for service-level CRUD operations related to <see cref="Exercise"/> entities,
    /// </summary>
    internal interface IExerciseService
    {
        /// <summary>
        /// Creates new <see cref="Exercise"/> entry
        /// </summary>
        /// <param name="entity"><see cref="Exercise"/> to be created</param>
        /// <returns>
        /// A <see cref="RepositoryResult{T}"/> containing the created <see cref="Exercise"/> or an error in case of any fail
        /// </returns>
        Task<RepositoryResult<ExerciseDto>> CreateAsync(ExerciseDto entity);
        /// <summary>
        /// Deletes existing <see cref="Exercise"/> entry
        /// </summary>
        /// <param name="id">Id of entry to be deleted</param>
        /// <returns>
        /// A <see cref="RepositoryResult{T}"/> containing the created <see cref="bool"/> value indicating success or fail
        /// </returns>
        Task<RepositoryResult<bool>> DeleteAsync(int id);
        /// <summary>
        /// Updates existing <see cref="Exercise"/> entry
        /// </summary>
        /// <param name="id">Id of entry to be updated</param>
        /// <param name="updatedEntity"><see cref="Exercise"/> entry with updated values</param>
        /// <returns>
        /// A <see cref="RepositoryResult{T}"/> containing the updated <see cref="Exercise"/> or an error in case of any fail
        /// </returns>
        Task<RepositoryResult<ExerciseDto>> EditAsync(int id, ExerciseDto updatedEntity);
        /// <summary>
        /// Retrieves single <see cref="Exercise"/> entry 
        /// </summary>
        /// <param name="id">Id of entry to be retrieved</param>
        /// <returns>
        /// A <see cref="RepositoryResult{T}"/> containing the retrieved <see cref="Exercise"/> or an error in case of any fail
        /// </returns>
        Task<RepositoryResult<ExerciseDto>> GetByIdAsync(int id);
        /// <summary>
        /// Retrieves all <see cref="Exercise"/> entries
        /// </summary>
        /// <returns>
        /// A <see cref="RepositoryResult{T}"/> containing List of all retrieved <see cref="Exercise"/> or an error in case of any fail
        /// </returns>
        Task<RepositoryResult<List<ExerciseDto>>> ViewAllAsync();

        Task<RepositoryResult<List<ExerciseType>>> GetExerciseTypes();
    }
}