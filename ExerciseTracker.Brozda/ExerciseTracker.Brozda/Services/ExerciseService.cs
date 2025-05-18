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
            try
            {
                var entity = await _repository.Create(dto);

                return RepositoryResult<Exercise>.Success(entity);
            }
            catch (Exception ex)
            {
                return RepositoryResult<Exercise>.Fail($"{AppStrings.ServiceErrorOcurred}: {ex.Message}");
            }

        }
        public async Task<RepositoryResult<List<Exercise>>> ViewAllAsync()
        {
            try
            {
                var entities = await _repository.GetAll();
                return RepositoryResult<List<Exercise>>.Success(entities);
            }
            catch (Exception ex)
            {
                return RepositoryResult<List<Exercise>>.Fail($"{AppStrings.ServiceErrorOcurred}: {ex.Message}");
            }
        }
        public async Task<RepositoryResult<Exercise>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _repository.GetById(id);

                return entity is not null
                    ? RepositoryResult<Exercise>.Success(entity)
                    : RepositoryResult<Exercise>.NotFound();

            }
            catch (Exception ex)
            {
                return RepositoryResult<Exercise>.Fail($"{AppStrings.ServiceErrorOcurred}: {ex.Message}");
            }
        }
        public async Task<RepositoryResult<Exercise>> EditAsync(int id, Exercise updatedEntity)
        {
            try
            {
                if (updatedEntity.Id != id)
                { return RepositoryResult<Exercise>.Fail(AppStrings.ServiceErrorUpdateIdMismatch); }

                var updateResult = await _repository.Edit(updatedEntity);

                return updateResult is not null
                    ? RepositoryResult<Exercise>.Success(updateResult)
                    : RepositoryResult<Exercise>.NotFound();
            }
            catch (Exception ex)
            {
                return RepositoryResult<Exercise>.Fail($"{AppStrings.ServiceErrorOcurred}: {ex.Message}");
            }
        }
        public async Task<RepositoryResult<bool>> DeleteAsync(int id)
        {
            try
            {
                var isDeleteSucessful = await _repository.DeleteById(id);

                return isDeleteSucessful
                    ? RepositoryResult<bool>.Success(isDeleteSucessful)
                    : RepositoryResult<bool>.NotFound();

            }
            catch (Exception ex)
            {
                return RepositoryResult<bool>.Fail($"{AppStrings.ServiceErrorOcurred}: {ex.Message}");
            }
        }
    }
}
