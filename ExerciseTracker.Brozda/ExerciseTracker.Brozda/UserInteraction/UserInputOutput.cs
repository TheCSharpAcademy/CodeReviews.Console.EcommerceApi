using ExerciseTracker.Brozda.Models;
using Spectre.Console;
using System.Globalization;

namespace ExerciseTracker.Brozda.UserInteraction
{
    /// <summary>
    /// Provides means to show data and to retrieve input from the user via Console
    /// </summary>
    internal class UserInputOutput : IUserInputOutput
    {
        private string _dateFormat = "yyyy-MM-dd HH:mm";

        /// <summary>
        /// Prints provided text to the console
        /// </summary>
        /// <param name="text">Text to be printed</param>
        public void PrintText(string text)
        {
            Console.WriteLine(text);
        }
        /// <summary>
        /// Prints provided error message to the console
        /// </summary>
        /// <param name="errorMsg">Error message to be printed</param>
        public void PrintError(string? errorMsg)
        {
            Console.WriteLine(errorMsg ?? "Unhandled error");
        }
        /// <summary>
        /// Prints "Press any Key to continue" and awaits user input, effectively pausing the app flow
        /// </summary>
        public void PrintPressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
            Console.Clear();
        }
        /// <summary>
        /// Prints the menu and awaits user choice
        /// </summary>
        /// <param name="menuOptions"><see cref="Dictionary{TKey, TValue}"/>; 
        /// Keys are <see cref="int"/> representing menu options enumeration
        /// Values are <see cref="string"/> representing option lable
        /// </param>
        /// <returns><see cref="int"/> representing menu options enumeration</returns>
        public int ShowMenuAndGetInput(Dictionary<int, string> menuOptions)
        {
            var input = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                .Title("Please select your choice")
                .AddChoices(menuOptions.Select(x => x.Key).ToList())
                .UseConverter(x => menuOptions[x])
                );

