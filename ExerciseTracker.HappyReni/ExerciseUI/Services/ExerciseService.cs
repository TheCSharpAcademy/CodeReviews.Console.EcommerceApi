using ExerciseUI.Model;
using ExerciseUI.Repositories;

namespace ExerciseUI.Services
{
    internal class ExerciseService : IExerciseService<ExerciseModel>
    {
        private readonly IRepository<ExerciseModel> _repository;
        public ExerciseService(IRepository<ExerciseModel> repository)
        {
            _repository = repository;
        }

        public IEnumerable<ExerciseModel> GetExercises() => _repository.GetAll();
        public bool AddExercise(ExerciseModel entity)
        {
            return _repository.Create(entity);
        }

        public bool RemoveExercise(int id)
        {
            return _repository.Delete(id);
        }

        public bool UpdatingExercise(ExerciseModel entity)
        {
            return _repository.Update(entity);
        }

        public ExerciseModel GetExercise(int id)
        {
            return _repository.Get(id);
        }
    }
}
