namespace ExerciseTracker.Validations;

public interface IInputValidation
{
    public bool ValidateInput(string? input);

    public bool ValidateIdInput(string? input, out int id);
}