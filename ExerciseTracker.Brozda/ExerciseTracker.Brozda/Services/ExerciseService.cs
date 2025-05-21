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

        public async Task<RepositoryResult<ExerciseDto>> CreateAsync(ExerciseDto dto)
        {
            var model = dto.MapFromDto();

            var result = await ExecuteSafeAsync(() => _repository.Create(model));

            return result.Map(x => x.MapToDto());

        }
        public async Task<RepositoryResult<List<ExerciseDto>>> ViewAllAsync()
        {
            var result = await ExecuteSafeAsync(() => _repository.GetAll());

            return result.Map(x => x.Select(x => x.MapToDto()).ToList());
        }
        public async Task<RepositoryResult<ExerciseDto>> GetByIdAsync(int id)
        {
            var result = await ExecuteSafeAsync<Exercise>(() => _repository.GetById(id)!);

            return result.Map(x=>x.MapToDto());

        }
        public async Task<RepositoryResult<ExerciseDto>> EditAsync(int id, ExerciseDto updatedEntity)
        {
            if (updatedEntity.Id != id)
            { 
                return RepositoryResult<ExerciseDto>.Fail(AppStrings.ServiceErrorUpdateIdMismatch); 
            }

            var entity = updatedEntity.MapFromDto();

            var result =  await ExecuteSafeAsync<Exercise>(() => _repository.Edit(entity)!);

            return result.Map(x=> x.MapToDto());

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

                if(result is null)
                {
                    return RepositoryResult<T>.NotFound();
                }
                return result is not null
                    ? RepositoryResult<T>.Success(result)
                    : RepositoryResult<T>.NotFound();
            }
            catch (Exception ex)
            {
                return RepositoryResult<T>.Fail($"{AppStrings.ServiceErrorOcurred}: {ex.Message}");
            }
        }
        public async Task<RepositoryResult<List<ExerciseType>>> GetExerciseTypes()
        {
            return await ExecuteSafeAsync(() => _repository.GetExerciseTypes());
        }

    }
}
