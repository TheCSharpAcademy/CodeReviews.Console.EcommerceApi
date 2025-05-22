using ExerciseTracker.KamilKolanowski.Enums;

namespace ExerciseTracker.KamilKolanowski.Interfaces;

public interface IExerciseRepositoryFactory
{
    IExerciseRepository GetExerciseRepository(ExerciseType exerciseType);
}