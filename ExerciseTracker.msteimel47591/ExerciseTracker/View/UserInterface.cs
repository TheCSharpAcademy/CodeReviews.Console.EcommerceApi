using ExerciseTracker.Controller;
using ExerciseTracker.Models;
using Spectre.Console;

namespace ExerciseTracker.View;

internal class UserInterface
{
    public static void DisplayMainMenu()
    {
        while (true)
        {
            Helpers.PrintHeader();
            AnsiConsole.MarkupLine("[blue]Main Menu[/]");
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(10)
                    .AddChoices("Add Exercise", "View Exercises", "Delete Exercise", "Update Exercise", "Exit")
            );

            switch (choice)
            {
                case "Add Exercise":
                    ExerciseController.AddExercise();
                    break;
                case "View Exercises":
                    ExerciseController.ViewExercises();
                    break;
                case "Delete Exercise":
                    ExerciseController.DeleteExercise();
                    break;
                case "Update Exercise":
                    ExerciseController.UpdateExercise();
                    break;
                case "Exit":
                    Environment.Exit(0);
                    break;
            } 
        }
    }

    internal static void PrintExercises(List<Exercise> exercises)
    {
        Helpers.PrintHeader();

        Table table = new Table();

        table.AddColumn("ID");
        table.AddColumn("Start Date");
        table.AddColumn("End Date");
        table.AddColumn("Duration");
        table.AddColumn("Comment");

        foreach (var exercise in exercises)
        {
            table.AddRow(exercise.Id.ToString(), exercise.DateStart.ToString(), exercise.DateEnd.ToString(), exercise.Duration.ToString(), exercise.Comment);
        }

        AnsiConsole.Write(table);

    }
}
