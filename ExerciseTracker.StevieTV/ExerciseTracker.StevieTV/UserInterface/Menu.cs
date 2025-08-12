using System.Globalization;
using ExerciseTracker.StevieTV.Controllers;
using ExerciseTracker.StevieTV.Helpers;
using ExerciseTracker.StevieTV.Models;
using Spectre.Console;

namespace ExerciseTracker.StevieTV.UserInterface;

public class Menu
{
    private readonly ExerciseController _exerciseController;

    public Menu(ExerciseController exerciseController)
    {
        _exerciseController = exerciseController;
    }

    public void MainMenu()
    {
        var exitMenu = false;

        while (!exitMenu)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Exercise Tracker"));

            var menuSelection = new SelectionPrompt<MainMenuOptions>();
            menuSelection.Title("Please choose an option");
            menuSelection.AddChoice(MainMenuOptions.ViewExercises);
            menuSelection.AddChoice(MainMenuOptions.AddExercise);
            menuSelection.AddChoice(MainMenuOptions.DeleteExercise);
            menuSelection.AddChoice(MainMenuOptions.Exit);
            menuSelection.UseConverter(option => option.GetEnumDescription());

            var selectedOption = AnsiConsole.Prompt(menuSelection);

            switch (selectedOption)
            {
                case MainMenuOptions.ViewExercises:
                    ViewExercises();
                    break;
                case MainMenuOptions.AddExercise:
                    AddExercise();
                    break;
                case MainMenuOptions.DeleteExercise:
                    DeleteExercise();
                    break;
                case MainMenuOptions.Exit:
                    exitMenu = true;
                    Environment.Exit(0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void ViewExercises()
    {
        var exercises = _exerciseController.GetExercises();
        var sortedExercises = exercises.OrderBy(x => x.DateStart).ToList();
        var table = new Table();
        table.AddColumns("Start", "End", "Duration", "Comment");

        foreach (var exercise in sortedExercises)
        {
            table.AddRow(
                exercise.DateStart.ToString("g"),
                exercise.DateEnd.ToString("g"),
                $"{exercise.Duration.Hours}h {exercise.Duration.Minutes:00}m",
                exercise.Comment
            );
        }

        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Exercises Logged"));
        AnsiConsole.Write(table);

        if (!AnsiConsole.Prompt(new ConfirmationPrompt("Press enter to continue")))
            Environment.Exit(0);
    }

    private void AddExercise()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(new FigletText("Add an Exercise")
            .Color(Color.Green));

        var date = UserInput.GetDate();
        var startTime = UserInput.GetTime(true);
        var endTime = UserInput.GetTime(false, DateTime.ParseExact(startTime, @"H\:m", CultureInfo.InvariantCulture));
        var dateStart = DateTime.ParseExact($"{date} {startTime}", "dd/MM/yy HH:mm", CultureInfo.InvariantCulture);
        var dateEnd = DateTime.ParseExact($"{date} {endTime}", "dd/MM/yy HH:mm", CultureInfo.InvariantCulture);
        var duration = dateEnd - dateStart;
        var comment = UserInput.GetComment();


        var exercise = new Exercise
        {
            DateStart = dateStart,
            DateEnd = dateEnd,
            Duration = duration,
            Comment = comment
        };

        var result = _exerciseController.AddExercise(exercise);

        if (!result)
        {
            AnsiConsole.Prompt(new ConfirmationPrompt("Unable to record this exercise. Press enter to continue"));
        }
        else if (!AnsiConsole.Prompt(new ConfirmationPrompt("Exercise Added - Do you wish to do more?")))
        {
            Environment.Exit(0);
        }
    }
    
    private void DeleteExercise()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("Delete an Exercise")
            .Color(Color.Red));
     
        var selectedExercise = SelectExercise();
     
        if (selectedExercise.Id == 0) return;
             
        var result = _exerciseController.RemoveExercise(selectedExercise);
             
        if (!result)
        {
            AnsiConsole.Prompt(new ConfirmationPrompt("Unable to delete this exercise. Press enter to continue"));
        }
        else if (!AnsiConsole.Prompt(new ConfirmationPrompt("Exercise Deleted - Do you wish to do more?")))
        {
            Environment.Exit(0);
        }
    }
    
    private Exercise SelectExercise()
    {
        var exercises = _exerciseController.GetExercises();
        var sortedExercises = exercises.OrderBy(x => x.DateStart).ToList();
        
        var selectOptions = new SelectionPrompt<Exercise>();
        selectOptions.AddChoice(new Exercise() {Id = 0});
        selectOptions.AddChoices(sortedExercises);
        selectOptions.UseConverter(exercise => (exercise.Id == 0 ? "CANCEL" : $"On {exercise.DateStart.ToShortDateString()} @ {exercise.DateStart.ToShortTimeString()} for {exercise.Duration.ToString()}- ({exercise.Comment})"));

        var selectedExercise = AnsiConsole.Prompt(selectOptions);

        return selectedExercise;
    }
}