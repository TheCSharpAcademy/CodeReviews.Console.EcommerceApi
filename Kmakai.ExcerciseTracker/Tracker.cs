using Kmakai.ExerciseTracker.Controllers;
using Spectre.Console;

namespace Kmakai.ExerciseTracker;

public class Tracker
{
    private readonly IExerciseController ExerciseController;

    public Tracker(IExerciseController exerciseController)
    {
        ExerciseController = exerciseController;
    }

    public void Run()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choice = UserInput.GetChoice();

            switch (choice)
            {
                case 1:
                    ExerciseController.AddExercise();
                    break;
                case 2:
                    ExerciseController.UpdateExercise();
                    break;
                case 3:
                    ExerciseController.DeleteExercise();
                    break;
                case 4:
                    ExerciseController.GetExercises();
                    break;
                case 0:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }

        }
    }
}
