using Spectre.Console;
using static ExerciseTracker.Models.Enums;

namespace ExerciseTracker.Input;

public class UserInput : IInput
{
    public string GetComments()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Ask<string>("Add comments: ");
    }

    public (DateTime, DateTime) GetDates()
    {
        AnsiConsole.Clear();
        DateTime start;
        DateTime end;
        bool valid = false;

        do
        {
            start = GetDate("start");
            end = GetDate("end");
            if (end > start)
            {
                valid = true;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Start time can't be after end time...[/]");
            }
        } while (!valid);

        return (start, end);
    }


    static public DateTime GetDate(string type)
    {
        DateTime result;
        bool valid;
        do
        {
            string time = AnsiConsole.Ask<string>($"Enter {type} time of exercise (dd-mm-yyyy hh:mm): ");
            if (DateTime.TryParse(time, out result))
            {
                valid = true;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Please enter a valid date-time...[/]");
                valid = false;
            }
        } while (!valid);

        return result;
    }

    public int GetId(string type)
    {
        return AnsiConsole.Ask<int>($"Which exercise do you want to {type.Trim().ToLower()}? ");
    }

    public MainMenuOption GetMenuOption()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(new SelectionPrompt<MainMenuOption>()
            .Title("Main Menu")
            .AddChoices(MainMenuOption.ShowAllExercises,
                        MainMenuOption.Add,
                        MainMenuOption.Update,
                        MainMenuOption.Remove,
                        MainMenuOption.Quit
            ));
    }

    ExerciseType IInput.GetType()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(new SelectionPrompt<ExerciseType>()
            .Title("Exercise Types")
            .AddChoices(ExerciseType.Cardio,
                        ExerciseType.WeightLifting
            ));
    }
}