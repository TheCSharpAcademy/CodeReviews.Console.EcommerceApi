using Spectre.Console;
using System.Globalization;

namespace ExerciseTracker.UserInterface;

public class Validation
{
    private  const string _correctDateTimeFormat = "MM-dd-yyyy HH:mm";

    internal static DateTime ValidateDateInput(string? dateInput)
    {
        if (string.IsNullOrWhiteSpace(dateInput))
        {
            throw new ArgumentException("Date input cannot be null or empty.");
        }
        if (dateInput.ToLower().Trim() == "q")
        {
            throw new ArgumentException("Operation cancelled.");
        }

        DateTime tempObject;
        dateInput = dateInput.Trim();
        if (!DateTime.TryParseExact(dateInput, _correctDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out tempObject))
        {
            throw new ArgumentException($"Invalid date/time format. Please use: {_correctDateTimeFormat} (e.g., 04-10-2024 04:30)");
        }
        if (DateTime.TryParse(dateInput, out tempObject) && tempObject > DateTime.Now)
        {
            throw new ArgumentException("Date cannot be in the future.");
        }
        tempObject = DateTime.ParseExact(dateInput, _correctDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None);
        return tempObject;
    }

    internal static int ValidateIntInput(string? intInput)
    {
        if (string.IsNullOrWhiteSpace(intInput))
            throw new ArgumentNullException(nameof(intInput), "Input cannot be null or empty.");

        intInput = intInput.Trim();

        if (intInput.ToLower().Trim() == "q")
            throw new ArgumentException("Operation cancelled.");

        if (string.IsNullOrWhiteSpace(intInput))
            throw new ArgumentException("Input cannot be null or empty.");

        if (!int.TryParse(intInput, out int result))
            throw new ArgumentException("Invalid input. Please enter a valid integer.");

        return result;
    }
}