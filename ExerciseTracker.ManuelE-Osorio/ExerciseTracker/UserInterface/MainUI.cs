using ExerciseTracker.Models;

namespace ExerciseTracker.UserInterface;

public class MainUI
{
    public static void DisplayWelcomeMessage()
    {      
        Console.WriteLine("Welcome to the exercise tracker app!");
        Thread.Sleep(2000);
    }

    public static void ClearUI()
    {
        Console.Clear();
    }

    public static void DisplayMainMenu(string? errorMessage)
    {
        if(errorMessage != null)
            Console.WriteLine($"Error: {errorMessage}");
        Console.WriteLine("Please select one of the following options or press Esc to exit the app:\n"+
            "1) Display all running logs\n"+
            "2) Insert a new running log\n"+
            "3) Update an existing running log\n"+
            "4) Delete a running log\n");
    }

    public static void DisplayExerciseList<T>(List<T>? exerciseList) where T : class
    {
        Console.Clear();
        TableUI.PrintTable(exerciseList);
    }

    public static void DisplayConfirmationPromt(ConfirmationOptions confirmationOption)
    {
        Console.Clear();
        Console.WriteLine($"Do you want to {confirmationOption} the selected log? [y/N]");
    }

    public static void DisplayModifyMenu<T>(T exerciseToModify) where T : IExerciseModel
    {
        Console.Clear();
        Console.WriteLine("Select the field you want to modify or press ESC to return:");
        Console.WriteLine($"1) Start Date: {exerciseToModify.StartDate}\n"+
            $"2) End Date: {exerciseToModify.EndDate}\n"+
            $"3) Comments: {exerciseToModify.Comments}");
    }

    public static void DisplayEnterId(string? errorMessage = null)
    {
        if(errorMessage != null)
            Console.WriteLine($"Error: {errorMessage}");

        Console.WriteLine("Please enter a log ID or press ESC to return:");
    }

    public static void DisplayEnterDate(DateOptions dateType, ConfirmationOptions confirmationType, string? errorMessage = null)
    {
        Console.Clear();
        if(errorMessage != null)
            Console.WriteLine($"Error: {errorMessage}");
        
        Console.WriteLine($"Please enter a {dateType} date to {confirmationType} with the following format yyyy/MM/dd HH:mm "+
            "or press ESC to return:\n"); 
    }

    public static void DisplayExitMessage()
    {
        Console.Clear();
        Console.WriteLine("Thank you for using the exercise tracker app!");
        Thread.Sleep(2000);
    }

    public static void DisplaySuccessMessage()
    {
        Console.Clear();
        Console.WriteLine("Operation successful!");
        Thread.Sleep(2000);
    }

    public static void DisplayEnterComments(ConfirmationOptions confirmationType, string? errorMessage)
    {
        Console.Clear();
        if(errorMessage != null)
            Console.WriteLine($"Error: {errorMessage}");
        
        Console.WriteLine($"Please enter the comments (within {ExerciseTrackerContext.CommentsLength} characters) " +
            $"to the exercise log to {confirmationType} or press ESC to return:\n");
    }

    public static void DisplayLoadingMessage()
    {
        Console.Clear();
        Console.WriteLine("Loading ...");
    }
}