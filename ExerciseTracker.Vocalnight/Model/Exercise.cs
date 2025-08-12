using System.ComponentModel.DataAnnotations;

namespace Exercise_Tracker.Model
{
    public class Exercise
    {
        [Key]
        public int Id { get; set; }
        public ExerciseEnum ExerciseType {  get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public TimeSpan Duration { get; set; }
        public string? Comments { get; set; }
    }
}
