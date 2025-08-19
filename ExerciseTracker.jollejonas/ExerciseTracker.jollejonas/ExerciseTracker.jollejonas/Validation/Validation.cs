namespace ExerciseTracker.jollejonas.Validation;

public class Validation
{
    public static bool ValidateDate(string date)
    {
        if (DateTime.TryParse(date, out DateTime result))
        {
            return true;
        }
        return false;
    }

    public static bool ValidateComment(string comment)
    {
        if (comment.Length > 0 && comment.Length < 1000)
        {
            return true;
        }
        return false;
    }
}
