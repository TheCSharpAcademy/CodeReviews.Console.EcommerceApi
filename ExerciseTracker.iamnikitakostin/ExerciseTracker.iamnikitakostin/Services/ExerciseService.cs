using ExerciseTracker.iamnikitakostin.Models;
using ExerciseTracker.iamnikitakostin.Repositories;

namespace ExerciseTracker.iamnikitakostin.Services;
internal class ExerciseService : ConsoleHelper
{
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseService(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    public void Add(Exercise exercise)
    {
        try
        {
            bool isSuccess = _exerciseRepository.Add(exercise);

            if (!isSuccess)
            {
                ErrorMessage($"There has been an error while adding the record.\nPlease, make sure that your DB is connected properly.");
                return;
            }
            SuccessMessage("The record has been added.");
        }
        catch (Exception ex)
        {
            ErrorMessage($"There has been an error: {ex.Message}");
        }
    }
    public void Update(Exercise exercise)
    {
        try
        {
            bool isSuccess = _exerciseRepository.Update(exercise);

            if (!isSuccess)
            {
                ErrorMessage($"There has been an error while updating the record.\nPlease, make sure that your DB is connected properly.");
                return;
            }
            SuccessMessage("The record has been updated.");
        }
        catch (Exception ex)
        {
            ErrorMessage($"There has been an error: {ex.Message}");
        }
    }
    public void Delete(int id)
    {
        try
        {
            bool isSuccess = _exerciseRepository.Delete(id);

            if (!isSuccess)
            {
                ErrorMessage($"There has been an error while deleting the record.\nPlease, make sure that your DB is connected properly.");
                return;
            }
            SuccessMessage("The record has been deleted.");
        }
        catch (Exception ex)
        {
            ErrorMessage($"There has been an error: {ex.Message}");
        }
    }
    public Exercise? GetById(int id)
    {
        try
        {
            Exercise exercise = _exerciseRepository.GetById(id);

            if (exercise == null)
            {
                ErrorMessage("There has been an error while finding the exercise, make sure it has a correct id.");
                return null;
            }

            return exercise;
        }
        catch (Exception ex)
        {
            ErrorMessage($"There has been an error: {ex.Message}");
            return null;
        }
    }
    public List<Exercise>? GetAll()
    {
        try
        {
            List<Exercise> exercises = _exerciseRepository.GetAll();
            return exercises;
        }
        catch (Exception ex)
        {
            ErrorMessage($"There has been an error: {ex.Message}");
            return null;
        }
    }

    public Dictionary<int, string> GetAllAsDictionary()
    {
        try
        {
            Dictionary<int, string> exercises = _exerciseRepository.GetAllAsDictionary();
            return exercises;
        }
        catch (Exception ex)
        {
            ErrorMessage($"There has been an error: {ex.Message}");
            return null;
        }
    }
}