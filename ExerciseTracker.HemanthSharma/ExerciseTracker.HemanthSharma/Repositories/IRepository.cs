using ExerciseTracker.Study.Models.DTO;

namespace ExerciseTracker.Study.Repositories
{
    public interface IRepository<T>
    {
        Task<ResponseDto<T>> GetAll();
        Task<ResponseDto<T>> GetById(int Id);
        Task<ResponseDto<T>> Update(T Entity);
        Task<ResponseDto<T>> Delete(int Id);
        Task<ResponseDto<T>> Create(T Entity);
    }
}
