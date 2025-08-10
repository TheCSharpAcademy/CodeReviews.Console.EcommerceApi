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
    internal class ExerciseService : IWeightExerciseService, ICardioExerciseService
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
            var model = Exercise.MapFromDto(dto);

            return await ExecuteSafeAsync(
                () => _repository.Create(model),
                result => Exercise.MapToDto(result));
        }

        public async Task<RepositoryResult<List<ExerciseDto>>> ViewAllAsync()
        {
            return await ExecuteSafeAsync(
                () => _repository.GetAll(),
                result => result.Select(x => Exercise.MapToDto(x))
                .ToList()
                );
        }

        public async Task<RepositoryResult<ExerciseDto>> GetByIdAsync(int id)
        {
            return await ExecuteSafeAsync(
                () => _repository.GetById(id),
                result => Exercise.MapToDto(result!));
        }

        public async Task<RepositoryResult<ExerciseDto>> EditAsync(int id, ExerciseDto updatedEntity)
        {
            if (updatedEntity.Id != id)
            {
                return RepositoryResult<ExerciseDto>.Fail(AppStrings.ServiceErrorUpdateIdMismatch);
            }

            var entity = Exercise.MapFromDto(updatedEntity);

            return await ExecuteSafeAsync(
                () => _repository.Edit(entity),
                result => Exercise.MapToDto(result!));
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
        ///
        public async Task<RepositoryResult<List<ExerciseType>>> GetExerciseTypes()
        {
            return await ExecuteSafeAsync(
                () => _repository.GetExerciseTypes(),
                x => x);
        }

        /// <summary>
        /// Safely executes action and return data in form of <see cref="RepositoryResult{TOut}"/>
        /// </summary>
        /// <typeparam name="TIn">Type returned from repository</typeparam>
        /// <typeparam name="TOut">Type received from repository mapped to DTO/typeparam>
        /// <param name="dbCall">Action to be run against the repository</param>
        /// <param name="mapToResult">Mapping function used to map returned data to respective DTO</param>
        ///<returns>A Task result contains<see cref="RepositoryResult{T}"/> with expected data based on the request
        /// Can return Successful result, NotFound result or Fail in case of any error</returns>
        private async Task<RepositoryResult<TOut>> ExecuteSafeAsync<TIn, TOut>(
            Func<Task<TIn>> dbCall,
            Func<TIn, TOut> mapToResult)
        {
            try
            {
                var result = await dbCall();

                if (result is null)
                {
                    return RepositoryResult<TOut>.NotFound();
                }

                var dtoData = mapToResult(result);

                return RepositoryResult<TOut>.Success(dtoData);
            }
            catch (Exception ex)
            {
                return RepositoryResult<TOut>.Fail($"{AppStrings.ServiceErrorOcurred}: {ex.Message}");
            }
        }

        /// <summary>
        /// Safely executes action returning <see cref="bool"/> and return data in form of <see cref="RepositoryResult{TOut}"/> containing
        /// bool data
        /// </summary>
        /// <param name="dbCall">Action to be run against the repository returning <see cref="bool"/></param>
        /// <returns>A Task result contains<see cref="RepositoryResult{T}"/></returns>
        private async Task<RepositoryResult<bool>> ExecuteSafeAsync(Func<Task<bool>> dbCall)
        {
            try
            {
                var result = await dbCall();

                return result
                    ? RepositoryResult<bool>.Success(result)
                    : RepositoryResult<bool>.NotFound();
            }
            catch (Exception ex)
            {
                return RepositoryResult<bool>.Fail($"{AppStrings.ServiceErrorOcurred}: {ex.Message}");
            }
        }
    }
}