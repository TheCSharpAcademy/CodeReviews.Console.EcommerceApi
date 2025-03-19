using ExerciseTracker.ASV.Models;

namespace ExerciseTracker.ASV.UserInput;

public class Input : IInput
{
    public ExerciseData GetWorkoutDetails()
    {
        DateTime startDate, endDate;
        var exerciseData = new ExerciseData();
        Console.Write("Enter Start Date and Time (yyyy-MM-dd HH:mm): ");
        while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out startDate))
        {
            Console.WriteLine("Invalid input.");
            Console.Write("Please enter in the format \"yyyy-MM-dd HH:mm\": ");
        }
        exerciseData.DateStart = startDate;
        Console.Write("Enter End Time (HH:mm): ");
        while (!DateTime.TryParseExact(Console.ReadLine(), "HH:mm", null, System.Globalization.DateTimeStyles.None, out endDate)
               || endDate.TimeOfDay <= exerciseData.DateStart.TimeOfDay)
        {
            Console.WriteLine("Invalid input.");
            Console.Write("Please enter a time greater than the Start Time and in the format \"HH:mm\": ");
        }
        exerciseData.DateEnd = new DateTime(exerciseData.DateStart.Year,
                                            exerciseData.DateStart.Month,
                                            exerciseData.DateStart.Day,
                                            endDate.Hour,
                                            endDate.Minute,
                                            0);
        exerciseData.Duration = exerciseData.DateEnd - exerciseData.DateStart;
        Console.WriteLine($"Duration automatically calculated as: {exerciseData.Duration}");
        Console.Write("Enter Description (optional, press Enter to skip): ");
        exerciseData.Description = Console.ReadLine();
        return exerciseData;
    }

    public int GetWorkoutId()
    {
        int id;
        Console.Write("Enter Exercise Id (integer): ");
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.Write("Invalid Id. Please enter a valid integer:");
        }
        return id;
    }
}