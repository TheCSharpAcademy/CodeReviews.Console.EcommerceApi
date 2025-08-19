using System.ComponentModel.DataAnnotations;

namespace ExerciseTracker.Study.Models.DTO
{
    public class ExerciseShiftDto
    {
        public int ExerciseId { get; set; }
        [RegularExpression(@"^(0?[0-9]|1[0-9]|2[0-3]):[0-5]\d$")]
        public string StartTime { get; set; }
        [RegularExpression(@"^(0?[0-9]|1[0-9]|2[0-3]):[0-5]\d$")]
        public string EndTime { get; set; }
        [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])-(0[1-9]|1[0-2])-(19|20)\d{2}$")]
        public string ExerciseDate { get; set; }
        public string? Comments { get; set; }
    }
}
