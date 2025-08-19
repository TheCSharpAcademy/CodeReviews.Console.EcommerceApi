using ExerciseTracker.jollejonas.Controllers;
using ExerciseTracker.jollejonas.UserInput;
using ExerciseTracker.jollejonas.Data;
using ExerciseTracker.jollejonas.Enums;
using ExerciseTracker.jollejonas.Repositories;

class Program
{
    static void Main()
    {
        var exerciseContext = new ExerciseContext();
        var exerciseRepository = new ExerciseRepository(exerciseContext);
        var userInput = new UserInput();
        var exerciseService = new ExerciseService(exerciseRepository, userInput);
        var exerciseController = new ExerciseController(exerciseService);

        while (true)
        {
            var choice = userInput.GetMenuOption();

            switch (choice)
            {
                case MenuOptions.AddExercise:
                    exerciseController.AddExercise();
                    break;
                case MenuOptions.UpdateExercise:
                    exerciseController.UpdateExercise();
                    break;
                case MenuOptions.DeleteExercise:
                    exerciseController.DeleteExercise();
                    break;
                case MenuOptions.ShowAllExercises:
                    exerciseController.GetAllExercises();
                    break;
                case MenuOptions.Exit:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

    }
}