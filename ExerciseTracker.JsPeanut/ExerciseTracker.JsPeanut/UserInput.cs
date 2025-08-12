using Spectre.Console;

namespace ExerciseTracker.JsPeanut
{
    internal class UserInput
    {
        public static string GetStartTime()
        {
            string startTime = AnsiConsole.Ask<string>("Date and time in which your exercise session started:");

            while (!Validator.ValidateStartDateString(startTime))
            {
                startTime = AnsiConsole.Ask<string>("Date and time in which your exercise session started:");
            }

            return startTime;
        }

        public static string GetEndTime(string startTime)
        {
            string endTime = AnsiConsole.Ask<string>("Date and time in which your exercise session ended:");

            while (!Validator.ValidateEndDateString(startTime, endTime))
            {
                endTime = AnsiConsole.Ask<string>("Date and time in which your exercise session started:");
            }

            return endTime;
        }

        public static string GetCommentsString()
        {
            string comment = AnsiConsole.Ask<string>("Comments?");

            while (!Validator.IsStringValid(comment))
            {
                comment = AnsiConsole.Ask<string>("Comments?");
            }

            return comment;
        }

        public static string GetId(string message, List<Exercise> exercises)
        {
            string id = AnsiConsole.Ask<string>(message);

            while (!Validator.IsIdValid(id, exercises))
            {
                id = AnsiConsole.Ask<string>(message);
            }

            return id;
        }
    }
}
