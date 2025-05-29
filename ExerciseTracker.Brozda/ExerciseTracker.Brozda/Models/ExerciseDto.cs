namespace ExerciseTracker.Brozda.Models
{
    /// <summary>
    /// Represents record in Excercises table passed to from the service
    /// </summary>
    internal class ExerciseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int TypeId { get; set; }
        public string TypeName { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public double Volume { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public long? Duration { get; set; }
        public string? Comments { get; set; }

    }

    
}
