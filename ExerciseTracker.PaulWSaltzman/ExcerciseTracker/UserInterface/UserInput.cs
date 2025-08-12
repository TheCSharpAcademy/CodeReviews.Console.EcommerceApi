using ExerciseTracker.Models;
using Spectre.Console;

namespace ExerciseTracker.UserInterface;

internal class UserInput
{

    internal static Exercise CreateExerciseSession()
    {
        var datesValid = false;

        Exercise exercise = new Exercise();

        Console.WriteLine("Please enter the start date time.");
        exercise.DateStart = DTValidEntry.GetDateTime();

        Console.WriteLine("Please enter the end time.");
        exercise.DateEnd = DTValidEntry.GetEndDateTime(exercise.DateStart);

        exercise.Comments = AnsiConsole.Ask<string>("Please enter comments about this exercise session.");


        while (!datesValid)
        {
            datesValid = Validators.DateValidator(exercise);
            if (!datesValid)
            {
                exercise = UpdateSession(exercise);
            }
        }

        if (exercise.Duration == TimeSpan.Zero)
        {
            exercise = Validators.CalculateSessionLength(exercise);
        }

        return exercise;
    }

    internal static Exercise GetSingleSession(List<Exercise> exercises)
    {
        int exerxciseId = AnsiConsole.Prompt(
            new TextPrompt<int>("Session Id")
            .ValidationErrorMessage("[red] That is not a valid Id.[/]")
            .Validate(exerxciseId =>
            {
                if (!exercises.Any(exercise => exercise.Id == exerxciseId))
                {
                    return ValidationResult.Error("[red]There is no session with that Id.[/]");
                }
                else
                {
                    return ValidationResult.Success();
                }

            }));

        var exercise = exercises.FirstOrDefault(ex => ex.Id == exerxciseId);
        return exercise;
    }

    internal static Exercise UpdateSession(Exercise exerciseToUpdate)
    {
        var exitMenu = false;
        while (!exitMenu)
        {
            exerciseToUpdate.DateStart = AnsiConsole.Confirm("Change the start time?")
                ? exerciseToUpdate.DateStart = DTValidEntry.GetDateTime()
                : exerciseToUpdate.DateStart;

            exerciseToUpdate.DateEnd = AnsiConsole.Confirm("Change the end time?")
                ? exerciseToUpdate.DateEnd = DTValidEntry.GetDateTime()
                : exerciseToUpdate.DateEnd;

            exerciseToUpdate.Comments = AnsiConsole.Confirm("Change the comments?")
                ? exerciseToUpdate.Comments = AnsiConsole.Ask<string>("Please enter comments about this exercise session.")
                : exerciseToUpdate.Comments;

            exitMenu = Validators.DateValidator(exerciseToUpdate);
        }
        exerciseToUpdate = Validators.CalculateSessionLength(exerciseToUpdate);

        return exerciseToUpdate;
    }
}