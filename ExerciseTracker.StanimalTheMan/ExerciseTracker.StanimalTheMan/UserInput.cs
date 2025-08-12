using ConsoleTableExt;
using ExerciseTracker.StanimalTheMan.Controllers;
using ExerciseTracker.StanimalTheMan.Models;
using Spectre.Console;
using static ExerciseTracker.StanimalTheMan.Enums;

namespace ExerciseTracker.StanimalTheMan;

public class UserInput
{
	public static void ShowMainMenu(ExerciseController exerciseController)
	{
		var isAppRunning = true;
		while (isAppRunning)
		{
			Console.Clear();
			var option = AnsiConsole.Prompt(
				new SelectionPrompt<MenuOptions>()
				.Title("What would you like to do?")
				.AddChoices(
					MenuOptions.AddExercise,
					MenuOptions.DeleteExercise,
					MenuOptions.UpdateExercise,
					MenuOptions.GetAllExercises,
					MenuOptions.GetExerciseById,
					MenuOptions.Quit));

			var exercises = exerciseController.GetAllExercises().ToList();
			Console.WriteLine("Exercise List:");
			ConsoleTableBuilder
				.From(exercises)
				.WithFormat(ConsoleTableBuilderFormat.MarkDown)
				.ExportAndWriteLine();

			switch (option)
			{
				case MenuOptions.GetAllExercises:
					Console.WriteLine("Press any key to return to main menu");
					Console.ReadLine();
					break;
				case MenuOptions.GetExerciseById:
					// prompt user for id; if that is not acceptable, will make api request to fetch all valid ids
					int id = GetExerciseId();
					var exercise = exerciseController.GetExerciseById(id);
					while (exercise == null)
					{
						Console.WriteLine("No exercise found with that id.  Choose from table.");
						id = GetExerciseId();
						exercise = exerciseController.GetExerciseById(id);
					}
					Console.WriteLine(ConsoleTableBuilder.From(new List<Run> { exercise }).Export());
					Console.WriteLine("Press any key to return to main menu...");
					Console.ReadLine();
					break;
				case MenuOptions.AddExercise:
					Console.WriteLine("Enter distance in miles (e.g. 1, 1.5, 2)");
					double distance;
					while (!double.TryParse(Console.ReadLine(), out distance))
					{
						Console.WriteLine("Invalid distance.  Enter an id of the run you want to fetch info of");
					}

					Console.WriteLine("Enter Start Time of exercise: ");
					var startTimeInfo = Utility.GetDateTimeInput();

					Console.WriteLine("Enter End Time of exercise: ");
					var endTimeInfo = Utility.GetDateTimeInput();
					while (endTimeInfo.dateTime < startTimeInfo.dateTime)
					{
						Console.WriteLine("End time has to be after start time.  Enter End Time of shift:");
						endTimeInfo = Utility.GetDateTimeInput();
					}

					Console.WriteLine("Enter any comments on your run");
					string comments = Console.ReadLine();

					TimeSpan duration = Utility.CalculateDuration(endTimeInfo.dateTime, startTimeInfo.dateTime);
					exerciseController.AddExercise(new Run() { Distance = distance, DateStart = startTimeInfo.dateTime, DateEnd = endTimeInfo.dateTime, Duration = duration, Comments = comments });
					break;
				case MenuOptions.UpdateExercise:
					Console.WriteLine("Select id of exercise you want to update:");
					id = GetExerciseId();
					while (exerciseController.GetExerciseById(id) == null)
					{
						Console.WriteLine("No exercise found with that id.  Choose from table.");
						id = GetExerciseId();
					}
					Console.WriteLine("Enter distance in miles (e.g. 1, 1.5, 2)");
					while (!double.TryParse(Console.ReadLine(), out distance))
					{
						Console.WriteLine("Invalid distance.  Enter an id of the run you want to fetch info of");
					}

					Console.WriteLine("Enter Start Time of exercise: ");
					startTimeInfo = Utility.GetDateTimeInput();

					Console.WriteLine("Enter End Time of exercise: ");
					endTimeInfo = Utility.GetDateTimeInput();
					while (endTimeInfo.dateTime < startTimeInfo.dateTime)
					{
						Console.WriteLine("End time has to be after start time.  Enter End Time of shift:");
						endTimeInfo = Utility.GetDateTimeInput();
					}

					Console.WriteLine("Enter any comments on your run");
					comments = Console.ReadLine();

					duration = Utility.CalculateDuration(endTimeInfo.dateTime, startTimeInfo.dateTime);
					exerciseController.UpdateExercise(new Run {Id = id,  Distance = distance, DateStart = startTimeInfo.dateTime, DateEnd = endTimeInfo.dateTime, Duration = duration, Comments = comments });
					Console.WriteLine("Press any key to return to main menu.");
					Console.ReadLine();
					break;
				case MenuOptions.DeleteExercise:
					Console.WriteLine("Select id of exercise you want to delete:");
					while (!Int32.TryParse(Console.ReadLine(), out id))
					{
						Console.WriteLine("Invalid id.  Enter an id of the exercise you want to delete");
					}
					exerciseController.DeleteExercise(id);
					Console.WriteLine("Press any key to return to main menu.");
					Console.ReadLine();
					break;
				case MenuOptions.Quit:
					Console.WriteLine("Goodbye");
					isAppRunning = false;
					break;
			}
		}
	}

	internal static int GetExerciseId()
	{
		int id;
		Console.WriteLine("Enter an id of the exercise you want to fetch");
		while (!Int32.TryParse(Console.ReadLine(), out id))
		{
			Console.WriteLine("Invalid id.  Enter an id of the run you want to fetch");
		}
		return id;
	}
}
