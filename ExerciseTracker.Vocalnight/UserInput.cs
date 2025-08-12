using ConsoleTableExt;
using Exercise_Tracker.Controller;
using Exercise_Tracker.Model;

namespace Exercise_Tracker
{
    internal class UserInput
    {
        private readonly ExcerciseController excerciseController;

        public UserInput(ExcerciseController excerciseController )
        {
            this.excerciseController = excerciseController;
        }

        public void Menu()
        {
            Console.WriteLine("Welcome, what would you like to do? Select with the numpad.");
            Console.WriteLine("1 - Add exercise");
            Console.WriteLine("2 - Remove exercise");
            Console.WriteLine("3 - See exercise list");
            Console.WriteLine("4 - Leave application");
        }


        public void Run()
        {
            while (true) {

                Menu();

                var key = Console.ReadKey(false).Key;

                switch (key)
                {
                    case ConsoleKey.NumPad1:
                        Console.Clear();
                        ExerciseSelection();
                        break;
                    case ConsoleKey.NumPad2:
                        Console.Clear();
                        RemoveExercise();
                        break;
                    case ConsoleKey.NumPad3:
                        Console.Clear();
                        ShowExercises();
                        break;
                    case ConsoleKey.NumPad4:
                        Console.Clear();
                        Console.WriteLine("Bye!");
                        return;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input.");
                        break;

                }
            }
        }

        private void RemoveExercise()
        {
            int id;

            while (true)
            {
                ShowExercises();
                Console.WriteLine("\nType the Id of the Exercise you wish to remove.");

                if (Int32.TryParse(Console.ReadLine(), out id)) {
                    excerciseController.RemoveExercise(id);
                    break;

                } 
                else
                {
                    Console.Clear();
                    Console.WriteLine("\nNot a valid number, try again");
                }
                
            }
        }

        private void ShowExercises()
        {
            var tableBuilder = ConsoleTableBuilder.From((List<Exercise>)excerciseController.GetExercises())
            .WithFormat(ConsoleTableBuilderFormat.Minimal);

            // Print the table to the console
             tableBuilder.ExportAndWriteLine();
            Console.WriteLine("\n");
        }

        private void ExerciseSelection()
        {
            Console.WriteLine("What exercise would you like to track? Select with the numpad.");
            Console.WriteLine("1 - Push-Ups");
            Console.WriteLine("2 - Cardio");
            Console.WriteLine("3 - Exit");

            var key = Console.ReadKey(false).Key;
            Console.Clear();

            if (key == ConsoleKey.NumPad1)
            {
                AddNewExercise(ExerciseEnum.PushUp);

            }
            else if (key == ConsoleKey.NumPad2)
            {
                AddNewExercise(ExerciseEnum.Cardio);

            }
            else if (key == ConsoleKey.NumPad3)
            {
                Console.WriteLine("Bye!");
                return;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid input.");
            }
        }

        private void AddNewExercise(ExerciseEnum exerciseType)
        {
            var exercise = new Exercise();


            Console.WriteLine("Insert the start time (Format HH:mm)");
            Console.WriteLine("\nMake sure the ending time is higher than the starting time!");

            string startTime = excerciseController.checkTimeValidity(Console.ReadLine());

            Console.WriteLine("Insert the end time (Format HH:mm)");
            Console.WriteLine("\nMake sure the ending time is higher than the starting time!");

            string endTime = excerciseController.checkTimeValidity(Console.ReadLine());

            endTime = excerciseController.checkTimeChronology(startTime, endTime);

            //Get only the date without time.
            DateTime dateTimeNow = DateTime.Now;
            string dateOnlyString = dateTimeNow.ToString("dd/MM/yyyy");

            DateTime startingTimeDate = DateTime.ParseExact($"{dateOnlyString} {startTime}", "dd/MM/yyyy HH:mm",
                                          System.Globalization.CultureInfo.InvariantCulture);

            DateTime endTimeDate = DateTime.ParseExact($"{dateOnlyString} {endTime}", "dd/MM/yyyy HH:mm",
                                          System.Globalization.CultureInfo.InvariantCulture);

            exercise.ExerciseType = exerciseType;
            exercise.DateEnd = startingTimeDate;
            exercise.DateStart = endTimeDate;
            exercise.Duration = endTimeDate - startingTimeDate;

            Console.WriteLine("Would you like to add any extra comments? Type anything");

            string? comment = Console.ReadLine();

            exercise.Comments = comment;

            excerciseController.AddExercise(exercise);
            Console.Clear();

            Console.WriteLine("Added sucessfully");
            Console.WriteLine("----------");

        }
    }
}
