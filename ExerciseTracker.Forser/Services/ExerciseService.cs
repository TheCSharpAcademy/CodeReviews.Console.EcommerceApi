namespace ExerciseTracker.Forser.Services
{
    internal class ExerciseService : IExerciseService
    {
        private readonly IExerciseReposoitory _exerciseReposoitory;
        public ExerciseService(IExerciseReposoitory exerciseReposoitory)
        {
            _exerciseReposoitory = exerciseReposoitory;
        }
        public bool AddExercise(DateTime start, DateTime end, string? comments)
        {
            Exercise newExercise = new()
            {
                DateStart = start,
                DateEnd = end,
                Duration = end - start,
                Comments = comments
            };
            return _exerciseReposoitory.AddExercise(newExercise);
        }
        public void DeleteExercise(int id)
        {
            if(!_exerciseReposoitory.GetExerciseById(id, out Exercise? exerciseToDelete))
            {
                AnsiConsole.WriteLine("Couldn't find the exercise you wanted to delete");
                return;
            }
            if (exerciseToDelete == null)
            {
                AnsiConsole.WriteLine("Something went wrong when trying to get the exercise");
                return;
            }
            _exerciseReposoitory.DeleteExercise(exerciseToDelete);
            AnsiConsole.WriteLine("Successfully deleted exercise.");
        }
        public void DisplayExercises()
        {
            List<Exercise> exercises = _exerciseReposoitory.GetExercises().ToList();
            UserInterface.DisplayExerciseTable(exercises);
        }
        public Exercise EditExercise(int id)
        {
            return _exerciseReposoitory.EditExerciseById(id);
        }
        public bool UpdateExercise(Exercise exercise)
        {
            return _exerciseReposoitory.EditExercise(exercise);
        }
    }
}