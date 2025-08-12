using ExerciseTracker.Dejmenek.Data.Interfaces;
using ExerciseTracker.Dejmenek.Helpers;
using ExerciseTracker.Dejmenek.Models;
using Spectre.Console;

namespace ExerciseTracker.Dejmenek.Services;
public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUserInteractionService _userInteractionService;

    public ExerciseService(IExerciseRepository exerciseRepository, IUserInteractionService userInteractionService)
    {
        _exerciseRepository = exerciseRepository;
        _userInteractionService = userInteractionService;
    }

    public void AddExercise()
    {
        string? comments = null;
        string startTime = _userInteractionService.GetDateTime();
        string endTime = _userInteractionService.GetDateTime();

        DateTime startDateTime = DateTime.Parse(startTime);
        DateTime endDateTime = DateTime.Parse(endTime);

        while (!Validation.IsChronologicalOrder(startDateTime, endDateTime))
        {
            AnsiConsole.MarkupLine("The ending time should always be after the starting time. Try again.");
            startTime = _userInteractionService.GetDateTime();
            endTime = _userInteractionService.GetDateTime();

            startDateTime = DateTime.Parse(startTime);
            endDateTime = DateTime.Parse(endTime);
        }

        if (_userInteractionService.GetConfirmation("Would you like to add comments to the exercise?"))
        {
            comments = _userInteractionService.GetComment();
        }

        Exercise exercise = new Exercise
        {
            StartTime = startDateTime,
            EndTime = endDateTime,
            Duration = CalculateDuration(startDateTime, endDateTime),
            Comments = comments
        };

        _exerciseRepository.AddExercise(exercise);
    }

    public string CalculateDuration(DateTime startTime, DateTime endTime)
    {
        TimeSpan duration = endTime.Subtract(startTime);

        return duration.TotalMinutes.ToString();
    }

    public void DeleteExercise()
    {
        Exercise? selectedExercise = GetExercise();

        if (selectedExercise is null)
        {
            AnsiConsole.MarkupLine("There are no exercises.");
            return;
        }

        _exerciseRepository.DeleteExercise(selectedExercise.Id);
    }

    private Exercise? GetExercise()
    {
        List<Exercise> exercises = _exerciseRepository.GetExercises();

        if (exercises.Count == 0)
        {
            return null;
        }

        Exercise exercise = _userInteractionService.GetExercise(exercises);

        return exercise;
    }

    public List<ExerciseReadDTO> GetExercises()
    {
        var exercises = _exerciseRepository.GetExercises();

        if (exercises.Count == 0)
        {
            return [];
        }

        return Mapper.ToExerciseReadDtos(exercises);
    }

    public void UpdateExercise()
    {
        Exercise? selectedExercise = GetExercise();

        if (selectedExercise is null)
        {
            AnsiConsole.MarkupLine("There are no exercises.");
            return;
        }

        var oldExercise = _exerciseRepository.GetExerciseById(selectedExercise.Id);
        ExerciseUpdateDTO updatedExercise = new ExerciseUpdateDTO
        {
            EndTime = oldExercise.EndTime,
            Duration = oldExercise.Duration,
            Comments = oldExercise.Comments,
        };

        if (_userInteractionService.GetConfirmation("Would you like to update end time of the exercise?"))
        {
            string endTime = _userInteractionService.GetDateTime();
            DateTime endDateTime = DateTime.Parse(endTime);

            while (!Validation.IsChronologicalOrder(oldExercise.StartTime, endDateTime))
            {
                AnsiConsole.MarkupLine("The ending time should always be after the starting time. Try again.");

                endTime = _userInteractionService.GetDateTime();
                endDateTime = DateTime.Parse(endTime);
            }

            updatedExercise.EndTime = endDateTime;
            updatedExercise.Duration = CalculateDuration(oldExercise.StartTime, endDateTime);
        }

        if (_userInteractionService.GetConfirmation("Would you like to upadate comments?"))
        {
            updatedExercise.Comments = _userInteractionService.GetComment();
        }

        _exerciseRepository.UpdateExercise(oldExercise.Id, updatedExercise);
    }
}
