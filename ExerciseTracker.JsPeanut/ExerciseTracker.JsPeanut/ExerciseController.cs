using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExerciseTracker.JsPeanut
{
    public class ExerciseController
    {
        private readonly IExerciseService _service;

        public ExerciseController()
        {
            _service = new ExerciseService();
        }
        public ExerciseController(IExerciseService service)
        {
            _service = service;
        }

       public void InsertExercise()
       {
            DateTime startTime = DateTime.Parse(UserInput.GetStartTime());
            DateTime endTime = DateTime.Parse(UserInput.GetEndTime(startTime.ToString()));
            TimeSpan duration = endTime - startTime;
            string comments = UserInput.GetCommentsString();

            var exercise = new Exercise
            {
                StartTime = startTime,
                EndTime = endTime,
                Duration = duration,
                Comments = comments
            };

            _service.Insert(exercise);
       }

        public List<Exercise> ReadExercises()
        {
            List<Exercise> exercises = _service.GetExercises().ToList();

            UserInterface.ShowExercisesTable(exercises);

            return exercises;
        }

        public void ReadExercise()
        {
            List<Exercise> exercises = ReadExercises();

            string exerciseIdString = UserInput.GetId("\nPlease enter the id of the exercise session you want to see:", exercises);

            int exerciseId = Int32.Parse(exerciseIdString);
            Exercise exercise =_service.Get(exerciseId);
            UserInterface.ShowExercise(exercise);

            Console.WriteLine("\nPress any key to go back to the main menu.");
            Console.ReadKey();
            Program.MainMenu();
        }

        public void Delete()
        {
            List<Exercise> exercises = ReadExercises();

            string exerciseIdString = UserInput.GetId("\nPlease enter the id of the exercise session you want to delete:", exercises);

            int exerciseId = Int32.Parse(exerciseIdString);

            _service.Delete(exerciseId);

            Console.WriteLine("\nExercise session was deleted with success.\n\nPress any key to go back to the main menu.");
            Console.ReadKey();
            Program.MainMenu();
        }

        public void Update()
        {
            List<Exercise> exercises = ReadExercises();

            string exerciseIdString = UserInput.GetId("\nPlease enter the id of the exercise session you want to update", exercises);

            int exerciseId = Int32.Parse(exerciseIdString);

            Exercise exercise = _service.Get(exerciseId);

            exercise.StartTime = DateTime.Parse(UserInput.GetStartTime());
            exercise.EndTime = DateTime.Parse(UserInput.GetStartTime());
            exercise.Duration = exercise.EndTime - exercise.StartTime;
            exercise.Comments = UserInput.GetCommentsString();

            _service.Update(exercise);

            Console.WriteLine("\nExercise session was updated with success.\n\nPress any key to go back to the main menu.");
            Console.ReadKey();
            Program.MainMenu();
        }

    }
}
