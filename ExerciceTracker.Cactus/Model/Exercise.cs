using Microsoft.Identity.Client;

namespace ExerciseTracker.Cactus.Model
{
    public class Exercise
    {
        public int Id { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int Duration { get; set; }
        public string Comments { get; set; }

        public Exercise()
        { }

        public Exercise(DateTime dateStart, DateTime dateEnd, int duration, string comments)
        {
            DateStart = dateStart;
            DateEnd = dateEnd;
            Duration = duration;
            Comments = comments;
        }
    }
}
