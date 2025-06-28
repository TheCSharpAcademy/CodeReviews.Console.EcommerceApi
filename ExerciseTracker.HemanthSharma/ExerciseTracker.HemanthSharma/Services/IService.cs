using ExerciseTracker.Study.Models.DTO;

namespace ExerciseTracker.Study.Services
{
    public interface IService<T>
    {
        Task<ResponseDto<T>> GetAll();
        Task<ResponseDto<T>> GetById(int Id);
        Task<ResponseDto<T>> Update(T Entity);
        Task<ResponseDto<T>> Delete(int Id);
        Task<ResponseDto<T>> Create(T Entity);
    }
}
