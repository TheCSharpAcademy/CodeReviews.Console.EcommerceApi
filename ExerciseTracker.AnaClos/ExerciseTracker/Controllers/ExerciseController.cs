using ExerciseTracker.DTO;
using ExerciseTracker.Models;
using ExerciseTracker.Services;
using ExerciseTracker.UI;
using ExerciseTracker.Validators;

namespace ExerciseTracker.Controllers;

public class ExerciseController : IController
{
    private List<string> mainOptions = new List<string> { "Add", "Delete", "Update", "View", "Exit Menu" };
    private UserInput _userInput;
    private IService _exerciseService;
    public ExerciseController(UserInput userInput, IService exerciseService)
    {
        _userInput = userInput;
        _exerciseService = exerciseService;
    }

    public void Menu()
    {
        string option;
        do
        {
            option = _userInput.Menu("Manage Exercises", "blue", mainOptions);
            switch (option)
            {
                case "Add":
                    Add();
                    break;
                case "Delete":
                    Delete();
                    break;
                case "Update":
                    Update();
                    break;
                case "View":
                    View();
                    break;
                case "View All":
                    ViewAll();
                    break;
            }
        } while (option != "Exit Menu");
    }

    public void Add()
    {
        string startString;
        string endString;
        DateTime startTime;
        DateTime endTime;

        _userInput.ShowMessage("The new shift must be after the last shift interval. The shift interval must not exceed 8 hours. ", "blue");

        do
        {
            startString = _userInput.GetString("Start Time in format yy/MM/dd HH:mm:ss");
            startTime = ExerciseValidator.DateTimeValidator(startString);
        } while (startTime == DateTime.MinValue);

        do
        {
            endString = _userInput.GetString("End Time in format yy/MM/dd HH:mm:ss");
            endTime = ExerciseValidator.DateTimeValidator(endString);
        } while (endTime == DateTime.MinValue);

        if (!ExerciseValidator.IntervalValidator(startTime, endTime))
        {
            _userInput.MessageAndPressKey("The shift interval does not meet the requested requirements.", "red");
            return;
        }

        var last10Exercises = _exerciseService.GetLast10();
        var lastExercise = last10Exercises.FirstOrDefault();


        if (lastExercise != null && !ExerciseValidator.OrderValidator(lastExercise, startTime))
        {
            _userInput.MessageAndPressKey("The time slot is already used", "red");
            return;
        }

        string comments = _userInput.GetString("Input a comment.");
        string message = "";

        Exercise exercise = new Exercise
        {
            Id = 0,
            DateStart = startTime,
            DateEnd = endTime,
            Duration = endTime - startTime,
            Comments = comments
        };
        try
        {
            _exerciseService.Add(exercise);
            _userInput.MessageAndPressKey("Successfully created exercise.", "orange1");
        }
        catch (Exception ex)
        {
            _userInput.MessageAndPressKey("The exercise could not be created", "orange1");
        }
    }

    public void Delete()
    {
        string message;
        bool confirmation = _userInput.Choice("You can only delete the last record, continue?");
        if (!confirmation)
        {
            return;
        }

        var exercises = _exerciseService.GetLast10();
        Exercise exercise = exercises.FirstOrDefault();
        if (exercise == null)
        {
            _userInput.MessageAndPressKey("There is no exercise to delete", "red");
            return;
        }
        try
        {
            _exerciseService.Delete(exercise);
            _userInput.MessageAndPressKey("Successfully deleted exercise.", "orange1");
        }
        catch (Exception ex)
        {
            _userInput.MessageAndPressKey("The exercise couldn't be deleted", "red");
        }
    }

