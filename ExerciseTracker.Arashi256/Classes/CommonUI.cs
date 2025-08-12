using ExerciseTracker.Arashi256.Enums;
using Spectre.Console;
using System.Net.Quic;

namespace ExerciseTracker.Arashi256.Classes
{
    internal class CommonUI
    {
        public static int MenuOption(string question, int min, int max)
        {
            bool isValid = false;
            int selectedValue = 0;
            do
            {
                var userInput = AnsiConsole.Ask<int>(question);
                selectedValue = userInput;
                if (selectedValue < min || selectedValue > max)
                {
                    AnsiConsole.MarkupLine("[red]Invalid input. Please enter a value within the specified range.[/]");
                    isValid = false;
                }
                else
                    isValid = true;
            } while (!isValid);
            return selectedValue;
        }

        public static void Pause(string colour)
        {
            AnsiConsole.Markup($"[{colour}]Press any key to continue...[/]");
            Console.ReadKey(true);
        }

        public static string? GetStringWithPrompt(string prompt, int lengthlimit, string nullString)
        {
            AnsiConsole.MarkupLine("[white]Enter '0' to cancel[/]");
            string input = AnsiConsole.Ask<string>(prompt).Trim();
            if (input.Equals("0")) return null;
            while (input.Length > lengthlimit)
            {
                AnsiConsole.MarkupLine($"\n[red]Entry needs to be less than {lengthlimit} characters. Try again.[/]\n\n");
                input = AnsiConsole.Ask<string>(prompt);
            }
            return input;
        }

        public static DateTime? GetDateTimeDialog(string format)
        {
            DateTime? dateTime = null;
            while (!dateTime.HasValue)
            {
                AnsiConsole.MarkupLine($"[steelblue1_1]Note: Enter '0' to abort[/]");
                var userInput = AnsiConsole.Prompt(new TextPrompt<string>($"Enter a date/time in the format '{format}':").PromptStyle("white")).Trim();
                if (userInput == "0")
                {
                    return null;
                }
                else
                {
                    if (DateTime.TryParseExact(userInput, format, null, System.Globalization.DateTimeStyles.None, out DateTime result))
                    {
                        dateTime = result;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Invalid date/time format. Please enter the date/time in the specified format.[/]");
                    }
                }
            }
            return dateTime;
        }

        public static ExerciseType GetExerciseTypeDialog(IEnumerable<ExerciseType> exerciseTypes)
        {
            // Prompt the user to select an exercise type from the exercise type public enum. Avert your eyes.
            var exerciseType = AnsiConsole.Prompt(new SelectionPrompt<ExerciseType>().Title("Select your exercise type:").AddChoices(exerciseTypes));
            return exerciseType;
        }
    }
}
