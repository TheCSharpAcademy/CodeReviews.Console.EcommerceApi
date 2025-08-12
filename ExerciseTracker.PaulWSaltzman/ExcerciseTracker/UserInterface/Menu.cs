using ExerciseTracker.Controllers;
using ExerciseTracker.Models;
using Spectre.Console;
using static ExerciseTracker.Models.Enums;

namespace ExerciseTracker.UserInterface;

internal class Menu
{
    private readonly ExerciseController _exerciseController;

    public Menu(ExerciseController exerciseController)
    {
        _exerciseController = exerciseController;
    }

    internal void MainMenu()
    {
        bool exitProgram = false;
        while (!exitProgram)
        {
            ShowTitle();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<MainMenuOptions>()
                .Title("What would you like to do?")
                .AddChoices(
                    MainMenuOptions.AddExerciseSession,
                    MainMenuOptions.ExerciseSessionHistory,
                    MainMenuOptions.ExitProgram));

            switch (option)
            {
                case MainMenuOptions.AddExerciseSession:
                    AddExerciseMenu();
                    break;
                case MainMenuOptions.ExerciseSessionHistory:
                    ExerciseSessionHistory();
                    break;
                case MainMenuOptions.ExitProgram:
                    exitProgram = true;
                    break;
            }
        }
    }

    private void AddExerciseMenu()
    {
        Exercise exercise = UserInput.CreateExerciseSession();
        exercise = _exerciseController.AddExercise(exercise);
        ShowExercise(exercise);
        Console.WriteLine("Exercise created! Press any key to continue.");
        Console.ReadKey();
    }

    private void ExerciseSessionHistory()
    {
        var exitMenu = false;

        while (!exitMenu)
        {
            List<Exercise> exercises = _exerciseController.GetAllExercises();

            Console.Clear();

            var table = new Spectre.Console.Table()
                .AddColumns("ID", "Start", "End", "Duration", "Comments");

            foreach (Exercise session in exercises)
            {
                table.AddRow($"{session.Id}",
                    $"{session.DateStart}",
                    $"{session.DateEnd}",
                    $"{session.Duration}",
                    $"{session.Comments}");
            }
            AnsiConsole.Write(table);

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<CreateViewUpdateDeleteMenuOptions>()
                .Title("What would you like to do?")
                .AddChoices(
                    CreateViewUpdateDeleteMenuOptions.AddExerciseSession,
                    CreateViewUpdateDeleteMenuOptions.ViewSession,
                    CreateViewUpdateDeleteMenuOptions.UpdateSession,
                    CreateViewUpdateDeleteMenuOptions.DeleteSession,
                    CreateViewUpdateDeleteMenuOptions.Back));

            switch (option)
            {
                case CreateViewUpdateDeleteMenuOptions.AddExerciseSession:
                    AddExerciseMenu();
                    break;
                case CreateViewUpdateDeleteMenuOptions.ViewSession:
                    Exercise exercise = UserInput.GetSingleSession(exercises);
                    ShowExercise(exercise);
                    break;
                case CreateViewUpdateDeleteMenuOptions.UpdateSession:
                    exercise = UserInput.GetSingleSession(exercises);
                    UserInput.UpdateSession(exercise);
                    _exerciseController.UpdateExercise(exercise);
                    ShowExercise(exercise);
                    break;
                case CreateViewUpdateDeleteMenuOptions.DeleteSession:
                    exercise = UserInput.GetSingleSession(exercises);
                    _exerciseController.DeleteExercise(exercise);
                    break;
                case CreateViewUpdateDeleteMenuOptions.Back:
                    exitMenu = true;
                    break;
            }
        }
    }

    private static void ShowTitle()
    {
        Console.Clear();
        AnsiConsole.Write(
            new FigletText("Exercise Tracker")
            .LeftJustified());
    }

    private static void ShowExercise(Exercise exercise)
    {
        Console.Clear();

        var table = new Spectre.Console.Table()
            .AddColumn("Exercise")
            .AddRow(new Panel($"ID: {exercise.Id}"))
            .AddRow(new Panel($"Start: {exercise.DateStart}"))
            .AddRow(new Panel($"End: {exercise.DateEnd}"))
            .AddRow(new Panel($"Duration: {exercise.Duration}"))
            .AddRow(new Panel($"Comments: {exercise.Comments}"));

        AnsiConsole.Write(table);
        Console.ReadKey();
    }
}

