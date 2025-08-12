using ExerciseTracker.samggannon.Data.Models;
using ExerciseTracker.samggannon.Validation;
    
namespace ExerciseTracker.samggannon.UserInterface;

internal class UserInput
{
    internal static Exercise GetSessionDetails()
    {
        Exercise session = new Exercise();

        while (true)
        {
            session.DateStart = GetDateTime("start");
            session.DateEnd = GetDateTime("end");

            bool isValidTimeInput = InputValidation.ValidateTime(session.DateStart, session.DateEnd);

            if (isValidTimeInput)
            {
                break;
            }

            Console.WriteLine("End time must be after start time. Please enter the details again.");
        }

        session.Duration = CalculateDuration(session.DateStart, session.DateEnd);
        session.Comments = GetComments();
        return session;
    }

    public static string? GetComments()
    {
        Console.WriteLine("Enter any comments or notes you have about this workout");
        string? comments = Console.ReadLine();

        if (comments == null)
        {
            return "no notes or comments";
        }

        comments = comments.Trim();

        if (string.IsNullOrEmpty(comments))
        {
            return "no notes or comments";
        }

        if (comments.Length > 500)
        {
            Console.WriteLine("Comments are too long. Please enter shorter comments (max 500 characters).");
            return GetComments();
        }

        return comments;
    }

    public static TimeSpan CalculateDuration(DateTime dateStart, DateTime dateEnd)
    {
        return dateEnd - dateStart;
    }

    public static DateTime GetDateTime(string v)
    {
        Console.WriteLine($"Please enter the {v} time for your workout (format: yyyy-MM-dd HH:mm:ss):");
        while (true)
        {
            if (DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("\n\nInvalid input. Please enter the date and time in the format\n " +
                    "yyyy-MM-dd HH:mm:ss\n " +
                    "Example 2024-05-14 13:15:00 - (1:15pm)\n " +
                    "Press Q to quit the application\n\n");
            }

            string? userQuit = Console.ReadLine();
            if (userQuit.Trim().ToUpper() == "Q") Environment.Exit(0);
        }
    }

    internal static int GetIdInput()
    {
        Console.WriteLine("Enter the Session Id of the workout you wish to modify");
        string sessionId = Console.ReadLine();
        int intSessionId;

        if (sessionId == null) { sessionId = "0"; }

        while (!Int32.TryParse(sessionId, out intSessionId) || intSessionId < 0)
        {
            if (sessionId == "0")
            {
                Console.WriteLine("Exiting...");
                return 0;
            }

            Console.WriteLine("You must enter a number for the session id. Press zero [0] to exit.");
            sessionId = Console.ReadLine();
        };

        return intSessionId;
    }
}
