using ConsoleTableExt;
using ExerciseTracker.DTOs;
using ExerciseTracker.Interfaces;
using ExerciseTracker.Models;
using Spectre.Console;

namespace ExerciseTracker;

internal class ExerciseService : IExerciseService
{
    private readonly UserInput _userInput;
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseService(UserInput userInput, IExerciseRepository exerciseRepository)
    {
        _userInput = userInput;
        _exerciseRepository = exerciseRepository;
    }
    public Exercise CreateNewExercise()
    {
        var startDate = this._userInput.GetDate("start");
        var startTime = this._userInput.GetTime("start");
        var startDateTime = startDate.ToDateTime(startTime);

        Console.Clear();

        var endDate = this._userInput.GetDate("end");
        var endTime = this._userInput.GetTime("end");
        var endDateTime = endDate.ToDateTime(endTime);

        var duration = endDateTime - startDateTime;

        if (duration.TotalSeconds < 0) throw new Microsoft.EntityFrameworkCore.DbUpdateException();

        Console.Clear();

        var comments = this._userInput.GetComments();

        var exercise = new Exercise
        {
            StartDate = startDateTime,
            EndDate = endDateTime,
            Duration = duration,
            Comments = comments
        };

        return exercise;
    }

    public void ViewAllExercises()
    {
        var exercises = this._exerciseRepository.GetAllExercises();
        var exerciseDTOs = exercises.Select(exercise =>
            new ExerciseDTO
            {
                StartDate = exercise.StartDate.ToString("MM-dd-yyyy hh:mm tt"),
                EndDate = exercise.EndDate.ToString("MM-dd-yyyy hh:mm tt"),
                Duration = exercise.Duration,
                Comments = exercise.Comments
            }
        ).ToList();


        if (exercises.Count == 0)
        {
            Console.WriteLine("No exercises found.\n");
        }
        else
        {
            ConsoleTableBuilder.From(exerciseDTOs).WithTitle("EXERCISE HISTORY").ExportAndWriteLine();
        }
    }

    public Exercise? GetExercise()
    {
        var exercises = this._exerciseRepository.GetAllExercises();
        if (exercises.Count == 0)
        {
            Console.WriteLine("No exercises found.\n");
            return null;
        }

        var exerciseStrings = exercises
            .Select(exercise => $"{exercise.Id}. {exercise.StartDate} - {exercise.EndDate} ({exercise.Duration}) | {exercise.Comments}")
            .ToList();

        var exerciseId = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose exercise")
            .AddChoices(exerciseStrings)
            ).Split(".")[0];

        var exercise = this._exerciseRepository.GetExercise(int.Parse(exerciseId));

        return exercise;
    }

    public void UpdateExercise(Exercise exercise)
    {
        Console.WriteLine(exercise.StartDate.ToString());
        var updatedStart = AnsiConsole.Confirm("Update start date?") ?
            this._userInput.GetDate("start").ToDateTime(this._userInput.GetTime("start")) : exercise.StartDate;

        Console.WriteLine(exercise.EndDate.ToString());
        var updatedEnd = AnsiConsole.Confirm("Update end date?") ?
            this._userInput.GetDate("end").ToDateTime(this._userInput.GetTime("end")) : exercise.EndDate;

        var updatedDuration = updatedEnd - updatedStart;

        if (updatedDuration.TotalSeconds < 0) throw new Microsoft.EntityFrameworkCore.DbUpdateException();

        Console.WriteLine(exercise.Comments);
        var updatedComments = AnsiConsole.Confirm("Update comments?") ? this._userInput.GetComments() : exercise.Comments;

        exercise.StartDate = updatedStart;
        exercise.EndDate = updatedEnd;
        exercise.Duration = updatedDuration;
        exercise.Comments = updatedComments;

        this._exerciseRepository.UpdateExercise(exercise);
    }

    public void DeleteExercise(Exercise exercise)
    {
        try
        {
            this._exerciseRepository.DeleteExercise(exercise);
            Console.WriteLine("Exercise successfully deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Something went wrong.");
        }
    }
}
