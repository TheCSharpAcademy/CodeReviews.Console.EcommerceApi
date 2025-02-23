namespace STUDY.ConsoleProjects.ExerciseTrackerFour;
public class Validation
{
    public static void isEndTimeBeforeStartTime(DateTime startTimeInput, DateTime endTimeInput)
    {
        if (endTimeInput < startTimeInput)
        {
            Console.WriteLine("Not in chronological order");
            Console.WriteLine("Going Back To The Main Menu");
            MainMenu.ShowMainMenu();
        }
        else
        {
            Console.WriteLine("Correctly in chronological order");
        }
    }
    public static void isPositiveNumber(DateTime startTimeInput, DateTime endTimeInput)
    {
        if (endTimeInput < DateTime.MinValue) 
        {
            Console.WriteLine("Date and time cannot be negative");
            Console.WriteLine("Going Back To The Main Menu");
            MainMenu.ShowMainMenu();
        }
        else if (startTimeInput < DateTime.MinValue)
        {
            Console.WriteLine("Date and time cannot be negative");
            Console.WriteLine("Going Back To The Main Menu");
            MainMenu.ShowMainMenu();
        }    
        else
        {
            Console.WriteLine("Correctly in positive number");
        }
    }
}
