namespace ExerciseTracker.Brozda.Models
{
    /// <summary>
    /// Represents model for ExerciseTypes Table
    /// </summary>
    internal class ExerciseType
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public string Unit { get; set; } = null!;
    }
}
