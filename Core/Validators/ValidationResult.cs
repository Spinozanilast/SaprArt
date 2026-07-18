namespace Core.Validators;

public class ValidationResult
{
    private readonly List<string> _errors = [];

    public IReadOnlyList<string> Errors => _errors.AsReadOnly();
    public bool IsValid => _errors.Count == 0;

    public ValidationResult(List<string> errors) => _errors = errors;

    public void ThrowIfInvalid()
    {
        if (!IsValid)
            throw new ValidationException(string.Join("; ", _errors));
    }
}