namespace ExerciseTrackerCarDioLogics;

static public class Validator
{
    static public DateTime GetValidDate(string input, out bool isDateValid)
    {
        if (DateTime.TryParseExact(input, "yyyy/MM/dd HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime validDate))
        {
            isDateValid = true;
        }
        else
        {
            isDateValid = false;
            Console.WriteLine("Invalid time format! Valid format: yyyy/MM/dd HH:mm");
            Console.ReadLine();
        }
        return validDate;
    }
}
