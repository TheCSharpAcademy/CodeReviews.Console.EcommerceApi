using ExerciseTracker.Repositories;

namespace ExerciseTracker.Services
{
    class ExerciceController<IExercices>
    {
        private readonly IRepository<IExercices> _exercicesRepository;

        public ExerciceController(IRepository<IExercices> exercicesRepository)
        {
            _exercicesRepository = exercicesRepository;
        }

        internal List<IExercices> GetExercices()
        {
            return _exercicesRepository.GetAll();
        }

        internal IExercices? GetExerciceById(int id)
        {
            return _exercicesRepository.GetById(id);
        }

        internal void DeleteExercice(IExercices exercice)
        {
            _exercicesRepository.Delete(exercice);
        }

        internal void AddExercice(IExercices exercice)
        {
            _exercicesRepository.Add(exercice);
        }

        internal void UpdateExercice(IExercices exercice)
        {
            _exercicesRepository.Update(exercice);
        }
    }
}