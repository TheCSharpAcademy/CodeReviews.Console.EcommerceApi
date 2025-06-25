namespace ExerciseTracker.Models;

public class Exercise
{
   public int Id { get; set; }
   public required string  Description { get; set; }
   public DateTime DateStart { get; set; }
   public DateTime DateEnd { get; set; }
}