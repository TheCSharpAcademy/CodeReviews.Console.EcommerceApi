

namespace ExerciseTracker.Brozda.Models
{
    /// <summary>
    /// Represents record in Excercises table
    /// </summary>
    internal class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public double WeightLifted { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public long? Duration { get; set; }
        public string? Comments { get; set; }

        /// <summary>
        /// Maps value from passed value to current one
        /// </summary>
        /// <param name="updated"></param>
        public void MapFromUpdate(Exercise updated)
        {
            this.Name = updated.Name;
            this.WeightLifted = updated.WeightLifted;
            this.DateStart = updated.DateStart;
            this.DateEnd = updated.DateEnd;
            this.Duration = updated.Duration;
            this.Comments = updated.Comments;
        }
    }
    
}


