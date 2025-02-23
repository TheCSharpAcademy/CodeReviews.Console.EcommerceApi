using exerciseTracker.doc415.Service;
using Spectre.Console;
using static exerciseTracker.doc415.Enums;

namespace exerciseTracker.doc415.Controller;

internal class ExerciseController
{

    private ExerciseService _service;

    public ExerciseController()
    {
        _service = new();
    }

    public void MainMenu()
    {
        while (true)
        {
            AnsiConsole.Write(new FigletText("Exercise Logger").Color(Color.LightSalmon3_1).Centered());
            var selection = AnsiConsole.Prompt(new SelectionPrompt<MainMenuSelections>()
                                                    .Title("Main Menu")
                                                    .AddChoices(MainMenuSelections.AddExercise,
                                                                MainMenuSelections.ViewExercises,
                                                                MainMenuSelections.ViewExerciseById,
                                                                MainMenuSelections.UpdateExercise,
                                                                MainMenuSelections.DeleteExercise,
                                                                MainMenuSelections.Quit
                                                   ));
            switch (selection)
            {
                case MainMenuSelections.AddExercise:
                    AddExercise();
                    break;
                case MainMenuSelections.ViewExercises:
                    ViewExercises(true);
                    break;
                case MainMenuSelections.DeleteExercise:
                    DeleteExercise();
                    break;
                case MainMenuSelections.UpdateExercise:
                    UpdateExercise();
                    break;
                case MainMenuSelections.ViewExerciseById:
                    ViewExerciseById();
                    break;
                case MainMenuSelections.Quit:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    private void AddExercise()
    {
        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MaxValue;
        TimeSpan duration = TimeSpan.Zero;

        var type = UserInput.GetModelType();
        var dates = UserInput.GetDates();
        startDate = dates.Item1;
        endDate = dates.Item2;
        duration = endDate - startDate;
        string comments = UserInput.GetComments();

        try
        {
            _service.AddExercise(type, startDate, endDate, duration, comments);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was an error: {ex.Message}");
            EndOperation();
        }
    }

    private void ViewExercises(bool clear)
    {
        try
        {
            var table = new Table();
            table.AddColumns("Id", "Type", "Start Time", "End Time", "Duration (minutes)", "Comments");
            var exercises = _service.GetExerciseList();
            foreach (var exercise in exercises)
            {
                table.AddRow(exercise.Id.ToString(), exercise.Type, exercise.DateStart.ToString(), exercise.DateEnd.ToString(), exercise.Duration.TotalMinutes.ToString(), exercise.Comments);
            }
            AnsiConsole.Write(table);
            if (clear) EndOperation();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There is no avaible data: {ex.Message}");
            EndOperation();
            return;
        }
    }

    private void DeleteExercise()
    {
        ViewExercises(false);
        int id = UserInput.GetId("to delete");
        if (!AnsiConsole.Confirm("This will delete record permanently. Are you sure? ", false)) return;
        try
        {
            _service.DeleteExercise(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was an error: {ex.Message}");
            EndOperation();
        }
    }

    private void UpdateExercise()
    {
        ViewExercises(false);
        int id = UserInput.GetId("to update");

        try
        {

            DateTime startDate = DateTime.MinValue;
            DateTime endDate = DateTime.MaxValue;
            TimeSpan duration = TimeSpan.Zero;

            var type = UserInput.GetModelType();
            var dates = UserInput.GetDates();
            startDate = dates.Item1;
            endDate = dates.Item2;
            duration = endDate - startDate;
            string comments = UserInput.GetComments();


            _service.UpdateExercise(id, type, startDate, endDate, duration, comments);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was an error: {ex.Message}");
            EndOperation();
            return;
        }
    }
    private void ViewExerciseById()
    {
        try
        {
            int id = UserInput.GetId("to view");
            var exercise = _service.GetExerciseById(id);
            var table = new Table();
            table.AddColumns("Id", "Type", "Start Date", "End Date", "Duration (minutes)", "Comments");
            table.AddRow(exercise.Id.ToString(), exercise.Type, exercise.DateStart.ToString(), exercise.DateEnd.ToString(), exercise.Duration.Minutes.ToString(), exercise.Comments);
            AnsiConsole.Write(table);
            EndOperation();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There is no avaible data: {ex.Message}");
            EndOperation();
            return;
        }
    }

    private void EndOperation()
    {
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
        Console.Clear();
    }
}
