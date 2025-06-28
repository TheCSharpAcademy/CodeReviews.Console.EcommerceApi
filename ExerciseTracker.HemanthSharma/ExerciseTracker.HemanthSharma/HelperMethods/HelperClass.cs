using ExerciseTracker.Study.Models;
using ExerciseTracker.Study.Models.DTO;
using ExerciseTracker.Study.ValidationsMethods;

namespace ExerciseTracker.HemanthSharma.HelperMethods
{
    public class HelperClass
    {
        public static ExerciseShift ConvertToExercise(ExerciseShiftDto ExerciseDto)
        {
            if (!Validations.ValidTime(ExerciseDto.StartTime) || !Validations.ValidTime(ExerciseDto.EndTime) || !Validations.ValidDate(ExerciseDto.ExerciseDate))
            {
                return null;
            }
            DateTime ShiftStartTime = DateTime.ParseExact(ExerciseDto.StartTime, "HH:mm", null);
            DateTime ShiftEndTime = DateTime.ParseExact(ExerciseDto.EndTime, "HH:mm", null);
            DateTime ShiftDate = DateTime.ParseExact(ExerciseDto.ExerciseDate, "dd-MM-yyyy", null);
            ExerciseShift NewExercise = new ExerciseShift
            {
                ExerciseId = ExerciseDto.ExerciseId,
                StartTime = ShiftStartTime,
                EndTime = ShiftEndTime,
                Comments = ExerciseDto.Comments,
                ExerciseDate = ShiftDate
            };
            return NewExercise;
        }
    }
}
