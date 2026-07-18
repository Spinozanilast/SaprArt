using Core.Validators;

namespace Core.Abstractions;

public interface IValidator<in TComparable> where TComparable : IComparable<TComparable>
{
    IValidator<TComparable> Must(bool condition, string message);
    IValidator<TComparable> MustBePositive(TComparable value, string name);
    IValidator<TComparable> MustNotBeNegative(TComparable value, string name);
    IValidator<TComparable> MustBeLessThan(TComparable value, TComparable max, string name, string maxName);
    IValidator<TComparable> MustBeInRange(TComparable value, TComparable rangeStart, TComparable rangeEnd, string name);
    ValidationResult Check();
}