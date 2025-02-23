using ExerciseTracker.Dejmenek.Controllers;
using ExerciseTracker.Dejmenek.Data;
using ExerciseTracker.Dejmenek.Data.Repositories;
using ExerciseTracker.Dejmenek.Enums;
using ExerciseTracker.Dejmenek.Helpers;
using ExerciseTracker.Dejmenek.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var exerciseContext = new ExerciseContext();
        var exerciseRepository = new ExerciseRepository(exerciseContext);
        var userInteractionService = new UserInteractionService();
        var exerciseService = new ExerciseService(exerciseRepository, userInteractionService);
        var exercisesController = new ExercisesController(exerciseService);

        bool exit = false;

        while (!exit)
        {
            var userMenuOption = userInteractionService.GetMenuOption();

            switch (userMenuOption)
            {
                case MenuOptions.Exit:
                    exit = true;
                    break;

                case MenuOptions.AddExercise:
                    exercisesController.AddExercise();
                    userInteractionService.GetUserInputToContinue();
                    Console.Clear();
                    break;

                case MenuOptions.RemoveExercise:
                    exercisesController.RemoveExercise();
                    userInteractionService.GetUserInputToContinue();
                    Console.Clear();
                    break;

                case MenuOptions.UpdateExercise:
                    exercisesController.UpadateExercise();
                    userInteractionService.GetUserInputToContinue();
                    Console.Clear();
                    break;

                case MenuOptions.ViewExercises:
                    var exercisesDtos = exercisesController.GetExercises();
                    DataVisualizer.DisplayExercises(exercisesDtos);
                    userInteractionService.GetUserInputToContinue();
                    Console.Clear();
                    break;
            }
        }
    }
}