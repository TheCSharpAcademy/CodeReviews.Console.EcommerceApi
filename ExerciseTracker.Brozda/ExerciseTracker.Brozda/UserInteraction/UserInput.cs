using ExerciseTracker.Brozda.Models;
using Spectre.Console;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExerciseTracker.Brozda.UserInteraction
{
    internal class UserInput
    {
        private string _dateFormat = "yyyy-MM-dd HH:mm";

        public UserInput()
        {
            
        }
        public void PrintError(string error)
        {
            Console.WriteLine(error);
        }
        public void PrintText(string text)
        {
            Console.WriteLine(text);
        }
        public void PrintPressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
            Console.Clear();
        }
        /*public void ConsoleClear()
        {
            Console.Clear();
        }*/
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
        public void PrintExercises(List<Exercise> exercises)
        {
            var table = new Table();
            table.AddColumns("Id", "Name", "Weight Lifted", "Start", "End", "Duration", "Comments");
            foreach(var exercise in exercises)
            {
                table.AddRow(GetTableRow(exercise));
            }

            AnsiConsole.Write(table);   
        }
        public void PrintExercise(Exercise exercise)
        {
            var table = new Table();
            table.AddColumns("Id", "Name", "Weight Lifted", "Start", "End", "Duration", "Comments");
            table.AddRow(GetTableRow(exercise));

            AnsiConsole.Write(table);
        }
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
        public Exercise GetExercise(Exercise? existing)
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
        private ValidationResult ValidateDateTime(string dateString, DateTime? startDate)
        {
            if (startDate is not null) 
            {
                DateTime endDate;

                if (!DateTime.TryParseExact(dateString, _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
                    return ValidationResult.Error("Invalid date format, format needs to be yyyy-mm-dd hh:mm 24H format");
                if(endDate < startDate)
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
        private double GetDouble(string prompt,double? defaultVal = null)
        {
            var textPrompt = new TextPrompt<double>(prompt);

            if (defaultVal is not null)
            {
                textPrompt.DefaultValue(defaultVal.Value);
            }

            return AnsiConsole.Prompt(textPrompt);

        }
        private string GetString(string prompt, string? defaultVal = null)
        {
            var textPrompt = new TextPrompt<string>(prompt);

            if(defaultVal is not null)
            {
                textPrompt.DefaultValue(defaultVal);
            }

            return AnsiConsole.Prompt(textPrompt);
                
        }
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


