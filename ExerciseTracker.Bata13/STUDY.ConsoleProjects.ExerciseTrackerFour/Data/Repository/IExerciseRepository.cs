using STUDY.ConsoleProjects.ExerciseTrackerFour.Models;

namespace STUDY.ConsoleProjects.ExerciseTrackerFour.Data.Repository;
public interface IExerciseRepository
{
    public Exercise ViewSpecificExerciseEntry(int exerciseId);
    void AddExerciseEntry(Exercise exercise);
    void UpdateExerciseEntry(int exerciseId, Exercise newExercise);
    void DeleteExerciseEntry(int exerciseId);
    public Exercise GetExerciseEntryById(int exerciseId);
    public List<Exercise> GetExercises();
}
