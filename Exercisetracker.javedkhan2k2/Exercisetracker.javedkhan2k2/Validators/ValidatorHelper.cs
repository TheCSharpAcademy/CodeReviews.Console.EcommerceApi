using System.Globalization;
using System.Text.RegularExpressions;
using Exercisetacker.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Exercisetacker.Validators;

public static class ValidatorHelper
{
    public static bool IsValidName(string value) => Regex.IsMatch(value, @"^[a-zA-Z][a-zA-Z\s]{2,}$");

    public static bool IsValidEmail(string value) => Regex.IsMatch(value, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");

    public static bool IsValidPhoneNumber(string value) => Regex.IsMatch(value, @"^\+49\d{10,14}$");
    
    internal static bool IsValidDateTimeInput(string? date)
    {
        if (date != null && date == "0")
        {
            return true;
        }
        if (date == null || !DateTime.TryParseExact(date, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            return false;
        }
        return true;
    }

    internal static bool IsValidDateInput(string? date, string? format)
    {
        if (date != null && date == "0")
        {
            return true;
        }
        if (date == null || !DateTime.TryParseExact(date, format, new CultureInfo("en-US"), DateTimeStyles.None, out _))
        {
            return false;
        }
        return true;
    }

    internal static bool IsValidComment(string? userInput)
    {
        if (userInput != null && userInput == "0")
        {
            return true;
        }
        if(userInput.IsNullOrEmpty() || userInput.Length > 255)
        {
            return false;
        }
        return true;
    }

    internal static bool IsValidDateTimeInputs(Jogging jogging) => DateTime.Compare(jogging.DateEnd, jogging.DateStart) > 0 ? true : false;
    
}