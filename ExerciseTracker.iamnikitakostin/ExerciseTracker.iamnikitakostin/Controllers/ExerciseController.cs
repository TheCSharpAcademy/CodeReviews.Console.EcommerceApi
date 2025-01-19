using ExerciseTracker.iamnikitakostin.Models;
using ExerciseTracker.iamnikitakostin.Services;
using Spectre.Console;

namespace ExerciseTracker.iamnikitakostin.Controllers;

internal class ExerciseController : ConsoleHelper
{
    private readonly ExerciseService _service;

    public ExerciseController(ExerciseService service)
    {
        _service = service;
    }
    public void MainMenu()
    {
        while (true)
        {
            AnsiConsole.Clear();

            var menuOptions = EnumToDisplayNames<Enums.MainMenu>();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<Enums.MainMenu>()
                .Title("What do you want to do next?")
                .AddChoices(menuOptions.Keys)
                .UseConverter(option => menuOptions[option]));

            AnsiConsole.Clear();

            switch (choice)
            {
                case Enums.MainMenu.AllExercises:
                    ShowAllExercises();
                    break;
                case Enums.MainMenu.AddExercise:
                    AddExercise();
                    break;
                case Enums.MainMenu.UpdateExercise:
                    UpdateExercise();
                    break;
                case Enums.MainMenu.DeleteExercise:
                    DeleteExercise();
                    break;
                default:
                    return;
            }
        }
    }

    internal void ShowAllExercises()
    {
        List<Exercise>? exercises = _service.GetAll();

        if (exercises.Count == 0)
        {
            ErrorMessage("There are no exercises yet, please add some first.");
            return;
        }

        Table table = new Table();
        table.AddColumns("Id", "Start Time", "End Time", "Duration", "Comments");

        foreach (Exercise e in exercises)
        {
            table.AddRow(e.Id.ToString(), e.DateStart.ToString(), e.DateEnd.ToString(), e.Duration.ToString(), e.Comments);
        }

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine("Please, press any key to continue...");
        AnsiConsole.Console.Input.ReadKey(false);
    }

    internal void UpdateExercise()
    {
        Dictionary<int, string> exercises = _service.GetAllAsDictionary();

        int exerciseId = AnsiConsole.Prompt(
           new SelectionPrompt<int>()
           .Title("Select an exercise: ")
           .AddChoices(exercises.Keys)
           .UseConverter(option => exercises[option]));

        var menuOptions = EnumToDisplayNames<Enums.UpdateExerciseMenu>();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<Enums.UpdateExerciseMenu>()
            .Title("What do you want to do next?")
            .AddChoices(menuOptions.Keys)
            .UseConverter(option => menuOptions[option]));

        Exercise exercise = _service.GetById(exerciseId);

        switch (choice)
        {
            case Enums.UpdateExerciseMenu.Comments:
                exercise.Comments = AnsiConsole.Prompt(new TextPrompt<string>("Enter any comments you might have."));

                break;
            case Enums.UpdateExerciseMenu.StartDate:
                string dateStart;
                DateTime parsedDateStart;

                do
                {
                    dateStart = AnsiConsole.Prompt(new TextPrompt<string>("Enter the date when you ended the exercise, in the format MM/dd/yyyy HH:mm"));
                } while (!DateTime.TryParseExact(dateStart, "MM/dd/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out parsedDateStart));

                break;
            case Enums.UpdateExerciseMenu.EndDate:
                string dateEnd;
                DateTime parsedDateEnd;

                do
                {
                    dateEnd = AnsiConsole.Prompt(new TextPrompt<string>("Enter the date when you ended the exercise, in the format MM/dd/yyyy HH:mm"));
                } while (!DateTime.TryParseExact(dateEnd, "MM/dd/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out parsedDateEnd));

                break;
            default:
                return;
        }

        _service.Update(exercise);
    }

    internal void DeleteExercise()
    {
        Dictionary<int, string> exercises = _service.GetAllAsDictionary();

        int exerciseId = AnsiConsole.Prompt(
           new SelectionPrompt<int>()
           .Title("Select an exercise to delete: ")
           .AddChoices(exercises.Keys)
           .UseConverter(option => exercises[option]));

        Exercise exercise = _service.GetById(exerciseId);

        if (exercise != null)
        {
            bool confirmed = ConfirmDeletion("exercise");

            if (confirmed)
            {
                _service.Delete(exercise.Id);
            }
        }
    }

    internal void AddExercise()
    {
        Exercise exercise = new Exercise();
        string dateStart, dateEnd;
        DateTime parsedDateStart, parsedDateEnd;

        do
        {
            dateStart = AnsiConsole.Prompt(new TextPrompt<string>("Enter the date when you started the exercise, in the format MM/dd/yyyy HH:mm"));
        } while (!DateTime.TryParseExact(dateStart, "MM/dd/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out parsedDateStart));

        do
        {
            dateEnd = AnsiConsole.Prompt(new TextPrompt<string>("Enter the date when you ended the exercise, in the format MM/dd/yyyy HH:mm"));
        } while (!DateTime.TryParseExact(dateEnd, "MM/dd/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out parsedDateEnd));

        if (parsedDateStart > parsedDateEnd)
        {
            ErrorMessage("The end date cannot be before the start date, press any button to continue...");
            do
            {
                dateEnd = AnsiConsole.Prompt(new TextPrompt<string>("Enter the date when you ended the exercise, in the format MM/dd/yyyy HH:mm"));
            } while (!DateTime.TryParseExact(dateEnd, "MM/dd/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out parsedDateEnd));
        }

        string comments = AnsiConsole.Prompt(new TextPrompt<string>("Enter any comments you might have."));

        exercise.DateStart = parsedDateStart;
        exercise.DateEnd = parsedDateEnd;
        exercise.Duration = parsedDateEnd - parsedDateStart;
        exercise.Comments = comments;

        _service.Add(exercise);
    }

    internal static Dictionary<TEnum, string> EnumToDisplayNames<TEnum>() where TEnum : struct, Enum
    {
        return Enum.GetValues(typeof(TEnum))
            .Cast<TEnum>()
            .ToDictionary(
                value => value,
                value => SplitCamelCase(value.ToString())
            );
    }

    internal static string SplitCamelCase(string input)
    {
        return string.Join(" ", System.Text.RegularExpressions.Regex
            .Split(input, @"(?<!^)(?=[A-Z])"));
    }
}