using ExerciseTracker.hasona23.Enums;
using ExerciseTracker.hasona23.Handlers;
using ExerciseTracker.hasona23.Services;
using Spectre.Console;

namespace ExerciseTracker.hasona23.Controllers;

public class ExerciseController(ExerciseService exerciseService,InputHandler inputHandler)
{
    private void AddExercise()
    {
        var newExercise = inputHandler.CreateExercise();
        exerciseService.AddExercise(newExercise);
    }

    private void UpdateExercise()
    {
        var updatedExercise = inputHandler.UpdateExercise(exerciseService.GetAllExercises());
        bool success = exerciseService.UpdateExercise(updatedExercise);
        AnsiConsole.MarkupLine($"{(success?"[green]Updated Exercise Successfully[/]":"[red]Couldn't Update Exercise[/]")}");
    }

    private void DeleteExercise()
    {
        int deletedId = inputHandler.SelectExercise(exerciseService.GetAllExercises()).Id;
        bool success = exerciseService.DeleteExercise(deletedId);
        AnsiConsole.MarkupLine($"{(success?"[green]Deleted Exercise Successfully[/]":"[red]Couldn't Delete Exercise[/]")}");
    }

    private void DisplayAllExercises()
    {
        VisualisationHandler.DisplayExercisesTable(exerciseService.GetAllExercises());
    }
    
    public void HandleExercises()
    {
        switch (MenuBuilder.GetExerciseOption())
        {
            case ExerciseOptions.Add:
                AddExercise();
                break;
            case ExerciseOptions.Delete:
                DeleteExercise();
                break;
            case ExerciseOptions.Update:
                UpdateExercise();
                break;
            case ExerciseOptions.ReadAll:
                DisplayAllExercises();
                break;
            case ExerciseOptions.Return:
                break;
        }
    }
}