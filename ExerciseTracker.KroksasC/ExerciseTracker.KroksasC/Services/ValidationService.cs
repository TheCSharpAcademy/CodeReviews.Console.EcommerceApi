using System.Text.RegularExpressions;

namespace ExerciseTracker.KroksasC.Services
{
    internal class ValidationService
    {
        public static string ValidateString(string? name)
        {
            name = name.Trim();

            string pattern = @"^[A-Za-z]+(?: [A-Za-z]+)*";

            while (!Regex.IsMatch(name, pattern))
            {
                Console.WriteLine("Name entered incorrectly! Please try again!");
                name = Console.ReadLine();
                name = name.Trim();
            }
            return name;
        }
        public static DateTime ValidateDate(string? date)
        {
            DateTime result;
            while (true)
            {
                string format = "yyyy-MM-dd HH:mm:ss";
                if (DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out result))
                {
                    return result; // Return the valid DateTime
                }
                else
                {
                    Console.WriteLine("Invalid time format. Please use yyyy-MM-dd HH:mm:ss. Enter again:" +
                        "");
                    date = Console.ReadLine();
                }
            }
        }
        public static int ValidateNumber(string? number)
        {
            int result;
            
            while (!int.TryParse(number, out result) || result <= 0)
            {
                Console.WriteLine("Only postive number is allowed! Try again");
                number = Console.ReadLine();
            }
            return result;
        }
    }
}
