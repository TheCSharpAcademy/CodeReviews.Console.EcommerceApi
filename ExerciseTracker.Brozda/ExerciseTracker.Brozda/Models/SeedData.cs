namespace ExerciseTracker.Brozda.Models
{
    /// <summary>
    /// Represents data used for autoseed functionality
    /// </summary>
    internal class SeedDataEf
    {
        public List<Exercise> ExercisesWeight { get; set; } = null!;
        public List<ExerciseType> ExerciseTypes { get; set; } = null!;
        public List<Exercise> ExercisesCardio { get; set; } = null!;
    }
}