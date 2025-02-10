using ExerciseTracker.DTO;
using ExerciseTracker.UI;

namespace ExerciseTracker.Controllers;

public class ExerciseController:IController
{
    private List<string> mainOptions = new List<string> { "Add", "Delete", "Update", "View", "Exit Menu" };
    private UserInput _userInput;
    public ExerciseController(UserInput userInput)
    {
        _userInput = userInput;
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
            startTime = _validator.DateTimeValidator(startString);
        } while (startTime == DateTime.MinValue);

        do
        {
            endString = _consoleController.GetString("End Time in format yy/MM/dd HH:mm:ss");
            endTime = _validator.DateTimeValidator(endString);
        } while (endTime == DateTime.MinValue);

        if (!_validator.IntervalValidator(startTime, endTime))
        {
            _consoleController.MessageAndPressKey("The shift interval does not meet the requested requirements.", "red");
            return;
        }

        var last10Shift = _shiftsService.GetLast10Shifts(employee.Id);
        var lastShift = last10Shift.FirstOrDefault();


        if (lastShift != null && !_validator.OrderValidator(lastShift, startTime))
        {
            _consoleController.MessageAndPressKey("The time slot is already used", "red");
            return;
        }

        ShiftDto shiftDto = new ShiftDto
        {
            Id = 0,
            StartTime = startTime,
            EndTime = endTime,
            EmployeeId = employee.Id,
        };
        var response = _shiftsService.AddShift(shiftDto);
        _consoleController.MessageAndPressKey(response, "orange1");
    }

    public void Delete()
    {
        bool confirmation = _consoleController.Choice("You can only delete the last record, continue?");
        if (!confirmation)
        {
            return;
        }

        EmployeeDto employee = _employeesController.GetEmployeeFromMenu("Select an employee");
        if (employee == null)
        {
            return;
        }

        var shifts = _shiftsService.GetLast10Shifts(employee.Id);
        ShiftDto shift = shifts.FirstOrDefault();
        if (shift == null)
        {
            _consoleController.MessageAndPressKey("There is no Shift to delete", "red");
            return;
        }

        string message = _shiftsService.RemoveShift(shift.Id);
        if (message == null)
        {
            _consoleController.MessageAndPressKey("The shift couldn't be deleted", "red");
        }
        else
        {
            _consoleController.MessageAndPressKey(message, "green");
        }
    }

    public void Update()
    {
        string startString;
        DateTime startTime;
        string endString;
        DateTime endTime;
        bool confirmation = _consoleController.Choice("You can only update the last record, continue?");
        if (!confirmation)
        {
            return;
        }

        EmployeeDto employee = _employeesController.GetEmployeeFromMenu("Select an employee");
        if (employee == null)
        {
            return;
        }

        var shifts = _shiftsService.GetLast10Shifts(employee.Id);
        ShiftDto shift = shifts.FirstOrDefault();
        if (shift == null)
        {
            _consoleController.MessageAndPressKey("There is no Shift to update", "red");
            return;
        }

        do
        {
            startString = _consoleController.GetString("Start Time in format yy/MM/dd HH:mm:ss");
            startTime = _validator.DateTimeValidator(startString);
        } while (startTime == DateTime.MinValue);

        do
        {
            endString = _consoleController.GetString("End Time in format yy/MM/dd HH:mm:ss");
            endTime = _validator.DateTimeValidator(endString);
        } while (endTime == DateTime.MinValue);

        if (!_validator.IntervalValidator(startTime, endTime))
        {
            _consoleController.MessageAndPressKey("The shift interval does not meet the requested requirements.", "red");
            return;
        }

        shifts.RemoveAt(0);
        if (!_validator.OrderValidator(shifts.FirstOrDefault(), startTime))
        {
            _consoleController.MessageAndPressKey("The time slot is already used", "red");
        }

        shift.StartTime = startTime;
        shift.EndTime = endTime;
        string message = _shiftsService.UpdateShift(shift);
        if (message == null)
        {
            _consoleController.MessageAndPressKey("The shift couldn't be updated", "red");
        }
        else
        {
            _consoleController.MessageAndPressKey(message, "green");
        }
    }

    public void View()
    {
        int order;
        EmployeeDto employee = _employeesController.GetEmployeeFromMenu("Select an employee");
        if (employee == null)
        {
            _consoleController.MessageAndPressKey("There is no employee to select ", "red");
            return;
        }

        List<ShiftDto> shifts = _shiftsService.GetLast10Shifts(employee.Id);
        if (shifts.Count == 0)
        {
            _consoleController.MessageAndPressKey("There is no shift to select ", "red");
            return;
        }

        var orderedShifts = shifts.OrderBy(sh => sh.Id).ToList();
        List<string> stringShifts = ShiftToString(orderedShifts);
        string stringShift = GetShiftFromMenu("Select a shift to view details", stringShifts);
        if (stringShift == "Exit Menu")
        {
            return;
        }

        int.TryParse(stringShift.Split('#')[1], out order);

        string[] columns = { "Property", "Value" };

        var shiftDto = orderedShifts.ElementAt(order - 1);
        var shift = ShiftService.Dto2Model(shiftDto);

        shift.Employee = EmployeeService.Dto2Model(employee);
        var recordShift = ShiftToProperties(shift);

        _consoleController.ShowTable("Shift", columns, recordShift);
        _consoleController.PressKey("Press a key to continue.");
    }

    public void ViewAll()
    {
        EmployeeDto employee = _employeesController.GetEmployeeFromMenu("Select an employee");

        if (employee == null)
        {
            return;
        }

        List<ShiftDto> shifts = _shiftsService.GetLast10Shifts(employee.Id);
        if (shifts.Count == 0)
        {
            _consoleController.MessageAndPressKey("There is no shift to view ", "red");
            return;
        }

        foreach (ShiftDto shiftDto in shifts)
        {
            var shift = new Shift
            {
                StartTime = shiftDto.StartTime,
                EndTime = shiftDto.EndTime
            };
            TimeSpan duration = shift.CalculateDuration();
            _consoleController.ShowMessage($"Shift Date: {shift.StartTime.Year}/{shift.StartTime.Month}/{shift.StartTime.Day} Duration: {duration.Hours} hours, {duration.Minutes} minutes, {duration.Seconds} seconds", "green");
        }

        _consoleController.PressKey("Press a key to continue.");
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

    public List<string> ShiftToString(List<ShiftDto> shifts)
    {
        var tableRecord = new List<string>();

        for (int i = 1; i <= shifts.Count; i++)
        {
            tableRecord.Add($"Shitf #{i}");
        }
        return tableRecord;
    }

    public List<RecordDto> ShiftToProperties(Shift shift)
    {
        var tableRecord = new List<RecordDto>();
        RecordDto record = null;
        foreach (var property in shift.GetType().GetProperties())
        {
            if (property.GetValue(shift) != null)
            {
                string value = "";

                if (property.Name == "Employee")
                {
                    value = shift.Employee.Name;
                }
                else if (property.Name == "EmployeeId")
                {
                    continue;
                }
                else
                {
                    value = property.GetValue(shift).ToString();
                }
                record = new RecordDto { Column1 = property.Name, Column2 = value };
                tableRecord.Add(record);
            }
        }

        record = new RecordDto { Column1 = "Duration", Column2 = shift.CalculateDuration().ToString() };
        tableRecord.Add(record);
        return tableRecord;
    }
}