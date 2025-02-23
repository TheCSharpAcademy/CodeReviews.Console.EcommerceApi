using ExerciceTracker.Data.Models;
using System.Globalization;

namespace ExerciceTracker
{
    internal class UserInput
    {

        internal static Exercise GetUserInputExercise()
        {
            var exercise = new Exercise();
            DateTime startTime, endTime;
            bool isValidStartTime = false;
            bool isValidEndTime = false;
            // get start time
            while (!isValidStartTime)
            {
                Console.WriteLine("What's the start time? (dd/mm/yyyy hh:mm) \n");
                string userInputStart = Console.ReadLine();

                if (DateTime.TryParseExact(userInputStart, "d/M/yyyy H:m", CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
                {
                    exercise.DateStart = startTime;
                    isValidStartTime = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. (dd/mm/yyyy hh:mm)");
                } 
            }

            // get end time
            while (!isValidEndTime)
            {
                Console.WriteLine("What's the end time? (dd/mm/yyyy hh:mm) \n");
                string userInputEnd = Console.ReadLine();

                if (DateTime.TryParseExact(userInputEnd, "d/M/yyyy H:m", CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
                {
                    if (endTime < exercise.DateStart)
                    {
                        Console.WriteLine("Invalid input, end time can't be earlier than start time.");
                    }
                    else
                    {
                        exercise.DateEnd = endTime;
                        isValidEndTime = true;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. (dd/mm/yyyy hh:mm)");
                } 
            }

            // get duration
            TimeSpan duration;
            duration = exercise.DateEnd - exercise.DateStart;
            exercise.Duration = duration;

            // get comments
            Console.WriteLine("Want to add a comment?\n");
            string userInputComments = Console.ReadLine();
            exercise.Comments = userInputComments;

            return exercise;
        }
    }
}
