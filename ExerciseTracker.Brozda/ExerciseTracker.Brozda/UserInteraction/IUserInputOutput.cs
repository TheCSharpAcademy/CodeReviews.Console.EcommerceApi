using ExerciseTracker.Brozda.Models;

namespace ExerciseTracker.Brozda.UserInteraction
{
    /// <summary>
    /// Defines contract to show data and to retrieve input from the user via Console
    /// </summary>
    internal interface IUserInputOutput
    {
        /// <summary>
        /// Retrieves an exercise values from user input
        /// </summary>
        /// <param name="existing">Optional <see cref="Exercise"/> value which values will be shown to user as default values</param>
        /// <returns></returns>
        ExerciseDto GetExercise(ExerciseType exType, ExerciseDto? existing = null);

        /// <summary>
        /// Shows user all list of records from which user needs to select
        /// </summary>
        /// <param name="exercises"></param>
        /// <param name="prompt"></param>
        /// <returns></returns>
        int GetRecordId(List<ExerciseDto> exercises);
        /// <summary>
        /// Prints error message to the output
        /// </summary>
        /// <param name="errorMsg">Nullable string containing error message, default value will be used in case of null value</param>
        void PrintError(string? errorMsg);
        /// <summary>
        /// Prints <see cref="ExerciseDto"/> to the output
        /// </summary>
        /// <param name="exercise"></param>
        void PrintExercise(ExerciseDto exercise);
        /// <summary>
        /// Prints a list of <see cref="ExerciseDto"/> to the output
        /// </summary>
        /// <param name="exercises">A List contaning <see cref="ExerciseDto"/> objects</param>
        void PrintExercises(List<ExerciseDto> exercises);
        /// <summary>
        /// Prints "Press any key to continue" text and awaits user input before returning the app flow
        /// </summary>
        void PrintPressAnyKeyToContinue();
        /// <summary>
        /// Prints provided text to the output
        /// </summary>
        /// <param name="text">A text to be printed</param>
        void PrintText(string text);
        /// <summary>
        /// Clears existing text in the console
        /// </summary>
        void ClearConsole();
        /// <summary>
        /// Prints out the menu and retrieves valid input from the user
        /// </summary>
        /// <param name="menuOptions">A <see cref="Dictionary{TKey, TValue}"/>
        /// Key is numeric representation of menu choice
        /// Value is string representation of menu choice</param>
        /// <returns>A numeric representation of menu choice</returns>
        int ShowMenuAndGetInput(Dictionary<int, string> menuOptions);
        /// <summary>
        /// Prints out existing exercise types and retrieves numeric representation of the <see cref="ExerciseType"/> from the user
        /// </summary>
        /// <param name="exTypes">A list of <see cref="ExerciseType"/> objects</param>
        /// <returns>A numeric representation of the <see cref="ExerciseType"/></returns>
        int GetExerciseTypeId(List<ExerciseType> exTypes);
    }
}