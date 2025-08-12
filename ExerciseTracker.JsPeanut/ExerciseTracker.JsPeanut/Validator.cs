using System.Globalization;

namespace ExerciseTracker.JsPeanut
{
    internal class Validator
    {
        public static bool ValidateStartDateString(string startTimeString)
        {
            string format = "dd/MM/yyyy HH:mm";
            CultureInfo culture = CultureInfo.InvariantCulture;
            DateTimeStyles styles = DateTimeStyles.None;
            bool isDateValid = DateTime.TryParseExact(startTimeString, format, culture, styles, out _);

            if (isDateValid == false)
            {
                Console.WriteLine("Wrong format. You can only use the one provided that is dd/mm/yyyy hh:mm. Example: 01/01/2023 12:00 ");

                return false;
            }

            return true;
        }

        public static bool ValidateEndDateString(string startTimeString, string endTimeString)
        {
            string format = "dd/MM/yyyy HH:mm";
            CultureInfo culture = CultureInfo.InvariantCulture;
            DateTimeStyles styles = DateTimeStyles.None;

            bool isDateValid = DateTime.TryParseExact(endTimeString, format, culture, styles, out _);
            DateTime startTime = DateTime.Parse(startTimeString);
            DateTime endTime = DateTime.Parse(endTimeString);

            if (!isDateValid)
            {
                Console.WriteLine("Wrong format. You can only use the one provided that is dd/mm/yyyy hh:mm. Example: 01/01/2023 12:00 ");

                return false;
            }
            if (endTime < startTime)
            {
                Console.WriteLine("The time in which your workout ended was before it even started! Try again, or go back to the main menu to insert the time in which your workout started once again.");

                return false;
            }

            return true;
        }

        public static bool IsStringValid(string stringToValidate)
        {
            if (String.IsNullOrEmpty(stringToValidate))
            {
                Console.WriteLine("You can't leave this field empty!");

                return false;
            }

            foreach (char c in stringToValidate)
            {
                if (!Char.IsLetter(c) && c != ' ' && c != '?' && c != '!' && c != ',' && c != '.' && c != ';')
                {
                    Console.WriteLine("Your comment must only contain letters. Please don't use numbers or special characters and try again");
                    return false;
                }
            }

            return true;
        }

        public static bool IsIdValid(string idToValidate, List<Exercise> exerciseList)
        {
            if (!Int32.TryParse(idToValidate, out _))
            {
                Console.WriteLine("You didn't enter a valid id. Please enter a number.");

                return false;
            }

            if (!exerciseList.Any(e => e.Id == Int32.Parse(idToValidate)))
            {
                Console.WriteLine("No exercise contains that id. Please check your exercises and try again.");

                return false;
            }

            return true;
        }
    }
}
