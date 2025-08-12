using System.Globalization;

namespace ExerciseTracker.StanimalTheMan;

internal class Utility
{
	internal static (string dateTimeString, DateTime dateTime) GetDateTimeInput()
	{
		Console.Write("Enter a date and time (e.g., 'yyyy-MM-dd HH:mm').  Type 0 to return to main menu.\n\n");

		string dateTimeInput = Console.ReadLine();

		DateTime dateTime;
		while (!DateTime.TryParseExact(dateTimeInput, "yyyy-MM-dd HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out dateTime))
		{
			Console.WriteLine("\n\nNot a valid datetime.  Please insert the datetime with the format: yyyy-MM-dd HH:mm.\n\n");
			dateTimeInput = Console.ReadLine();
		}

		Console.WriteLine();

		return (dateTimeInput, dateTime);
	}

	internal static TimeSpan CalculateDuration(DateTime endDateTime, DateTime startDateTime)
	{
		TimeSpan duration = endDateTime - startDateTime;

		return TimeSpan.Parse($"{duration.Hours}:{duration.Minutes}");
	}
}
