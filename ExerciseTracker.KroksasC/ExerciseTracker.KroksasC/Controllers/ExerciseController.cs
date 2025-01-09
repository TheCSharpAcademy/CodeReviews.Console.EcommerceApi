using ExerciseTracker.KroksasC.Services;
using Spectre.Console;

namespace ExerciseTracker.KroksasC.Controllers
{
    internal class ExerciseController
    {
        private readonly ExerciseService _exerciseService;

        // Constructor injection for ExerciseService
        public ExerciseController(ExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }
        public async Task Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Welcome to exercises logger!");

                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What option do you want to choose?")
                        .AddChoices("Create exercise", "View exercises", "Delete exercise", "Update exercise", "Exit")
                );

                switch (option)
                {
                    case "Create exercise":
                        await _exerciseService.AddExercise();
                        break;
                    case "View exercises":
                        await _exerciseService.ShowAllExercises();
                        break;
                    case "Delete exercise":
                        await _exerciseService.DeleteExercise();
                        break;
                    case "Update exercise":
                        await _exerciseService.UpdateExercise();
                        break;
                    case "Exit":
                        Console.Clear();
                        Console.WriteLine("Goodbye! Shutting down app..");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }
    }
}
