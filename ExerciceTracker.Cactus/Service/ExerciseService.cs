using ExerciseTracker.Cactus.Data.Interfaces;
using ExerciseTracker.Cactus.Model;
using Spectre.Console;

namespace ExerciseTracker.Cactus.Service
{
    public class ExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseService(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<IEnumerable<Exercise>> GetAllExercisesAsync()
        {
            return await _exerciseRepository.GetAllAsync();
        }

        public async Task<Exercise> AddExerciseAsync()
        {
            Exercise exercise = ExerciseServiceHelper.InputExercise();

            await _exerciseRepository.AddAsync(exercise);

            return exercise;
        }

        public async Task<Exercise> UpdateExerciseAsync()
        {
            var exercises = await _exerciseRepository.GetAllAsync();

            if (exercises.Count() <= 0) { return null; }

            Exercise selectedExercise = ExerciseServiceHelper.SelectExerciseById(exercises);

            selectedExercise.DateStart = AnsiConsole.Confirm("Update start date?") ? ExerciseServiceHelper.GetValidDate() : selectedExercise.DateStart;
            selectedExercise.DateEnd = AnsiConsole.Confirm("Update end date?") ? ExerciseServiceHelper.GetValidEndDate(selectedExercise.DateStart) : selectedExercise.DateEnd;
            selectedExercise.Duration = AnsiConsole.Confirm("Update duarion?") ? AnsiConsole.Ask<int>("Please input duration:") : selectedExercise.Duration;
            selectedExercise.Comments = AnsiConsole.Confirm("Update comments?") ? AnsiConsole.Ask<string>("Please input your comments:") : selectedExercise.Comments;

            await _exerciseRepository.UpdateAsync(selectedExercise);

            return selectedExercise;
        }

        public async Task<Exercise> DeleteExerciseAsync()
        {
            var exercises = await _exerciseRepository.GetAllAsync();

            if (exercises.Count() <= 0) { return null; }

            var selectedExeercise = ExerciseServiceHelper.SelectExerciseById(exercises);

            await _exerciseRepository.DeleteAsync(selectedExeercise.Id);

            return selectedExeercise;
        }
    }
}
