using ExerciseTracker.ASV.Repositories;
using ExerciseTracker.ASV.UserInput;
using ExerciseTracker.ASV.Views;
using ExerciseTracker.ASV.Models;

namespace ExerciseTracker.ASV.Services;

public class ExerciseService : IExerciseService
{
    private readonly IInput _input;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IDisplay _display;
    public ExerciseService(IExerciseRepository exerciseRepository, IInput input, IDisplay display)
    {
        _exerciseRepository = exerciseRepository;
        _input = input;
        _display = display;
    }
    public async Task DeleteWorkoutAsync()
    {
        List<ExerciseData> exercises = await _exerciseRepository.GetExercises();
        if (exercises.Count == 0)
        {
            Console.WriteLine("No previous workouts present. Kindly insert some records before deleting.");
            Console.ReadLine();
        }
        else
        {
            _display.DisplayWorkouts(exercises, new string[] { "Id", "DateStart", "DateEnd", "Duration", "Description" }, "Workout list");
            int id = _input.GetWorkoutId();
            ExerciseData data = await _exerciseRepository.GetExerciseById(id);
            if (data.Id == id)
            {
                bool deleteStatus = await _exerciseRepository.DeleteExercise(id);
                if (deleteStatus)
                {
                    Console.WriteLine("Workout deleted successfully.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Some error occured. Cannot delete");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Your entered workoutid was invalid.Cannot delete. Pls enter valid workout id.");
                Console.ReadLine();
            }
        }
    }

    public async Task EditWorkoutAsync()
    {
        List<ExerciseData> exercises = await _exerciseRepository.GetExercises();
        if (exercises.Count == 0)
        {
            Console.WriteLine("No previous workouts present. Kindly insert some records before editing.");
            Console.ReadLine();
        }
        else
        {
            _display.DisplayWorkouts(exercises, new string[] { "Id", "DateStart", "DateEnd", "Duration", "Description" }, "Workout list");
            int id = _input.GetWorkoutId();
            ExerciseData data = await _exerciseRepository.GetExerciseById(id);
            if (data.Id == id)
            {
                Console.WriteLine("Enter the new details.");
                ExerciseData exerciseData = _input.GetWorkoutDetails();
                exerciseData.Id = id;
                bool edited = await _exerciseRepository.PutExercise(exerciseData);
                if (edited)
                {
                    Console.WriteLine("Workout was edited successfully.");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Some error occured.Couldn't edit the workout");
                    Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine("Your entered workoutid was invalid. Pls enter valid workout id.");
                Console.ReadLine();
            }
        }
    }

    public async Task CreateWorkoutAsync()
    {
        ExerciseData exerciseData = _input.GetWorkoutDetails();
        bool workoutPosted = await _exerciseRepository.PostExercise(exerciseData);
        if (workoutPosted)
        {
            Console.WriteLine("Workout saved successfully.");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Some error occured.Couldn't save the workout");
            Console.ReadLine();
        }
    }

    public async Task DisplayAllWorkoutsAsync()
    {
        List<ExerciseData> exercises = await _exerciseRepository.GetExercises();
        if (exercises.Count == 0)
        {
            Console.WriteLine("No previous workouts present to display");
            Console.ReadLine();
        }
        else
        {
            _display.DisplayWorkouts(exercises, new string[] { "Id", "DateStart", "DateEnd", "Duration", "Description" }, "Workout list");
            Console.ReadLine();
        }
    }

    public string GetSelection()
    {
        return _display.GetSelection("Welcome to Exercise Tracker", new[] { "View Past Workouts", "Create a new workout", "Edit a previous workout", "Delete a workout", "Exit Application" });
    }
}