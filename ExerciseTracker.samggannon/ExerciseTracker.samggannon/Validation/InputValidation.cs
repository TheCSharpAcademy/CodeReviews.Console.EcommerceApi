namespace ExerciseTracker.samggannon.Validation;

internal class InputValidation
{
    internal static void VaidateEditTime(DateTime dateStart, DateTime dateEnd)
    {
        bool isValidEditTime = ValidateTime(dateStart, dateEnd);

        if (isValidEditTime)
        {
            return;
        }
        else
        {
            Console.WriteLine("Start time must be before end time. Press [enter] to continue");
            Console.ReadLine();
        }
    }

    internal static bool ValidateTime(DateTime StartTime, DateTime EndTime)
    {
        if (EndTime > StartTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
