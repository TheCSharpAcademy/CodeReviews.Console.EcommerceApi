namespace ExerciseTracker.Validations;

public class InputValidation : IInputValidation
{
    public bool ValidateInput(string? input)
    {
        return !string.IsNullOrWhiteSpace(input);
    }

    public bool ValidateIdInput(string? input, out int id)
    {
        id = -1;
        return !string.IsNullOrWhiteSpace(input) && int.TryParse(input, out id);
    }
}