using System.Globalization;

namespace sadklouds.ExcerciseTracker.DataInput;
public class UserInput : IUserInput
{
    public int GetIdInput()
    {
        int id;
        Console.Write("Enter Exercise id: ");
        string? textId = Console.ReadLine();
        while (Validator.IsValid(textId) == false)
        {
            Console.WriteLine("Invalid id given");
            GetIdInput();
        }
        int.TryParse(textId, out id);
        return id;
    }

    public string GetComment()
    {
        Console.WriteLine("Please enter comment for exercise or leave blank: ");
        string comment = Console.ReadLine();
        while (Validator.ValidCommentLength(comment) == false)
        {
            Console.WriteLine("Comment must be under 255 characters");
            comment = Console.ReadLine();
        }
        return comment;
    }

    public DateTime GetStartDate()
    {
        Console.Write("Please enter exercise start date & time (dd/MM/yyyy HH:mm): ");
        string textDate = Console.ReadLine();
        DateTime startDate;
        while (Validator.IsValidDateFormat(textDate) == false)
        {
            Console.Write("Invalid start date format given, Correct formatting is dd/MM/yyyy HH:mm: ");
            textDate = Console.ReadLine();
        }
        DateTime.TryParseExact(textDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);
        return startDate;

    }

    public DateTime GetEndDate(DateTime startDate)
    {
        Console.Write("Please enter exercise end date & time (dd/MM/yyyy HH:mm): ");
        string textDate = Console.ReadLine();
        DateTime endDate;
        while (Validator.IsValidDateFormat(textDate) == false)
        {
            Console.Write("Invalid end date format given. Correct formatting is dd/MM/yyyy HH:mm");
            textDate = Console.ReadLine();
        }
        DateTime.TryParseExact(textDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate);
        while (Validator.IsValidEndDate(textDate, startDate) == false)
        {
            Console.WriteLine("Exercise cannot end before it has started, Correct formatting is dd/MM/yyyy HH:mm.");
            GetEndDate(startDate);
        }
        return endDate;
    }
}
