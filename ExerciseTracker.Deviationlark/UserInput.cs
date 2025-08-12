using Spectre.Console;

namespace ExerciseTracker;
public class UserInput : IUserInput
{
    public Exercise GetExerciseInfo()
    {
        var exercise = new Exercise();
        string startDate = AnsiConsole.Ask<string>("Enter the date you started the exercise(format: dd-MM-yyyy hh:mm:ss): ");
        while (!Validation.ValidateStartDate(startDate))
        {
            Console.WriteLine("Invalid date! Try again.");
            startDate = AnsiConsole.Ask<string>("Enter the date you started the exercise(format: dd-MM-yyyy hh:mm:ss): ");
        }

        string endDate = AnsiConsole.Ask<string>("Enter the date you ended the exercise(format: dd-MM-yyyy hh:mm:ss): ");
        while (!Validation.ValidateEndDate(endDate, startDate))
        {
            Console.WriteLine("Invalid date! Try again.");
            endDate = AnsiConsole.Ask<string>("Enter the date you ended the exercise(format: dd-MM-yyyy hh:mm:ss): ");
        }
        var duration = DateTime.Parse(endDate) - DateTime.Parse(startDate);
        var addComment = AnsiConsole.Confirm("Would you like to add a comment?");
        if (addComment)
        {
            exercise.Comments = AnsiConsole.Ask<string>("Comment: ");
        }
        else exercise.Comments = "No Comment";
        exercise.StartDate = DateTime.Parse(startDate);
        exercise.EndDate = DateTime.Parse(endDate);
        exercise.Duration = duration;
        return exercise;
    }

    public int GetExerciseId(string message, IEnumerable<Exercise> exercises)
    {
        string userInput = AnsiConsole.Ask<string>(message);
        while (!Validation.ValidateId(userInput, exercises))
        {
            Console.WriteLine("Invalid Id.");
            userInput = AnsiConsole.Ask<string>(message);
        } 
        return int.Parse(userInput);
    }
}