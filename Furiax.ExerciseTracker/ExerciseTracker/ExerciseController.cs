using ExerciseTracker.Models;
using Spectre.Console;

namespace ExerciseTracker
{
	public class ExerciseController
	{
		private readonly ExerciseService _exerciseService;
		public ExerciseController(ExerciseService exerciseService)
		{
			_exerciseService = exerciseService;
		}
		public static void PrintExercisesTable(List<ExerciseModel> exercises)
		{
			var table = new Table();
			table.AddColumn("Id");
			table.AddColumn("Type");
			table.AddColumn("Start time");
			table.AddColumn("End time");
			table.AddColumn("Duration");
			table.AddColumn("Comment");

			foreach (var exercise in exercises)
			{
				table.AddRow(exercise.ExerciseId.ToString(), exercise.ExerciseType,
					exercise.DateStart.ToString(), exercise.DateEnd.ToString(),
					exercise.Duration.ToString(), exercise.Comments);
			}
			AnsiConsole.Write(table);

			Console.WriteLine("Press any key to continue");
			Console.ReadKey();
		}
		public static void PrintExercise(ExerciseModel exercise)
		{
			var panel = new Panel($@"Exercise: {exercise.ExerciseType}
Start time: {exercise.DateStart}
End time: {exercise.DateEnd}
Duration: {exercise.Duration}
Comment: {exercise.Comments}");
			panel.Header = new PanelHeader("Exercise info:");
			panel.Padding = new Padding(2, 2, 2, 2);

			AnsiConsole.Write(panel);
			Console.WriteLine("Press any key to continue.");
			Console.ReadKey();
			Console.Clear();
		}
		internal static int GetIdOption(List<ExerciseModel> exercises)
		{
			int id = UserInput.GetIdOfExercise(exercises);
			return id;
		}
		internal static ExerciseModel GetUpdateInfo(ExerciseModel exercise)
		{
			var updatedExercise = UserInput.GetUpdatedInfo(exercise);
			return updatedExercise;
		}
		public void MainMenu()
		{
			bool isAppAlive = true;
			while (isAppAlive)
			{
				Console.Clear();
				var option = AnsiConsole.Prompt(new SelectionPrompt<Menu>()
					.Title("What would you like to do?")
					.AddChoices(
						Menu.AddExercise,
						Menu.ViewAllExercises,
						Menu.ViewExercise,
						Menu.UpdateExercise,
						Menu.DeleteExercise,
						Menu.Quit
						));
				switch (option)
				{
					case Menu.AddExercise:
						_exerciseService.AddExercise();
						break;
					case Menu.ViewAllExercises:
						_exerciseService.GetAll();
						break;
					case Menu.ViewExercise:
						_exerciseService.GetExerciseById();
						break;
					case Menu.UpdateExercise:
						_exerciseService.UpdateExercise();
						break;
					case Menu.DeleteExercise:
						_exerciseService.DeleteExercise();
						break;
					case Menu.Quit:
						isAppAlive = false;
						break;
				}
			}
		}
		enum Menu
		{
			AddExercise,
			ViewAllExercises,
			ViewExercise,
			UpdateExercise,
			DeleteExercise,
			Quit
		}
	}
}
