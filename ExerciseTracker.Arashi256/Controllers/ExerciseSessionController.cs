using ExerciseTracker.Arashi256.Classes;
using ExerciseTracker.Arashi256.Models;
using ExerciseTracker.Arashi256.Services;

namespace ExerciseTracker.Arashi256.Controllers
{
    internal class ExerciseSessionController
    {
        private readonly ExerciseSessionService _exerciseService;
        public ExerciseSessionController(ExerciseSessionService service)
        {
            _exerciseService = service;
        }

        public ServiceResponse GetAllExerciseSessions()
        {
            return _exerciseService.GetAllExerciseSessions();
        }

        public ServiceResponse AddNewExerciseSession(ExerciseSessionInputDto newSession)
        {
            return _exerciseService.AddNewExerciseSession(newSession);
        }

        public ServiceResponse DeleteExistingExerciseSession(int id)
        {
            return _exerciseService.DeleteExistingExerciseSession(id);
        }

        public ServiceResponse UpdateExistingExerciseSession(int id, ExerciseSessionInputDto updateSession)
        {
            return _exerciseService.UpdateExistingExerciseSession(id, updateSession);
        }

        public ServiceResponse GetExerciseSessionById(int id)
        {
            return _exerciseService.GetExerciseSessionById(id);
        }
    }
}
