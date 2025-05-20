using ExerciseTracker.Brozda.Models;

namespace ExerciseTracker.Brozda.Repositories.Interfaces
{
    /// <summary>
    /// Defines a contract for accessing and manipulating Exercise entities in a repository.
    /// </summary>
    internal interface IExerciseRepository
    {
        /// <summary>
        /// Creates new <see cref="Exercise"/> record in the Exercise table
        /// </summary>
        /// <param name="entity"><see cref="Exercise"/> entity to be added</param>
        /// <returns><see cref="Exercise"/> entity with populated Id field</returns>
        Task<ExerciseDto> Create(Exercise entity);
        /// <summary>
        /// Deletes an existing record from the database based on its Id
        /// </summary>
        /// <param name="id">Id of <see cref="Exercise"/> record to be deleted</param>
        /// <returns><see cref="bool"/> true is record was sucessfully deleted, false otherwise</returns>
        Task<bool> DeleteById(int id);
        /// <summary>
        /// Updates an existing record int the database with new values
        /// </summary>
        /// <param name="updatedEntity">An <see cref="Exercise"/> object containing updated values</param>
        /// <returns>Returns passed <see cref="Exercise"/> object in case of sucessful update; null otherwise</returns>
        Task<ExerciseDto?> Edit(Exercise updatedEntity);
        /// <summary>
        /// Retrieves all records from Exercise table
        /// </summary>
        /// <returns><see cref="List{}"/> containing all <see cref="Exercise"/> records</returns>
        Task<List<ExerciseDto>> GetAll();
        /// <summary>
        /// Retrieves single record from database based on provided record Id
        /// </summary>
        /// <param name="id">Id of record to be retrieved</param>
        /// <returns>Valied <see cref="Exercise"/> record if it exists, false otherwise</returns>
        Task<ExerciseDto?> GetById(int id);
    }
}