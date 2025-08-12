using ExerciseTracker.Cactus.Service;
using ExerciseTracker.Cactus.UI;
using Spectre.Console;

enum MenuOptions
{
    AddExercise,
    DeleteExercise,
    ViewAllExercises,
    UpdateExercise,
    Quit
}

namespace ExerciseTracker.Cactus.Controller
{
    public class ExerciseController
    {
        private readonly ExerciseService _exerciseService;

        public ExerciseController(ExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        public async Task MainMenu()
        {
            bool isAppRunning = true;
            while (isAppRunning)
            {
                var option = AnsiConsole.Prompt(
                new SelectionPrompt<MenuOptions>()
                .Title("What would you like to do?")
                .AddChoices(
                    MenuOptions.AddExercise,
                    MenuOptions.ViewAllExercises,
                    MenuOptions.UpdateExercise,
                    MenuOptions.DeleteExercise,
                    MenuOptions.Quit));

                switch (option)
                {
                    case MenuOptions.AddExercise:
                        var exercise = await _exerciseService.AddExerciseAsync();
                        UserInterface.ShowExercise(exercise);
                        UserInterface.BackToMainMenuPrompt();
                        break;
                    case MenuOptions.DeleteExercise:
                        var deletedExercise = await _exerciseService.DeleteExerciseAsync();
                        UserInterface.ShowExercise(deletedExercise);
                        UserInterface.BackToMainMenuPrompt();
                        break;
                    case MenuOptions.ViewAllExercises:
                        var exercises = await _exerciseService.GetAllExercisesAsync();
                        UserInterface.ShowExercise(exercises);
                        UserInterface.BackToMainMenuPrompt();
                        break;
                    case MenuOptions.UpdateExercise:
                        var updatedExercise = await _exerciseService.UpdateExerciseAsync();
                        UserInterface.ShowExercise(updatedExercise);
                        UserInterface.BackToMainMenuPrompt();
                        break;
                    case MenuOptions.Quit:
                        isAppRunning = false;
                        break;
                }
            }
        }
    }
}
