namespace Core.Validators;

public interface IValidator
{
    IValidator Must(bool condition, string message);
    IValidator MustBePositive(double value, string name);
    IValidator MustNotBeNegative(double value, string name);
    IValidator MustBeLessThan(double value, double max, string name, string maxName);
    IValidator MustBeInRange(double value, double rangeStart, double rangeEnd, string name);
    ValidationResult Check();
}
