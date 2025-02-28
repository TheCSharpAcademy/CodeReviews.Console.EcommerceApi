using System.Globalization;
using Spectre.Console;

namespace ExerciseTracker.cacheMe512;

internal static class Validation
{
    public static ValidationResult ValidateDateTime(string input)
    {
        if (!DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
        {
            return ValidationResult.Error("Invalid format. Use YYYY-MM-DD HH:mm (e.g., 2025-02-24 14:30).");
        }
        return ValidationResult.Success();
    }

    public static ValidationResult ValidatePositiveInt(int input)
    {
        return input > 0
            ? ValidationResult.Success()
            : ValidationResult.Error("Value must be greater than zero.");
    }

    public static ValidationResult ValidateNonEmptyString(string input)
    {
        return !string.IsNullOrWhiteSpace(input)
            ? ValidationResult.Success()
            : ValidationResult.Error("Input cannot be empty.");
    }

    public static ValidationResult ValidatePositiveDouble(string input)
    {
        if (double.TryParse(input, out double result) && result >= 0)
        {
            return ValidationResult.Success();
        }
        return ValidationResult.Error("Invalid number or negative value.");
    }

    public static ValidationResult ValidateExerciseDates(string startInput, string endInput)
    {
        bool startValid = DateTime.TryParseExact(startInput, "yyyy-MM-dd HH:mm",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime start);
        bool endValid = DateTime.TryParseExact(endInput, "yyyy-MM-dd HH:mm",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime end);

        if (!startValid || !endValid)
        {
            return ValidationResult.Error("One or both date/time inputs are invalid. Use YYYY-MM-DD HH:mm.");
        }

        if (end <= start)
        {
            return ValidationResult.Error("End date/time must be after start date/time.");
        }

        return ValidationResult.Success();
    }
}
