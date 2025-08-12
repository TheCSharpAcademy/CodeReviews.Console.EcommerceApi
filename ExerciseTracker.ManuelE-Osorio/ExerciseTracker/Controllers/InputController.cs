using ExerciseTracker.Models;
using ExerciseTracker.UserInterface;
using ExerciseTracker.Validation;

namespace ExerciseTracker.Controllers;

public class InputController
{
    public static int? GetId<T>(List<T>? exerciseList) where T : class, IExerciseModel
    {
        string idString = "";
        string? errorMessage = null;
        ConsoleKeyInfo pressedKey;

        while(true)
        {
            if(exerciseList != null || exerciseList?.Count > 0)
                MainUI.DisplayExerciseList(exerciseList);
            MainUI.DisplayEnterId(errorMessage);
            Console.Write(idString);
            pressedKey = Console.ReadKey();
            
            switch(pressedKey.Key)
            {
                case(ConsoleKey.Enter):
                    errorMessage = InputValidation.IdValidation(idString, exerciseList ?? [], out int id);
                    if(errorMessage == null)
                        return id;
                    break;
                
                case(ConsoleKey.Backspace):
                    if(idString.Length > 0)
                        idString = idString.Remove(idString.Length-1);
                    break;
                
                default:
                    idString += pressedKey.KeyChar.ToString();
                    break;

                case(ConsoleKey.Escape):
                    return null; 
            }
        }
    }

    public static DateTime? GetDate(DateOptions dateType, ConfirmationOptions confirmationType)
    {
        string dateString = "";
        string? errorMessage = null;
        ConsoleKeyInfo pressedKey;

        while(true)
        {
            MainUI.DisplayEnterDate(dateType, confirmationType, errorMessage);
            Console.Write(dateString);
            pressedKey = Console.ReadKey();
            
            switch(pressedKey.Key)
            {
                case(ConsoleKey.Enter):
                    errorMessage = InputValidation.DateValidation(dateString, out DateTime date);
                    if(errorMessage == null)
                        return date;

                    break;
                
                case(ConsoleKey.Backspace):
                    if(dateString.Length > 0)
                        dateString = dateString.Remove(dateString.Length-1);
                    break;
                
                default:
                    dateString += pressedKey.KeyChar.ToString();
                    break;

                case(ConsoleKey.Escape):
                    return null; 
            }
        }
    }

    public static DateTime? GetDate(DateOptions dateType, ConfirmationOptions confirmationType, DateTime? existingDate)
    {
        string dateString = "";
        string? errorMessage = null;
        ConsoleKeyInfo pressedKey;

        while(true)
        {
            MainUI.DisplayEnterDate(dateType, confirmationType, errorMessage);
            Console.Write(dateString);
            pressedKey = Console.ReadKey();
            
            switch(pressedKey.Key)
            {
                case(ConsoleKey.Enter):
                    errorMessage = InputValidation.DateValidation(dateString, existingDate, dateType, out DateTime date);
                    if(errorMessage == null)
                        return date;
                    break;
                
                case(ConsoleKey.Backspace):
                    if(dateString.Length > 0)
                        dateString = dateString.Remove(dateString.Length-1);
                    break;
                
                default:
                    dateString += pressedKey.KeyChar.ToString();
                    break;

                case(ConsoleKey.Escape):
                    return null; 
            }
        }
    }

    public static string? GetComments(ConfirmationOptions confirmationType)
    {
        string? comments = "";
        string? errorMessage = null;
        ConsoleKeyInfo pressedKey;

        while(true)
        {
            MainUI.DisplayEnterComments(confirmationType, errorMessage);
            Console.Write(comments);
            pressedKey = Console.ReadKey();
            
            switch(pressedKey.Key)
            {
                case(ConsoleKey.Enter):
                    errorMessage = InputValidation.CommentsValidation(comments);
                    if(errorMessage == null)
                        return comments;
                    break;
                
                case(ConsoleKey.Backspace):
                    if(comments.Length > 0)
                        comments = comments.Remove(comments.Length-1);
                    break;
                
                default:
                    comments += pressedKey.KeyChar.ToString();
                    break;

                case(ConsoleKey.Escape):
                    return null; 
            }
        }
    }

    public static T? GetExercise<T>(ConfirmationOptions confirmationType) where T : class, IExerciseModel, new()
    {
        var startDate = GetDate(DateOptions.start, confirmationType);
        if(startDate == null)
            return default;
        
        var endDate = GetDate(DateOptions.end, confirmationType, startDate);
        if(endDate == null)
            return default;
        
        var comments = GetComments(confirmationType);
        if(comments == null)
            return default;
        
        var inputExercise = new T{StartDate = (DateTime)startDate, 
            EndDate = (DateTime)endDate, Duration = (TimeSpan)(endDate - startDate), Comments = comments};
        return inputExercise;
    }

    public static bool GetConfirmation(ConfirmationOptions confirmationType)
    {
        MainUI.DisplayConfirmationPromt(confirmationType);
        var confirmation = Console.ReadLine() ?? "";
        if(confirmation.Equals("y", StringComparison.InvariantCultureIgnoreCase))
            return true;
        return false;
    }
}