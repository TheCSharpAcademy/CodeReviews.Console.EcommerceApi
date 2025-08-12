using ExerciseTracker.hasona23;
using ExerciseTracker.hasona23.Controllers;
using ExerciseTracker.hasona23.Enums;
using ExerciseTracker.hasona23.Handlers;
using ExerciseTracker.hasona23.Repository;
using ExerciseTracker.hasona23.Services;
using Spectre.Console;

bool isRunning = true;
FigletText figlet = new FigletText("Exercise Tracker").Color(Color.Yellow);
figlet.Justify(Justify.Center);
AnsiConsole.Write(figlet);
ExerciseRepository exerciseRepository = new ExerciseRepository();
ExerciseService service = new ExerciseService(exerciseRepository);
ExerciseController exerciseController = new ExerciseController(service,new InputHandler());
MenuBuilder.Pause();

while (isRunning)
{
    Console.Clear();
    switch (MenuBuilder.GetOptions())
    {
        case Options.Exercises:
            exerciseController.HandleExercises();
            break;
        case Options.Help:
            MenuBuilder.ShowHelpMenu();
            break;
        case Options.Exit:
            AnsiConsole.MarkupLine("[bold yellow]Thanks for using Exercise Tracker![/]");
            isRunning = false;
            break;
    }
}


