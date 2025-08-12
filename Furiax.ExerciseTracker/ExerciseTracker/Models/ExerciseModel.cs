using System.ComponentModel.DataAnnotations;

namespace ExerciseTracker.Models
{
	public class ExerciseModel
	{
		[Key]
		public int ExerciseId { get; set; }
		public string ExerciseType { get; set; }
		public DateTime DateStart { get; set; }
		public DateTime DateEnd { get; set; }
		public TimeSpan Duration => DateEnd - DateStart;
		public string? Comments { get; set; }
	}
}
