using ExerciseTracker.Brozda.Helpers;
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
        private string _dateFormat = AppStrings.IoDateFormat;

        public void ClearConsole()
        {
            Console.Clear();
        }

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
            Console.WriteLine(errorMsg ?? AppStrings.IoUnhandledError);
        }
        /// <summary>
        /// Prints "Press any Key to continue" and awaits user input, effectively pausing the app flow
        /// </summary>
        public void PrintPressAnyKeyToContinue()
        {
            Console.WriteLine(AppStrings.IoPressAnyKeyToContinue);
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
                .Title(AppStrings.IoSelectMenu)
                .AddChoices(menuOptions.Select(x => x.Key).ToList())
                .UseConverter(x => menuOptions[x])
                );

            return input;
        }
        /// <summary>
        /// Prints exercises to the user console in form of Table
        /// </summary>
        /// <param name="exercises">A List of <see cref="Exercise"/> to be printed</param>
        public void PrintExercises(List<ExerciseDto> exercises)
        {
            var table = new Table();
            table.AddColumns("Id", "Name", "Volume", "Start", "End", "Duration", "Comments");
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
        public void PrintExercise(ExerciseDto exercise)
        {
            var table = new Table();
            table.AddColumns("Id", "Name", "Volume", "Start", "End", "Duration", "Comments");
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
        public int GetRecordId(List<ExerciseDto> exercises)
        {
            PrintExercises(exercises);

            var validIds = exercises.Select(x => x.Id).ToList();

            var selectedId = AnsiConsole.Prompt(
                new TextPrompt<int>(AppStrings.IoSelectRecordId)
                .Validate(x => validIds.Contains(x) || x == 0)
                );

            return selectedId;
        }
        /// <summary>
        /// Retrieves values for <see cref="Exercise"/> 
        /// </summary>
        /// <param name="existing">Optional argument of existing <see cref="Exercise"/>. If present then its values will be used as default values</param>
        /// <returns>A <see cref="Exercise"/> containing values from user input</returns>
        /*public ExerciseDto GetExercise(List<ExerciseType> exTypes, ExerciseDto? existing = null)
        {
            string name = GetString(AppStrings.IoExerciseName, existing?.Name);
            int typeId = GetExerciseTypeId(exTypes, existing);

            string unit = exTypes.Find(x => x.Id == typeId)!.Unit;

            double volume = GetDouble($"{AppStrings.IoVolume} ({unit}): ", existing?.Volume);
            DateTime start = GetDate(AppStrings.IoDateStart, existing?.DateStart); ;
            DateTime end = GetDate(AppStrings.IoDateEnd, existing?.DateEnd, start); ;
            long duration = (long)(end - start).TotalSeconds;
            string? comments = GetNullableString(AppStrings.IoComment, existing?.Comments);

            return new ExerciseDto()
            {
                Name = name,
                TypeId = typeId,
                Volume = volume,
                DateStart = start,
                DateEnd = end,
                Duration = duration,
                Comments = comments
            };

        }*/

        public ExerciseDto GetExercise(ExerciseType exerciseType, ExerciseDto? existing = null)
        {
            string name = GetString(AppStrings.IoExerciseName, existing?.Name);

            double volume = GetDouble($"{AppStrings.IoVolume} ({exerciseType.Unit}): ", existing?.Volume);
            DateTime start = GetDate(AppStrings.IoDateStart, existing?.DateStart); ;
            DateTime end = GetDate(AppStrings.IoDateEnd, existing?.DateEnd, start); ;
            long duration = (long)(end - start).TotalSeconds;
            string? comments = GetNullableString(AppStrings.IoComment, existing?.Comments);

            return new ExerciseDto()
            {
                Name = name,
                TypeId = exerciseType.Id,
                Volume = volume,
                DateStart = start,
                DateEnd = end,
                Duration = duration,
                Comments = comments
            };

        }
        public void PrintExerciseTypes(List<ExerciseType> exTypes)
        {
            var table = new Table();
            table.AddColumns("Id", "Type");
            foreach (var exerciseType in exTypes)
            {
                table.AddRow(new string[] { exerciseType.Id.ToString(), exerciseType.Name });
            }
            AnsiConsole.Write(table);
        }
        public int GetExerciseTypeId(List<ExerciseType> exTypes, ExerciseDto? existing = null!)
        {
            var validIds = exTypes.Select(x => x.Id);

            PrintExerciseTypes(exTypes);

            var prompt = new TextPrompt<int>(AppStrings.IoSelectExerciseType)
                .Validate(x => validIds.Contains(x));
            
            if(existing is not null)
            {
                prompt.DefaultValue(existing.TypeId);
            }

            return AnsiConsole.Prompt(prompt);
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
                    return ValidationResult.Error(AppStrings.IoErrorDateFormat);
                if (endDate < startDate)
                    return ValidationResult.Error(AppStrings.IoErrorStartBeforeEnd);

                return ValidationResult.Success();
            }
            else
            {
                if (!DateTime.TryParseExact(dateString, _dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    return ValidationResult.Error(AppStrings.IoErrorDateFormat);

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

            return (input == AppStrings.IoNullValueChar || input == string.Empty)
                ? null
                : input;

        }
        /// <summary>
        /// Maps <see cref="Exercise"/> to <see cref="string"/> array containing values from record values
        /// </summary>
        /// <param name="exercise">An <see cref="Exercise"/> to be mapped</param>
        /// <returns>A <see cref="string"/> array containing values from record values</returns>
        private string[] GetTableRow(ExerciseDto dto)
        {
            return new string[]
            {
                dto.Id.ToString(),
                dto.Name,
                dto.Volume.ToString() + dto.Unit,
                dto.DateStart.ToString(_dateFormat),
                dto.DateEnd.ToString(_dateFormat),
                TimeSpan.FromSeconds(dto.Duration!.Value).ToString(),
                dto.Comments ?? AppStrings.IoNullValueChar

            };
        }
        

    }
}


