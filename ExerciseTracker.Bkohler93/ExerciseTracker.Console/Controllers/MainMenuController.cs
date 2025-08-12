using Data.Entities;
using ExerciseTracker.Services;
using Microsoft.IdentityModel.Tokens;

namespace ExerciseTracker.Controllers;

public class MainMenuController {
    private readonly ExerciseService Service;
    public static readonly string[] Options = ["View logged exercises", "Log an exercise", "Edit a logged exercise", "Delete exercise log"];
    private readonly Dictionary<string, Func<Task>> OptionHandlers;
    public MainMenuController(ExerciseService service)
    {
        Service = service; 
        OptionHandlers = new Dictionary<string, Func<Task>>{
            { Options[0], ViewExercises},
            { Options[1], LogExercise},
            { Options[2], EditExercise},
            { Options[3], DeleteExercise}
        };
    }

    public async Task HandleChoice(string choice)
    {
        OptionHandlers.TryGetValue(choice, out var action);
        await action!();
    }


    private async Task ViewExercises()
    {
        var exercises = await Service.GetAllExercisesAsync();

        if (exercises.IsNullOrEmpty())
        {
            UI.ConfirmationMessage("No exercises to view");
            return;
        }

        UI.DisplayExercises(exercises);
        UI.ConfirmationMessage("");
    }

    private async Task LogExercise()
    {
        var startDateTime = UI.DateTimeResponseWithDefault("Enter the starting date and time", DateTime.Today);
        var endDateTime = UI.DateTimeResponseWithDefault("Enter the ending date time", DateTime.Today);
        var duration = endDateTime - startDateTime;
        var comment = UI.StringResponse("Enter any comments for this log");

        await Service.CreateExerciseAsync(startDateTime, endDateTime, duration, comment);

        UI.ConfirmationMessage("Exercise created");
    }

    private async Task EditExercise()
    {
        var exercises = await Service.GetAllExercisesAsync();

        UI.DisplayExercises(exercises);

        Exercise? exercise = null;
        while (exercise == null)
        {
            var id = UI.IntResponse("Enter the [green]id[/] of the exercise you wish to edit");
            exercise = await Service.GetExerciseByIdAsync(id);
            if (exercise == null) {
                UI.InvalidationMessage("There is no exercise with that id");
            }
        }

        var startDateTime = UI.DateTimeResponseWithDefault("Enter the [green]start date time[/] for the exercise", exercise.DateStart);
        var endDateTime = UI.DateTimeResponseWithDefault("Enter the [green]end date time[/] for the exercise", exercise.DateEnd);
        var duration = endDateTime - startDateTime;
        var comments = UI.StringResponseWithDefault("Enter the [green]comments[/] for the exercise", exercise.Comments ?? "");

        await Service.UpdateExerciseAsync(exercise.Id, startDateTime, endDateTime, duration, comments);

    }

    private async Task DeleteExercise()
    {
        var exercises = await Service.GetAllExercisesAsync();

        UI.DisplayExercises(exercises);

        Exercise? exercise = null;
        while (exercise == null)
        {
            var id = UI.IntResponse("Enter the [green]id[/] of the exercise you wish to delete");
            exercise = await Service.GetExerciseByIdAsync(id);
            if (exercise == null) {
                UI.InvalidationMessage("There is no exercise with that id");
            }
        }

        await Service.DeleteExerciseAsync(exercise);
    }
}