            return input;
        }
        /// <summary>
        /// Prints exercises to the user console in form of Table
        /// </summary>
        /// <param name="exercises">A List of <see cref="Exercise"/> to be printed</param>
        public void PrintExercises(List<Exercise> exercises)
        {
            var table = new Table();
            table.AddColumns("Id", "Name", "Weight Lifted", "Start", "End", "Duration", "Comments");
            foreach (var exercise in exercises)
            {
                table.AddRow(GetTableRow(exercise));
            }

            AnsiConsole.Write(table);
        }
        /// <summary>
        /// Prints a single exercise to the user console in form of Table
        /// </summary>
        /// <param name="exercise"> <see cref="Exercise"/> to be printed</param>
        public void PrintExercise(Exercise exercise)
        {
            var table = new Table();
            table.AddColumns("Id", "Name", "Weight Lifted", "Start", "End", "Duration", "Comments");
            table.AddRow(GetTableRow(exercise));

            AnsiConsole.Write(table);
        }
        /// <summary>
        /// Prints out all records to the console and awaits user input representing his choice
        /// </summary>
        /// <param name="exercises">A List of <see cref="Exercise"/> from which user can choose</param>
        /// <param name="prompt">A <see cref="string"/> representing the prompt for the choice</param>
        /// <returns>A <see cref="int"/> representing valid record Id</returns>
        /// <remarks> User input is validated and only valid choice is possible
        /// </remarks>
        public int GetRecordId(List<Exercise> exercises, string prompt)
        {
            PrintExercises(exercises);

            var validIds = exercises.Select(x => x.Id).ToList();

            var selectedId = AnsiConsole.Prompt(
                new TextPrompt<int>(prompt)
                .Validate(x => validIds.Contains(x) || x == 0)
                );

            return selectedId;
        }
        /// <summary>
        /// Retrieves values for <see cref="Exercise"/> 
        /// </summary>
        /// <param name="existing">Optional argument of existing <see cref="Exercise"/>. If present then its values will be used as default values</param>
        /// <returns>A <see cref="Exercise"/> containing values from user input</returns>
        public Exercise GetExercise(Exercise? existing = null)
        {
            string name = GetString("Exercise name: ", existing?.Name);
            double lifted = GetDouble("Weight lifted: ", existing?.WeightLifted); ;
            DateTime start = GetDate("Enter start date: ", existing?.DateStart); ;
            DateTime end = GetDate("Enter end date: ", existing?.DateEnd, start); ;
            long duration = (long)(end - start).TotalSeconds;
            string? comments = GetNullableString("Enter a comment (may be left empty): ", existing?.Comments); ;

            return new Exercise()
            {
                Name = name,
                WeightLifted = lifted,
                DateStart = start,
                DateEnd = end,
                Duration = duration,
                Comments = comments
            };

        }
        /// <summary>
        /// Retrieves <see cref="DateTime"/> value from user input 
        /// </summary>
        /// <param name="prompt">A <see cref="string"/> reprenting prompt</param>
        /// <param name="defaultVal">A optional argument containing defaul value</param>
        /// <param name="startDate">A optional argument containing start time. If filled it indicates that user is entering end time
        /// This fires addition check to make sure end is not before start</param>
        /// <returns>A valid <see cref="DateTime"/> value</returns>
        private DateTime GetDate(string prompt, DateTime? defaultVal = null, DateTime? startDate = null)
        {
            var textPrompt = new TextPrompt<string>(prompt)
            .Validate(x => ValidateDateTime(x, startDate));

            if (defaultVal is not null)
            {
                textPrompt.DefaultValue(defaultVal.Value.ToString(_dateFormat));
            }

            var dateString = AnsiConsole.Prompt(textPrompt);

            return DateTime.ParseExact(dateString, _dateFormat, CultureInfo.InvariantCulture);


        }
        /// <summary>
        /// Validates <see cref="DateTime"/> value
        /// </summary>
        /// <param name="dateString">A <see cref="string"/> value containing value from user input</param>
        /// <param name="startDate">A optional argument containing start time. If filled it indicates that user is entering end time
        /// This fires addition check to make sure end is not before start</param>
        /// <returns> A successfule <see cref="ValidationResult"/> or result containg respective error message</returns>
        private ValidationResult ValidateDateTime(string dateString, DateTime? startDate)
        {
            if (startDate is not null)
            {
                DateTime endDate;

                if (!DateTime.TryParseExact(dateString, _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                    return ValidationResult.Error("Invalid date format, format needs to be yyyy-mm-dd hh:mm 24H format");
                if (endDate < startDate)
                    return ValidationResult.Error("End cannot be before start");

                return ValidationResult.Success();
            }
            else
            {
                if (!DateTime.TryParseExact(dateString, _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    return ValidationResult.Error("Invalid date format, format needs to be yyyy-mm-dd hh:mm 24H format");

                return ValidationResult.Success();
            }


        }
        /// <summary>
        /// Retrieves <see cref="double"/> value from user input
        /// </summary>
        /// <param name="prompt">A <see cref="string"/> reprenting prompt</param>
        /// <param name="defaultVal">Optional value to be shown as default value for user input</param>
        /// <returns>A valid <see cref="double"/> value</returns>
        private double GetDouble(string prompt, double? defaultVal = null)
        {
            var textPrompt = new TextPrompt<double>(prompt);

            if (defaultVal is not null)
            {
                textPrompt.DefaultValue(defaultVal.Value);
            }

            return AnsiConsole.Prompt(textPrompt);

        }
        /// <summary>
        /// Retrieves <see cref="string"/> value from user input
        /// </summary>
        /// <param name="prompt">A <see cref="string"/> reprenting prompt</param>
        /// <param name="defaultVal">Optional value to be shown as default value for user input</param>
        /// <returns>A valid <see cref="string"/> value</returns>
        private string GetString(string prompt, string? defaultVal = null)
        {
            var textPrompt = new TextPrompt<string>(prompt);

            if (defaultVal is not null)
            {
                textPrompt.DefaultValue(defaultVal);
            }

            return AnsiConsole.Prompt(textPrompt);

        }
        /// <summary>
        /// Retrieves a <see cref="string"/> value from user input or null value
        /// </summary>
        /// <param name="prompt">A <see cref="string"/> reprenting prompt</param>
        /// <param name="defaultVal">Optional value to be shown as default value for user input</param>
        /// <returns>A valid <see cref="string"/> or null value</returns>
        private string? GetNullableString(string prompt, string? defaultVal = null)
        {
            var textPrompt = new TextPrompt<string>(prompt)
                .AllowEmpty();

            if (defaultVal is not null)
            {
                textPrompt.DefaultValue(defaultVal);
            }

            var input = AnsiConsole.Prompt(textPrompt);

            return input == string.Empty
                ? null
                : input;

        }
        /// <summary>
        /// Maps <see cref="Exercise"/> to <see cref="string"/> array containing values from record values
        /// </summary>
        /// <param name="exercise">An <see cref="Exercise"/> to be mapped</param>
        /// <returns>A <see cref="string"/> array containing values from record values</returns>
        private string[] GetTableRow(Exercise exercise)
        {
            return new string[]
            {
                exercise.Id.ToString(),
                exercise.Name,
                exercise.WeightLifted.ToString(),
                exercise.DateStart.ToString(_dateFormat),
                exercise.DateEnd.ToString(_dateFormat),
                TimeSpan.FromSeconds(exercise.Duration!.Value).ToString(),
                exercise.Comments ?? "-"

            };
        }


    }
}


