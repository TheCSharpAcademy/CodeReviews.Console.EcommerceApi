using ExerciseTracker.Controller;
using ExerciseTracker.Controllers;
using ExerciseTracker.Models;
using ExerciseTracker.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExerciseTracker;

internal class UI
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

	static internal void ShowTable(IEnumerable<ExerciseModel> exerciseModel)
	{
		var table = new Table();
		table.AddColumn("Id");
		table.AddColumn("Start time");
		table.AddColumn("End time");
		table.AddColumn("Comments");
		table.AddColumn("Duration");

		foreach (var data in exerciseModel)
		{
			table.AddRow(data.Id.ToString(), data.DateStart.ToString(), data.DateEnd.ToString(), data.Comments, data.Duration.ToString());
		}

		AnsiConsole.Write(table);

		Console.WriteLine("Press any key to continue");
		Console.ReadLine();
		Console.Clear();
	}

	
}
