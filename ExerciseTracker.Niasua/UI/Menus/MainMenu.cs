using ExerciseTracker.Niasua.Controllers;
using Spectre.Console;
using System.Data;

namespace ExerciseTracker.Niasua.UI.Menus;

public static class MainMenu
{
    public static async Task Show(ExerciseController controller)
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[blue]Exercise Tracker Menu[/]")
                .AddChoices(new[]
                {
                    "Add Exercise",
                    "Show Exercise",
                    "Show All Exercises",
                    "Edit Exercise",
                    "Remove Exercise",
                    "Exit"
                }));

            switch (option)
            {
                case "Add Exercise":

                    await controller.AddExerciseAsync();

                    Pause();

                    break;

                case "Show Exercise":

                    await controller.GetExerciseByIdAsync();

                    Pause();

                    break;

                case "Show All Exercises":

                    await controller.GetAllExercisesAsync();

                    Pause();

                    break;

                case "Edit Exercise":

                    await controller.UpdateExerciseAsync();

                    Pause();

                    break;

                case "Remove Exercise":

                    await controller.DeleteExerciseAsync();

                    Pause();

                    break;

                case "Exit":

                    exit = true;

                    break;
            }
        }
    }

    public static void Pause()
    {
        AnsiConsole.MarkupLine("\n[grey]Press any key to return...[/]");
        Console.ReadKey();
    }
}
