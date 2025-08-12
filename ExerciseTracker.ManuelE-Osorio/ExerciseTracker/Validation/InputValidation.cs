using System.Globalization;
using ExerciseTracker.Models;

namespace ExerciseTracker.Validation;

public class InputValidation
{
    public static string? IntValidation(string input, out int inputAsInt)
    {
        if(int.TryParse(input, out inputAsInt))
            return null;
        else
            return "Please enter a valid number.";
    }

    public static string? IdValidation<T>(string input, List<T> exerciseList, out int id) where T: IExerciseModel
    {
        var errorMessage = IntValidation(input, out id);
        var idToMatch = id;
        if(errorMessage == null)
        {
            if(exerciseList.Exists(p => p.Id == idToMatch))
                return null;
            else
                return "The Id you entered does not exists";
        }
        return errorMessage;
    }

    public static string? IntValidation(string input, int max, int min, out int inputAsInt)
    {
        if(int.TryParse(input, out inputAsInt))
        {
            if(inputAsInt >= min && inputAsInt <= max)
                return null;          
        }
        return $"Please enter a valid number within {min} and {max}.";
    }

    public static string? DateValidation(string input, out DateTime date)
    {
        if(DateTime.TryParseExact(input, "yyyy/MM/dd HH:mm", 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            return null;
        else
            return "Please enter a valid date with the specified format.";
    }

    public static string? DateValidation(string input, DateTime? existingDate, DateOptions dateType, out DateTime date)
    {
        if(DateTime.TryParseExact(input, "yyyy/MM/dd HH:mm", 
            CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        {
            switch(dateType)
            {
                case(DateOptions.start):
                    if (date < existingDate)
                        return null;             
                    else
                        return "The start date is after the end date.";
                case(DateOptions.end):
                    if (date > existingDate)
                        return null;
                    else
                        return "The end date is before the start date.";
            }
        }
        return "Please enter a valid date with the specified format.";
    }

    public static string? CommentsValidation(string comments)
    {
        if(comments.Length <= ExerciseTrackerContext.CommentsLength)
            return null;
        return "The length of the comments exceeds the maximum characters.";
    }
}