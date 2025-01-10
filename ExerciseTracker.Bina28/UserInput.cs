using Spectre.Console;

namespace ExerciseTracker;

internal class UserInput
{
	internal static void ShowExercise(ExerciseModel exerciseModel)
	{
		var panel = new Panel($@"Id: {exerciseModel.Id}
Start Time: {exerciseModel.DateStart}
End Time: {exerciseModel.DateEnd}
Comments: {exerciseModel.Comments}
Duration: {exerciseModel.Duration}");
		panel.Header = new PanelHeader("Exercise Information");
		panel.Padding = new Padding(2, 2, 2, 2);

		AnsiConsole.Write(panel);

		Console.WriteLine("Press any key to continue");
		Console.ReadLine();
		Console.Clear();

	}

	static internal void ShowTable(List<ExerciseModel> exerciseModel)
	{
		var table = new Table();
		table.AddColumn("Id");
		table.AddColumn("Start time");
		table.AddColumn("End time");
		table.AddColumn("Comments");
		table.AddColumn("Duration");

		foreach (var data in exerciseModel)
		{
			table.AddRow(data.Id.ToString(), data.DateStart.ToString(), data.DateEnd.ToString(), data.Comments, data.Duration.ToString();
		}

		AnsiConsole.Write(table);

		Console.WriteLine("Press any key to continue");
		Console.ReadLine();
		Console.Clear();
	}

	static internal void UpdateInput(ExerciseModel exerciseModel)
	{
		exerciseModel.DateStart = AnsiConsole.Confirm("Update start time?")
		? AnsiConsole.Ask<DateTime>("Enter start time: ")
		: exerciseModel.DateStart;
		exerciseModel.DateEnd = AnsiConsole.Confirm("Update end time?")
		? AnsiConsole.Ask<DateTime>("Enter end time: ")
		: exerciseModel.DateEnd;
		exerciseModel.Comments = AnsiConsole.Confirm("Update comments?")
		? AnsiConsole.Ask<String>("Enter comments: ")
		: exerciseModel.Comments;
		ExerciseController.Update(exerciseModel);
		AnsiConsole.MarkupLine("[green]Contact updated successfully![/]");

	}
}
