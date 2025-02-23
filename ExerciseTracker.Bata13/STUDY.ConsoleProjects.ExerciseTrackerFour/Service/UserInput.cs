namespace STUDY.ConsoleProjects.ExerciseTrackerFour.Service;
public class UserInput : IUserInput
{
    public (DateTime startTime, DateTime endTime, TimeSpan duration, string comments) GetUserInputForExcerciseEntry()
    {
        Console.WriteLine("Enter the new start time (yyyy-MM-dd HH:mm:ss):");
        DateTime startTime;

        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out startTime))
        {
            Console.WriteLine("Invalid input. Please enter a valid start time (yyyy-MM-dd HH:mm:ss):");
        }

        Console.WriteLine("Enter the new end time (yyyy-MM-dd HH:mm:ss):");

        DateTime endTime;

        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out endTime))
        {
            Console.WriteLine("Invalid input. Please enter a valid end time (yyyy-MM-dd HH:mm:ss):");
        }

        Console.WriteLine("Enter the end time of the exercise (yyyy-MM-dd HH:mm:ss):");

        Console.WriteLine("Enter Any Comments");
        string comments = Console.ReadLine();

        TimeSpan duration = endTime - startTime;

        Validation.isEndTimeBeforeStartTime(startTime, endTime);
        Validation.isPositiveNumber(startTime, endTime);

        return (startTime, endTime, duration, comments);
    }
    public (DateTime newStartTime, DateTime newEndTime, TimeSpan newDuration, string newComments, int exerciseId) GetUserInputForUpdatedExcerciseEntry()
    {
        Console.WriteLine("Enter the ID of the exercise entry you want to update:");

        int exerciseId;

        while (!int.TryParse(Console.ReadLine(), out exerciseId))
        {
            Console.WriteLine("Invalid input. Please enter a valid ID:");
        }

        Console.WriteLine("Enter the new start time (yyyy-MM-dd HH:mm:ss):");

        DateTime newStartTime;

        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out newStartTime))
        {
            Console.WriteLine("Invalid input. Please enter a valid start time (yyyy-MM-dd HH:mm:ss):");
        }

        Console.WriteLine("Enter the new end time (yyyy-MM-dd HH:mm:ss):");

        DateTime newEndTime;

        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out newEndTime))
        {
            Console.WriteLine("Invalid input. Please enter a valid end time (yyyy-MM-dd HH:mm:ss):");
        }

        Console.WriteLine("Enter Any Comments");
        string newComments = Console.ReadLine();

        TimeSpan newDuration = newEndTime - newStartTime;

        Validation.isEndTimeBeforeStartTime(newStartTime, newEndTime);
        Validation.isPositiveNumber(newStartTime, newEndTime);

        return (newStartTime, newEndTime, newDuration, newComments, exerciseId);
    }
    public int GetUserInputToDelete()
    {
        Console.WriteLine("Enter the ID of the exercise entry you want to delete:");
        int exerciseId;
        while (!int.TryParse(Console.ReadLine(), out exerciseId))
        {
            Console.WriteLine("Invalid input. Please enter a valid ID:");
        }

        return (exerciseId);
    }
}
