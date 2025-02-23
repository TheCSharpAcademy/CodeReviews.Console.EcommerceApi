using ExerciceTracker.Data.Repositories;

namespace ExerciceTracker.Services
{
    internal class ExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseService(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public void AddService()
        {
            Console.Clear();
            var exercise = UserInput.GetUserInputExercise();
            _exerciseRepository.Add(exercise);
        }
        public void DeleteService()
        {
            Console.Clear();
            var exercises = _exerciseRepository.GetAll().ToList();
            if (exercises.Count() == 0)
            {
                Console.WriteLine("The database is empty");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Enter the ID you want to delete");
                int id = int.Parse(Console.ReadLine());
                _exerciseRepository.Delete(id);
            }
        }
        public void GetAllService()
        {
            Console.Clear();
            var exercises = _exerciseRepository.GetAll().ToList();
            if (exercises.Count == 0)
            {
                Console.WriteLine("Db is empty");
                Console.ReadLine();
            }
            else
            {
                foreach (var exercise in exercises)
                {
                    Console.WriteLine($"Id: {exercise.Id}, Start: {exercise.DateStart}, End: {exercise.DateEnd}, Duration: {exercise.Duration}, Comments: {exercise.Comments}");
                }
            }
        }
    }
}
