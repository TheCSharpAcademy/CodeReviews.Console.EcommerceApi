using ExerciseTracker.Brozda.Helpers;
using ExerciseTracker.Brozda.Models;
using ExerciseTracker.Brozda.Repositories.Interfaces;
using ExerciseTracker.Brozda.Services.Interfaces;

namespace ExerciseTracker.Brozda.Services
{
    /// <summary>
    /// Provides business logic and service-level CRUD operations for managing <see cref="Exercise"/> entities.
    /// Implements the <see cref="IExerciseService"/>
    /// </summary>
    internal class ExerciseService : IExerciseService
    {
        public IExerciseRepository _repository;
        /// <summary>
        /// Initializes new instance of <see cref="ExerciseService"/>
        /// </summary>
        /// <param name="repository">Repository instance for data access</param>
        public ExerciseService(IExerciseRepository repository)
        {
            _repository = repository;
        }

        public async Task<RepositoryResult<Exercise>> CreateAsync(Exercise dto)
        {
            return await ExecuteSafeAsync( () => _repository.Create(dto));

        }
        public async Task<RepositoryResult<List<Exercise>>> ViewAllAsync()
        {
            var result = await ExecuteSafeAsync(() => _repository.GetAll());
            return await ExecuteSafeAsync(() => _repository.GetAll());
        }
        public async Task<RepositoryResult<Exercise>> GetByIdAsync(int id)
        {
            return await ExecuteSafeAsync<Exercise>(() => _repository.GetById(id)!);

        }
        public async Task<RepositoryResult<Exercise>> EditAsync(int id, Exercise updatedEntity)
        {
            if (updatedEntity.Id != id)
            { 
                return RepositoryResult<Exercise>.Fail(AppStrings.ServiceErrorUpdateIdMismatch); 
            }

            return await ExecuteSafeAsync<Exercise>(() => _repository.Edit(updatedEntity)!);

        }
        public async Task<RepositoryResult<bool>> DeleteAsync(int id)
        {
            return await ExecuteSafeAsync(() => _repository.DeleteById(id));

        }
        /// <summary>
        /// Safely executes action and return data in form of <see cref="RepositoryResult{T}"/>
        /// </summary>
        /// <typeparam name="T">Type expected from repository</typeparam>
        /// <param name="action">Action to be run against the repository</param>
        /// <returns>A Task result contains <see cref="RepositoryResult{T}"/> with expected data based on the request
        /// Can return Successful result, NotFound result or Fail in case of any error</returns>
        private async Task<RepositoryResult<T>> ExecuteSafeAsync<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action();

                return result is not null
                    ? RepositoryResult<T>.Success(result)
                    : RepositoryResult<T>.NotFound();
            }
            catch (Exception ex)
            {
                return RepositoryResult<T>.Fail($"{AppStrings.ServiceErrorOcurred}: {ex.Message}");
            }
        }
    }
}
