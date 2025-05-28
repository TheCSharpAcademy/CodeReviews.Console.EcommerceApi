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
        ExerciseDto GetExercise(List<ExerciseType> exTypes, ExerciseDto? existing = null);

        /// <summary>
        /// Shows user all list of records from which user needs to select
        /// </summary>
        /// <param name="exercises"></param>
        /// <param name="prompt"></param>
        /// <returns></returns>
        int GetRecordId(List<ExerciseDto> exercises);
        void PrintError(string? errorMsg);
        void PrintExercise(ExerciseDto exercise);
        void PrintExercises(List<ExerciseDto> exercises);
        void PrintPressAnyKeyToContinue();
        void PrintText(string text);
        void ClearConsole();
        int ShowMenuAndGetInput(Dictionary<int, string> menuOptions);
        int GetExerciseTypeId(List<ExerciseType> exTypes, ExerciseDto? existing = null!);
    }
}