    public void Update()
    {
        string startString;
        DateTime startTime;
        string endString;
        DateTime endTime;
        string message;
        bool confirmation = _userInput.Choice("You can only update the last record, continue?");
        if (!confirmation)
        {
            return;
        }

        var exercises = _exerciseService.GetLast10().ToList();
        Exercise exercise = exercises.FirstOrDefault();
        if (exercise == null)
        {
            _userInput.MessageAndPressKey("There is no exercise to update", "red");
            return;
        }

        do
        {
            startString = _userInput.GetString("Start Time in format yy/MM/dd HH:mm:ss");
            startTime = ExerciseValidator.DateTimeValidator(startString);
        } while (startTime == DateTime.MinValue);

        do
        {
            endString = _userInput.GetString("End Time in format yy/MM/dd HH:mm:ss");
            endTime = ExerciseValidator.DateTimeValidator(endString);
        } while (endTime == DateTime.MinValue);

        if (!ExerciseValidator.IntervalValidator(startTime, endTime))
        {
            _userInput.MessageAndPressKey("The shift interval does not meet the requested requirements.", "red");
            return;
        }

        exercises.RemoveAt(0);
        if (!ExerciseValidator.OrderValidator(exercises.FirstOrDefault(), startTime))
        {
            _userInput.MessageAndPressKey("The time slot is already used", "red");
        }

        string comments = _userInput.GetString("Input a comment.");
        exercise.DateStart = startTime;
        exercise.DateEnd = endTime;
        exercise.CalculateDuration();
        exercise.Comments = comments;
        try
        {
            _exerciseService.Update(exercise);
            _userInput.MessageAndPressKey("Successfully updated exercise.", "orange1");
        }
        catch (Exception ex)
        {
            _userInput.MessageAndPressKey("The exercise couldn't be updated", "red");
        }
    }

    public void View()
    {
        int order;

        var exercises = _exerciseService.GetLast10().ToList();
        if (exercises.Count == 0)
        {
            _userInput.MessageAndPressKey("There is no exercise to select ", "red");
            return;
        }

        var orderedExercises = exercises.OrderBy(sh => sh.Id).ToList();
        List<string> stringExercises = ExerciseToString(orderedExercises);
        string stringExercise = GetExerciseFromMenu("Select a exercise to view details", stringExercises);
        if (stringExercise == "Exit Menu")
        {
            return;
        }

        int.TryParse(stringExercise.Split('#')[1], out order);

        string[] columns = { "Property", "Value" };

        var exercise = orderedExercises.ElementAt(order - 1);

        var recordExercise = ExerciseToProperties(exercise);

        _userInput.ShowTable("Exercise", columns, recordExercise);
        _userInput.PressKey("Press a key to continue.");
    }

    public void ViewAll()
    {
        var exercises = _exerciseService.GetLast10().ToList();
        if (exercises.Count == 0)
        {
            _userInput.MessageAndPressKey("There is no exercises to view ", "red");
            return;
        }

        foreach (Exercise exercise in exercises)
        {

            exercise.CalculateDuration();
            _userInput.ShowMessage($"Exercise Date: {exercise.DateStart.Year}/{exercise.DateStart.Month}/{exercise.DateStart.Day} Duration: {exercise.Duration.Hours} hours, {exercise.Duration.Minutes} minutes, {exercise.Duration.Seconds} seconds", "green");
        }

        _userInput.PressKey("Press a key to continue.");
    }
    public List<string> ExerciseToString(List<Exercise> exercises)
    {
        var tableRecord = new List<string>();

        for (int i = 1; i <= exercises.Count; i++)
        {
            tableRecord.Add($"Exercise #{i}");
        }
        return tableRecord;
    }

    public int GetOrderFromList(List<string> stringShifts, string stringShift)
    {
        int order = 0;
        for (int i = 0; i < stringShifts.Count; i++)
        {
            if (stringShifts[i] == stringShift)
            {
                order = i;
            }
        }
        return order;
    }

    public string GetExerciseFromMenu(string title, List<string> stringShifts)
    {
        stringShifts.Add("Exit Menu");

        string stringExercise = _userInput.Menu(title, "blue", stringShifts);
        if (stringExercise == "Exit Menu")
        {
            return null;
        }
        return stringExercise;
    }

    public List<RecordDto> ExerciseToProperties(Exercise exercise)
    {
        var tableRecord = new List<RecordDto>();
        RecordDto record = null;
        foreach (var property in exercise.GetType().GetProperties())
        {
            if (property.GetValue(exercise) != null)
            {
                string value = "";
                value = property.GetValue(exercise).ToString();

                record = new RecordDto { Column1 = property.Name, Column2 = value };
                tableRecord.Add(record);
            }
        }

        return tableRecord;
    }
}