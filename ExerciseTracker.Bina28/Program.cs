using ExerciseTracker;
using ExerciseTracker.Controller;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using System.ComponentModel;


var serviceProvider = Starter.InitializeServices();  // Initialize services
var controller = serviceProvider.GetService<ExerciseController>();

var isAppRunning = true;

while (isAppRunning)
{
	Console.Clear();

	// Map enum to descriptions
	var enumToDescription = Enum.GetValues(typeof(MenuOptions))
		.Cast<MenuOptions>()
		.ToDictionary(option => GetDescription(option), option => option);

	while (isAppRunning)
	{
		Console.Clear();

		// Generate a list of descriptions based on enum
		var menuChoices = enumToDescription.Keys.ToList();

		// Use the description list in the SelectionPrompt
		var options = AnsiConsole.Prompt(
			new SelectionPrompt<string>()
				.Title("Select an option")
				.AddChoices(menuChoices)
		);

		// Get the corresponding enum value from the dictionary
		if (!enumToDescription.TryGetValue(options, out var selectedOption))
		{
			Console.WriteLine("Invalid selection.");
			continue;
		}

		// Call methods through ExerciseController
		switch (selectedOption)
		{
			case MenuOptions.AddData:
				controller.Add(); 
				break;
			case MenuOptions.RemoveData:
				controller.Remove(); 
				break;
			case MenuOptions.ShowAllData:
				controller.GetAll(); 
				break;
			case MenuOptions.ShowData:
				controller.GetById(); 
				break;
			case MenuOptions.UpdateData:
				controller.Update();  
				break;
			case MenuOptions.Exit:
				AnsiConsole.MarkupLine("[green]Exiting the application. Goodbye![/]");
				isAppRunning = false;  
				break;
		}
	}
}

// The method for getting the description of an enum
static string GetDescription(Enum value)
{
	Type type = value.GetType();
	string name = Enum.GetName(type, value);
	if (name != null)
	{
		var field = type.GetField(name);
		if (field != null)
		{
			var attr = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
			if (attr != null)
			{
				return attr.Description;
			}
		}
	}
	return value.ToString(); // Return the enum name if no description is found
}

enum MenuOptions
{
	[Description("Add")]
	AddData,

	[Description("Remove")]
	RemoveData,

	[Description("Show all exercises")]
	ShowAllData,

	[Description("Show exercise by Id")]
	ShowData,

	[Description("Update")]
	UpdateData,

	[Description("Exit")]
	Exit
}
