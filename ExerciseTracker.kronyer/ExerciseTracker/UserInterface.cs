using ExerciseTracker.Controllers;
using ExerciseTracker.Models;
using ExerciseTracker.Repositories;
using ExerciseTracker.Repositories.Interfaces;
using ExerciseTracker.Services;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace ExerciseTracker;

internal class UserInterface
{
    enum MenuOptions
    {
        AddExercise,
        GetExercise,
        GetAllExercises,
        UpdateExercise,
        DeleteExercise,
        Quit
    }
    public static void MainMenu()
    {
        Console.Clear();
        var serviceProvider = new ServiceCollection()
            .AddSingleton<ExerciseContext>()
            .AddSingleton<IExerciseRepository, ExerciseRepository>()
            //aqui é a chave da parada --> indentificação de que o IRepository é o Exercise Repository
            .AddSingleton<ExerciseService>()
            .AddSingleton<ExerciseController>()
            .BuildServiceProvider();

        var controller = serviceProvider.GetRequiredService<ExerciseController>();

        bool isRunning = true;
        while (isRunning)
        {
            var option = AnsiConsole.Prompt(new SelectionPrompt<MenuOptions>()
                .Title("What do you want to do?")
                .AddChoices(MenuOptions.AddExercise,
                MenuOptions.GetExercise,
                MenuOptions.GetAllExercises,
                MenuOptions.UpdateExercise,
                MenuOptions.DeleteExercise,
                MenuOptions.Quit));

            switch (option)
            {
                case MenuOptions.AddExercise:
                    controller.AddExercise();
                    break;
                case MenuOptions.UpdateExercise:
                    controller.Update();
                    break;
                case MenuOptions.DeleteExercise:
                    controller.Delete();
                    break;
                case MenuOptions.GetExercise:
                    PrintExercise(controller.Get());
                    break;
                case MenuOptions.GetAllExercises:
                    PrintAllExercises(controller.GetAll());
                    break;
                case MenuOptions.Quit:
                    isRunning = false;
                    break;
            }
        }
    }

    public static void PrintAllExercises(List<Exercise> exerciseList)
    {
        Console.Clear();
        int i = 1;
        foreach (var exercise in exerciseList)
        {
            Console.WriteLine($"{i} - {exercise}");
            i++;
        }
        Console.ReadKey();
    }

    public static void PrintExercise(Exercise exercise)
    {
        Console.Clear();
        if (exercise == null)
        {
            Console.WriteLine("There is no register in this id");
        }
        else
        {
            Console.WriteLine(exercise);
        }

        Console.ReadKey();
    }
}
