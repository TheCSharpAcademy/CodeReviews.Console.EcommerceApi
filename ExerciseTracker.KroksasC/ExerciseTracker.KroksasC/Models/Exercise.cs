namespace ExerciseTracker.KroksasC.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;
        public string? Comment { get; set; }
    }
}
