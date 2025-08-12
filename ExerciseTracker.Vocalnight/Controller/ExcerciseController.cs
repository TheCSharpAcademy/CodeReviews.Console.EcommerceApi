using Exercise_Tracker.Model;
using Exercise_Tracker.Services;
using System.Globalization;

namespace Exercise_Tracker.Controller
{
    internal class ExcerciseController
    {
        private readonly ExcerciseService _service;

        public ExcerciseController(ExcerciseService service)
        {
            _service = service;
        }

        public void AddExercise(Exercise exercise)
        {
            _service.AddExercise(exercise);
        }

        public IEnumerable<Exercise> GetExercises()
        {
            return _service.GetExercises();
        }

        public void RemoveExercise(int id)
        {
            _service.RemoveRegistry(id);
        }

        public string checkTimeValidity( string time )
        {
            while (!DateTime.TryParseExact(time, "HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.Clear();
                Console.WriteLine("\nInvalid time. Try again:\n");
                time = Console.ReadLine();
            }

            return time;
        }

        public string checkTimeChronology( string startTime, string endTime )
        {
            DateTime start = DateTime.Parse(startTime);
            DateTime end = DateTime.Parse(endTime);

            while (DateTime.Compare(start, end) > 0)
            {
                Console.Clear();
                Console.WriteLine("The end time is earlier than the start time, please insert a valid time!");
                endTime = Console.ReadLine();
                end = DateTime.Parse(endTime);
            }

            return endTime;
        }
    }
}
