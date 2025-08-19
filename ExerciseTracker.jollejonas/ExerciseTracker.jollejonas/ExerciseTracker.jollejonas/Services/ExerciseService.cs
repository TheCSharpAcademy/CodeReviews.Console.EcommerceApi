using ExerciseTracker.jollejonas.Models;
using ExerciseTracker.jollejonas.Repositories;
using ExerciseTracker.jollejonas.UserInput;
public class ExerciseService
{
    private readonly IExerciseRepository _exercieseRepository;
    private readonly IUserInput _userInput;

    public ExerciseService(IExerciseRepository exerciseRepository, IUserInput userInput)
    {
        _exercieseRepository = exerciseRepository;
        _userInput = userInput;
    }

    public void AddExercise()
    {
        var exercise = new Exercise();
        Console.WriteLine("Start time: ");
        exercise.DateStart = _userInput.GetDateTime();
        Console.WriteLine("End time: ");
        exercise.DateEnd = _userInput.GetDateTime();

        exercise.Duration = CalculateDuration(exercise.DateStart, exercise.DateEnd);

        exercise.Comments = _userInput.GetExerciseComments();

        try
        {

            _exercieseRepository.AddExercise(exercise);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void DeleteExercise()
    {
        Exercise selectedExercise = _userInput.GetExercise(_exercieseRepository.GetAllExercises());
        try
        {
            _exercieseRepository.DeleteExercise(selectedExercise);

        }

        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public List<Exercise> GetAllExercises()
    {
        try
        {
            foreach (var exercise in _exercieseRepository.GetAllExercises())
            {
                Console.WriteLine($"{exercise.DateStart} - {exercise.DateEnd} - {exercise.Duration} - {exercise.Comments}");
            }
            return _exercieseRepository.GetAllExercises();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public void UpdateExercise()
    {
        Exercise selectedExercise = _userInput.GetExercise(_exercieseRepository.GetAllExercises());

        if (selectedExercise == null)
        {
            Console.WriteLine("Exercise not found.");
            return;
        }

        var exercise = _exercieseRepository.GetExerciseById(selectedExercise.Id);


        if (_userInput.GetConfirmation("Do you want to update the start time? (y/n)"))
        {
            Console.WriteLine("Start time: ");
            exercise.DateStart = _userInput.GetDateTime();
        }
        if (_userInput.GetConfirmation("Do you want to update the end time? (y/n)"))
        {
            Console.WriteLine("End time: ");
            exercise.DateEnd = _userInput.GetDateTime();
            exercise.Duration = CalculateDuration(exercise.DateStart, exercise.DateEnd);
        }

        if (_userInput.GetConfirmation("Do you want to update the comments? (y/n)"))
        {
            exercise.Comments = _userInput.GetExerciseComments();
        }

        try
        {

            _exercieseRepository.UpdateExercise(exercise);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }

    public TimeSpan CalculateDuration(DateTime dateStart, DateTime dateEnd)
    {
        return dateEnd - dateStart;
    }
}
