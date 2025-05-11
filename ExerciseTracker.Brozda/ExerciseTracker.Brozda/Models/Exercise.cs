namespace ExerciseTracker.Brozda.Models
{
    internal class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double WeightLifted { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public TimeSpan Duration { get; set; }
        public string? Comments { get; set; }
    }
}


