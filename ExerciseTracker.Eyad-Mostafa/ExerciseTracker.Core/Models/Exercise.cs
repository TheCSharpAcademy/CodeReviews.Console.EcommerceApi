using System.ComponentModel.DataAnnotations;

namespace ExerciseTracker.Core.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        [MaxLength(400, ErrorMessage = "Comments cannot exceed 400 characters.")]
        public string? Comments { get; set; }

        public TimeSpan Duration => EndDate - StartDate;
    }
}
