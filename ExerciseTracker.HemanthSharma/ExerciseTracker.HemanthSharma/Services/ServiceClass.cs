using ExerciseTracker.Study.Models.DTO;
using ExerciseTracker.Study.Repositories;

namespace ExerciseTracker.Study.Services
{
    public class ServiceClass<T> : IService<T> where T:class
    {
        private readonly IRepository<T> Repo;
        public ServiceClass(IRepository<T> _repo)
        {
            Repo = _repo;
        }
        public async Task<ResponseDto<T>> Create(T Entity)
        {
            return await Repo.Create(Entity); 
        }

        public async Task<ResponseDto<T>> Delete(int Id)
        {
            return await Repo.Delete(Id);
        }

        public async Task<ResponseDto<T>> GetAll()
        {
            return await Repo.GetAll();
        }

        public async Task<ResponseDto<T>> GetById(int Id)
        {
            return await Repo.GetById(Id);
        }

        public async Task<ResponseDto<T>> Update(T Entity)
        {
            return await Repo.Update(Entity);
        }
    }
}
