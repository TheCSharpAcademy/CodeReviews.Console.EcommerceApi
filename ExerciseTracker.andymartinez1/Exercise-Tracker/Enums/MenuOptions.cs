using System.ComponentModel.DataAnnotations;

namespace Exercise_Tracker.Enums;

public enum MenuOptions
{
    [Display(Name = "View All Exercises")]
    ViewAllExercises,

    [Display(Name = "View Exercise By ID")]
    ViewExerciseById,

    [Display(Name = "Add New Exercise")]
    AddNewExercise,

    [Display(Name = "Update Existing Exercise")]
    UpdateExercise,

    [Display(Name = "Delete Exercise")]
    DeleteExercise,

    [Display(Name = "Exit")]
    Exit,
}
