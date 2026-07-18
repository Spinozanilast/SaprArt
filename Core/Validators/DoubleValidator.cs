namespace Core.Validators;

public class DoubleValidator : IValidator
{
    private readonly List<string> _errors = [];

    public IValidator Must(bool condition, string message)
    {
        if (!condition) _errors.Add(message);
        return this;
    }

    public IValidator MustBePositive(double value, string name) =>
        Must(value > 0, $"{name} must be positive.");

    public IValidator MustNotBeNegative(double value, string name) =>
        Must(value >= 0, $"{name} cannot be negative.");

    public IValidator MustBeLessThan(double value, double max, string name, string maxName) =>
        Must(value < max, $"{name} ({value}) must be less than {maxName} ({max}).");

    public IValidator MustBeInRange(double value, double rangeStart, double rangeEnd, string name) =>
        Must(value >= rangeStart && value <= rangeEnd, $"{name} must be in range [{rangeStart}, {rangeEnd}]");

    public ValidationResult Check() => new(_errors);
}
