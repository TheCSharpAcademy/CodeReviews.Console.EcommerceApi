using System.Globalization;
using Spectre.Console;

namespace ExerciseTrackerCLI.Interface;

public static class Validators
{
    public static string DtFormat { get; set; } = "MM/dd/yyyy HH:mm";
    
    public static ValidationResult DateTimeFormatValidator(string prompt)
    {
        if (DateTime.TryParseExact(
                prompt,
                DtFormat,
                new CultureInfo("en-US"),
                DateTimeStyles.None,
                out _))
        {
            return ValidationResult.Success();
        }
        
        return ValidationResult.Error("[red]Time must be in format MM/dd/yyyy HH:mm (e.g., \"03/20/2025 15:30\".[/]");
    }
    
    public static ValidationResult EndTimeValidator(string prompt, DateTime start)
    {
        // Can't chain validators with Spectre, so call the format validator from here.
        var formatResult = DateTimeFormatValidator(prompt);
        if (!formatResult.Successful)
        {
            return formatResult;
        }
        
        var endTime = DateTime.ParseExact(prompt, DtFormat, CultureInfo.InvariantCulture);
        return endTime < start 
            ? ValidationResult.Error("[red]End date must be after start time.[/]") 
            : ValidationResult.Success();
    }
}