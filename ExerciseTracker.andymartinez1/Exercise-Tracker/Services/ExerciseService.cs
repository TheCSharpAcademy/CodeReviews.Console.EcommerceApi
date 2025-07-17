using Exercise_Tracker.Models;
using Exercise_Tracker.Repository;
using Exercise_Tracker.Utils;
using Spectre.Console;

namespace Exercise_Tracker.Services;

public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository<Exercise> _repository;

    public ExerciseService(IExerciseRepository<Exercise> repository)
    {
        _repository = repository;
    }

    public List<Exercise> GetAllExercises()
    {
        return _repository.GetAllExercises();
    }

    public Exercise GetExerciseById(int id)
    {
        return _repository.GetExerciseById(id);
    }

    public void AddExercise()
    {
        var dates = Helpers.GetDates();

        var exercise = new Exercise
        {
            StartTime = dates[0],
            EndTime = dates[1],
            Comments = AnsiConsole.Ask<string>("Enter Exercise Comments: "),
        };

        _repository.AddExercise(exercise);
    }

    public void UpdateExercise(Exercise exercise)
    {
        var updateStartTime = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Would you like to update the exercise start and end time?")
                .AddChoices("Yes", "No")
        );
        if (updateStartTime == "Yes")
        {
            var dates = Helpers.GetDates();
            exercise.StartTime = dates[0];
            exercise.EndTime = dates[1];
        }
        else
        {
            exercise.StartTime = exercise.StartTime;
            exercise.EndTime = exercise.EndTime;
        }

        var updateComments = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Would you like to update the exercise comments? ")
                .AddChoices("Yes", "No")
        );
        if (updateComments == "Yes")
            exercise.Comments = AnsiConsole.Ask<string>("Enter Exercise Comments: ");
        else
            exercise.Comments = exercise.Comments;

        _repository.UpdateExercise(exercise);
    }

    public void DeleteExercise(int id)
    {
        _repository.DeleteExercise(id);

        AnsiConsole.Clear();
        AnsiConsole.MarkupLine($"[green]Exercise {id} deleted successfully![/]");
    }
}
