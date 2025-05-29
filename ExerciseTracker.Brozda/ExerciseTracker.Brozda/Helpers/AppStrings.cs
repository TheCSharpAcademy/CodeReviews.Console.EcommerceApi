namespace ExerciseTracker.Brozda.Helpers
{
    internal static class AppStrings
    {
        public const string ServiceErrorOcurred = "Error occured";
        public const string ServiceErrorUpdateIdMismatch = "Error occured: argument ID and entity ID mismatch";

        public const string IoUnhandledError = "Unhandled error";
        public const string IoDateFormat = "yyyy-MM-dd HH:mm";
        public const string IoPressAnyKeyToContinue = "Press any key to continue...";
        public const string IoSelectMenu = "Please select your choice: ";
        public const string IoSelectRecordId = "Please select your choice: ";
        public const string IoSelectExerciseType = "Please select exercise type ID: ";

        public const string IoSelectDatabase = "Please select database which you'd like to manage: ";
        public const string IoDatabaseNotSelected = "Database not selected!";
        public const string IoExerciseName = "Exercise name: ";
        public const string IoVolume = "Volume";
        public const string IoDateStart = "Enter start date: ";
        public const string IoDateEnd = "Enter end date: ";
        public const string IoComment = "Enter a comment (type '-' to keep comment empty): ";

        public const string IoErrorDateFormat = "Invalid date format, format needs to be yyyy-mm-dd hh:mm 24H format";
        public const string IoErrorStartBeforeEnd = "End cannot be before start";

        public const string IoNullValueChar = "-";

        public const string ControllerViewAll = "View all excercises";
        public const string ControllerCreate = "Create a new excercise";
        public const string ControllerEdit = "Update existing excercise";
        public const string ControllerDelete = "Delete existing excercise";
        public const string SelectExerciseType = "Select exercise type";
        public const string ControllerExit = "Exit the application";

        public const string ControllerSuccessCreate = "Exercise added successfully";
        public const string ControllerSuccessEdit = "Exercise updated successfully";
        public const string ControllerSuccessDelete = "Exercise deleted successfully";
    }
}