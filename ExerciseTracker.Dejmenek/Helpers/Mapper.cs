using ExerciseTracker.Dejmenek.Models;

namespace ExerciseTracker.Dejmenek.Helpers;
public static class Mapper
{
    public static ExerciseReadDTO ToExerciseReadDto(Exercise exercise)
    {
        return new ExerciseReadDTO
        {
            StartTime = exercise.StartTime,
            EndTime = exercise.EndTime,
            Duration = exercise.Duration,
            Comments = exercise.Comments,
        };
    }

    public static List<ExerciseReadDTO> ToExerciseReadDtos(List<Exercise> exercises)
    {
        List<ExerciseReadDTO> exerciseDtos = new List<ExerciseReadDTO>();

        foreach (var exercise in exercises)
        {
            exerciseDtos.Add(ToExerciseReadDto(exercise));
        }

        return exerciseDtos;
    }
}
