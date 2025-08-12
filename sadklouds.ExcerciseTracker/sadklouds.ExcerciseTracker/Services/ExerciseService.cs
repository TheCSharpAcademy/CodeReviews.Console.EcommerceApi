using sadklouds.ExcerciseTracker.DataInput;
using sadklouds.ExcerciseTracker.Models;
using sadklouds.ExcerciseTracker.Repositries;

namespace sadklouds.ExcerciseTracker.Services;
public class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUserInput _userInput;

    public ExerciseService(IExerciseRepository _exerciseRepository, IUserInput _userInput)
    {
        this._exerciseRepository = _exerciseRepository;
        this._userInput = _userInput;
    }

    public void AddExercise()
    {
        DateTime startDate = _userInput.GetStartDate();
        DateTime endDate = _userInput.GetEndDate(startDate);
        TimeSpan duration = endDate - startDate;
        string comment = _userInput.GetComment();
        ExerciseModel excercise = new ExerciseModel()
        {
            StartDate = startDate,
            EndDate = endDate,
            Duration = duration,
            Comments = comment
        };
        _exerciseRepository.Add(excercise);
    }

    public void DeleteExercise()
    {
        GetAllExercises();
        int id = _userInput.GetIdInput();
        var exercise = _exerciseRepository.GetById(id);
        if (exercise == null)
            Console.WriteLine($"Excercise with Id:{id} was  not found!");
        else
        {
            _exerciseRepository.Delete(exercise);
            Console.WriteLine($"Exercise Id:{id} was deleted");
        }
    }

    public void GetAllExercises()
    {
        List<ExerciseModel> exercises = _exerciseRepository.GetAll().ToList();
        if (exercises == null)
            Console.WriteLine("No excercises where found");
        else
        {
            Console.WriteLine("----------------------------------------------------------------------------------------");
            foreach (var exercise in exercises)
            {
                Console.WriteLine($"Id:{exercise.Id}, Start:{exercise.StartDate}, End:{exercise.EndDate}, Duration:{exercise.Duration}");
                Console.WriteLine($"Comment:{exercise.Comments}");
                Console.WriteLine("----------------------------------------------------------------------------------------\n");
            }
        }
    }

    public void GetExercise()
    {
        int id = _userInput.GetIdInput();
        var exercise = _exerciseRepository.GetById(id);
        if (exercise == null)
            Console.WriteLine($"Excercise with Id: {id} was  not found!");
        else
            Console.WriteLine($"\nId:{exercise.Id}, Start:{exercise.StartDate}, End:{exercise.EndDate}, Duration:{exercise.Duration}\n");
    }

    public void UpdateExercise()
    {
        GetAllExercises();
        int id = _userInput.GetIdInput();
        var currentExercise = _exerciseRepository.GetById(id);
        if (currentExercise == null)
        {
            Console.WriteLine("Excercise could not be found");
            return;
        }
        DateTime startDate = _userInput.GetStartDate();
        DateTime endDate = _userInput.GetEndDate(startDate);
        TimeSpan duration = endDate - startDate;
        string comment = _userInput.GetComment();
        ExerciseModel updatedModel = new ExerciseModel()
        {
            StartDate = startDate,
            EndDate = endDate,
            Duration = duration,
            Comments = comment
        };
        _exerciseRepository.Update(updatedModel, currentExercise);
    }
}
