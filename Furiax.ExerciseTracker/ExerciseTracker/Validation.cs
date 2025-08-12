namespace ExerciseTracker
{
	internal class Validation
	{
		internal static bool IsDateNotInFuture(DateTime date)
		{
			if (date < DateTime.Now)
				return true;
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Date can't be in the future");
				Console.ForegroundColor = ConsoleColor.White;
				return false;
			}
		}
		internal static bool IsEndDateGreaterThanStartDate(DateTime startDate, DateTime endDate)
		{
			if (startDate < endDate && endDate < DateTime.Now)
				return true;
			else if (startDate > endDate)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("The end time must be greater then the start time");
				Console.ForegroundColor = ConsoleColor.White;
				return false;
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Date can't be in the future");
				Console.ForegroundColor = ConsoleColor.White;
				return false;
			}

		}
	}
}
