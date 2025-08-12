using ExerciseUI.Model;
using ExerciseUI.Services;

namespace ExerciseUI.Controllers
{
    internal class ExerciseController : IExerciseController<ExerciseModel>
    {
        private readonly IExerciseService<ExerciseModel> _service;

        public ExerciseController(IExerciseService<ExerciseModel> exerciseService)
        {
            _service = exerciseService;
        }

        public bool AddExercise(ExerciseModel exercise)
        {
            return _service.AddExercise(exercise);
        }

        public ExerciseModel GetExercise(int id)
        {
            return _service.GetExercise(id);
        }

        public IEnumerable<ExerciseModel> GetExercises()
        {
            return _service.GetExercises();
        }

        public bool RemoveExercise(int id)
        {
            return _service.RemoveExercise(id);
        }

        public bool UpdateExercise(ExerciseModel entity)
        {
            return _service.UpdatingExercise(entity);
        }
    }
}
