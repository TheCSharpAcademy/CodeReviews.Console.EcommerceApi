using ExerciseTracker.kjanos89.Models;

namespace ExerciseTracker.kjanos89;

public class Input
{
    Validation validation = new Validation();
    public Exercise GetExerciseData()
    {
        DateTime startDate;
        DateTime endDate;
        string dateString;
        string comment;
        //start date
        while (true)
        {
            Console.WriteLine("Please enter the start date of the exercise or press '0' to return to the menu:");
            Console.WriteLine("Example: 2024.09.08. 14:16");
            dateString = Console.ReadLine();
            if (!string.IsNullOrEmpty(dateString))
            {
                if(dateString=="0")
                {
                    return null;
                }
                if (validation.IsValidDate(dateString))
                {
                    startDate = validation.DateConverter(dateString);
                    break;
                }
            }
            else
            {
                Console.WriteLine("Wrong input, try again!");
                Thread.Sleep(1000);
            }
        }
        //end date
        while (true)
        {
            Console.WriteLine("Please enter the end date of the exercise or press '0' to return to the menu:");
            Console.WriteLine("Example: 2024.09.08. 14:56");
            dateString = Console.ReadLine();
            if (!string.IsNullOrEmpty(dateString))
            {
                if (dateString == "0")
                {
                    return null;
                }
                if (validation.IsValidDate(dateString))
                {
                    endDate = validation.DateConverter(dateString);
                    break;
                }
                else
                {
                    Console.WriteLine($"Invalid date: {dateString}. Please try again.");
                    Thread.Sleep(1000);
                }
            }
            else
            {
                Console.WriteLine("Wrong input, try again!");
                Thread.Sleep(1000);
            }
        }
        if (!validation.IsEndLater(startDate, endDate))
        {
            Console.WriteLine("End date must be later than the start date and the duration must be less than 24 hours.");
            Console.WriteLine("Record addition canceled, pressing any button will return to the menu.");
            Console.ReadLine();
            return null;
        }
        //comment
        Console.WriteLine("Please add a comment below or press '0' to return to the menu:");
        while (true)
        {
            comment = Console.ReadLine();
            if (!string.IsNullOrEmpty(comment))
            {
                if (comment == "0")
                {
                    return null;
                }
                break;
            }
            else
            {
                Console.WriteLine("Wrong input, try again!");
                Thread.Sleep(1000);
            }
        }
        var exercise = new Exercise
        {
            Start = startDate,
            End = endDate,
            Duration = endDate - startDate,
            Comments = comment
        };
        return exercise;
    }

    public int GetId()
    {
        Console.WriteLine("Enter an id from the list:");
        if(Int32.TryParse(Console.ReadLine(), out int id))
        {
            return id;
        }
        else
        {
            return 0;
        }
    }
}