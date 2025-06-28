using ExerciseTracker.UI.Models;
using System.Globalization;

namespace ExerciseTracker.UI;

internal static class HelperMethods
{
    public static ExerciseShiftDto RefineExerciseShift(ExerciseShiftDto Shift)
    {
        Shift.StartTime = (DateTime.ParseExact(Shift.StartTime, "HH:mm", null, DateTimeStyles.None)).ToString("HH:mm");
        Shift.EndTime = (DateTime.ParseExact(Shift.EndTime, "HH:mm", null, DateTimeStyles.None)).ToString("HH:mm");
        Shift.ExerciseDate = (DateTime.Parse(Shift.ExerciseDate)).ToString("dd-MM-yyyy");
        return Shift;
    }
}
