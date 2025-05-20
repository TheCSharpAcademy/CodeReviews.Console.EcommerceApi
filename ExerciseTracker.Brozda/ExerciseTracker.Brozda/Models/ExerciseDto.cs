namespace ExerciseTracker.Brozda.Models
{
    internal class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ExerciseType? Type { get; set; }
        public double Volume { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public long? Duration { get; set; }
        public string? Comments { get; set; }
    }
}
