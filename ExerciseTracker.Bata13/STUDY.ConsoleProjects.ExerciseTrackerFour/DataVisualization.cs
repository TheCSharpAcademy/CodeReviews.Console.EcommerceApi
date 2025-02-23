using STUDY.ConsoleProjects.ExerciseTrackerFour.Models;

namespace STUDY.ConsoleProjects.ExerciseTrackerFour;
internal class DataVisualization
{
    public static void ShowDataInTable(List<Exercise> exercises, bool isSingle = false)
    {
        if (isSingle)
        {
            var exercise = exercises[0];
            ShowSingleExercise(exercise);
        }
        else
        {
            foreach (var exercise in exercises)
            {
                ShowSingleExercise(exercise);
            }
        }
    }
    public static void ShowSingleExercise(Exercise exercise)
    {
        TimeSpan duration = exercise.EndTime - exercise.StarTime;
        Console.WriteLine($@"
                Id: {exercise.Id}, 
                StartTime: {exercise.StarTime}, 
                EndTime: {exercise.EndTime}, 
                Duration: {duration},
                Comments: {exercise.Comments}
            ");
    }
}
