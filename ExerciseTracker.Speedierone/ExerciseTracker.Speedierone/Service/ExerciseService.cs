using ExerciseTracker.Speedierone.Model;
using ExerciseTracker.Speedierone.Repository;

namespace ExerciseTracker.Speedierone.Service
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly UserInput _userInput;

        public ExerciseService(IExerciseRepository exerciseRepository, UserInput userInput)
        {
            _exerciseRepository = exerciseRepository;
            _userInput = userInput;
        }
        public IEnumerable<Exercises> GetAllExercises()
        {
            return _exerciseRepository.GetAll();
        }
        public List<Exercises> GetExerciseById(int id)
        {
            int IdToFind = id;
            List<Exercises> exerciseFound = _exerciseRepository.GetById(IdToFind);
            List<Exercises> resultList = new List<Exercises>();
            if (exerciseFound != null)
            {
                resultList.AddRange(exerciseFound);
            }
            return resultList;
        }

        public void AddExercise(Exercises exercises)
        {
            Console.WriteLine("Please enter start date in format dd/mm/yyyy");
            var startDateAdd = UserInput.CheckDate();

            Console.WriteLine("Please enter start time in format hh:mm");
            var startTimeAdd = UserInput.CheckTime();

            var combinedStartDateAdd = _userInput.CombinedDateTime(startDateAdd, startTimeAdd);
            var stringCombinedStartDateAdd = combinedStartDateAdd.ToString();

            Console.WriteLine("Please enter end date in format dd/mm/yyyy");
            var endDateAdd = UserInput.CheckDate();

            Console.WriteLine("Please enter end time in format hh:mm");
            var endTimeAdd = UserInput.CheckTime();

            var combinedEndDateAdd = _userInput.CombinedDateTime(endDateAdd, endTimeAdd);
            var stringCombinedEndDateAdd = combinedEndDateAdd.ToString();

            var checkEndDate = UserInput.CheckEndDate(stringCombinedStartDateAdd, stringCombinedEndDateAdd);

            while (checkEndDate == false)
            {
                Console.WriteLine("Please enter endDate");
                endDateAdd = UserInput.CheckDate();
                Console.WriteLine("Please enter end Time");
                endTimeAdd = UserInput.CheckTime();
                combinedEndDateAdd = _userInput.CombinedDateTime(endDateAdd, endTimeAdd);
                stringCombinedEndDateAdd = combinedEndDateAdd.ToString();

                checkEndDate = UserInput.CheckEndDate(stringCombinedStartDateAdd, stringCombinedEndDateAdd);
            }
            var durationAdd = _userInput.GetDuration(combinedStartDateAdd, combinedEndDateAdd);

            Console.WriteLine("Please enter any comments you wish to make. Press enter when finished or wish to leave it blank.");
            var comments = Console.ReadLine();

            var exercise = new Exercises
            {
                DateStart = combinedStartDateAdd,
                DateEnd = combinedEndDateAdd,
                Duration = durationAdd,
                Comments = comments
            };
            _exerciseRepository.Add(exercise);
            _exerciseRepository.SaveChanges();
        }
        public void UpdateExercise(Exercises exercisesToUpdate)
        {
            Console.WriteLine("Please enter start date in format dd/mm/yyyy");
            var startDate = UserInput.CheckDate();

            Console.WriteLine("Please enter start time in format hh:mm");
            var startTime = UserInput.CheckTime();

            var combinedStartDate = _userInput.CombinedDateTime(startDate, startTime);
            var stringCombinedStartDate = combinedStartDate.ToString();

            Console.WriteLine("Please enter end date in format dd/mm/yyyy");
            var endDate = UserInput.CheckDate();

            Console.WriteLine("Please enter end time in format hh:mm");
            var endTime = UserInput.CheckTime();

            var combinedEndDate = _userInput.CombinedDateTime(endDate, endTime);
            var stringCombinedEndDate = combinedEndDate.ToString();

            var checkEndDate = UserInput.CheckEndDate(stringCombinedStartDate, stringCombinedEndDate);

            if (!checkEndDate)
            {
                Console.WriteLine("End date is before start date. Press any key to try again.");
                endDate = UserInput.CheckDate();
                endTime = UserInput.CheckTime();

                combinedEndDate = _userInput.CombinedDateTime(endDate, endTime);
                stringCombinedEndDate = combinedEndDate.ToString();

                checkEndDate = UserInput.CheckEndDate(stringCombinedStartDate, stringCombinedEndDate);

                while (!checkEndDate)
                {
                    endDate = UserInput.CheckDate();
                    endTime = UserInput.CheckTime();
                    combinedEndDate = _userInput.CombinedDateTime(endDate, endTime);
                    stringCombinedEndDate = combinedEndDate.ToString();
                    checkEndDate = UserInput.CheckEndDate(stringCombinedStartDate, stringCombinedEndDate);
                }
            }
            var duration = _userInput.GetDuration(combinedStartDate, combinedEndDate);

            Console.WriteLine("Please enter any comments you wish to make. Press enter when finished or wish to leave it blank.");
            var comments = Console.ReadLine();

            exercisesToUpdate.DateStart = combinedStartDate;
            exercisesToUpdate.DateEnd = combinedEndDate;
            exercisesToUpdate.Duration = duration;
            exercisesToUpdate.Comments = comments;
        }
        public void DeleteExercise(int id)
        {
            _exerciseRepository.Delete(id);
            _exerciseRepository.SaveChanges();
        }
    }
}
