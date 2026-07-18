using Core.Validators;

namespace Core.Test;

public class DoubleValidatorTests
{
    [Fact]
    public void Must_Passes_WhenConditionIsTrue()
    {
        var result = new DoubleValidator()
            .Must(true, "should not appear")
            .Check();

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Must_Fails_WhenConditionIsFalse()
    {
        var result = new DoubleValidator()
            .Must(false, "something went wrong")
            .Check();

        Assert.False(result.IsValid);
        Assert.Contains("something went wrong", result.Errors);
    }

    [Fact]
    public void MustBePositive_Fails_WhenValueIsZero()
    {
        var result = new DoubleValidator()
            .MustBePositive(0, "Length")
            .Check();

        Assert.False(result.IsValid);
    }

    [Fact]
    public void MustBePositive_Fails_WhenValueIsNegative()
    {
        var result = new DoubleValidator()
            .MustBePositive(-5, "Length")
            .Check();

        Assert.False(result.IsValid);
    }

    [Fact]
    public void MustBePositive_Passes_WhenValueIsPositive()
    {
        var result = new DoubleValidator()
            .MustBePositive(10, "Length")
            .Check();

        Assert.True(result.IsValid);
    }

    [Fact]
    public void MustNotBeNegative_Fails_WhenValueIsNegative()
    {
        var result = new DoubleValidator()
            .MustNotBeNegative(-1, "Offset")
            .Check();

        Assert.False(result.IsValid);
    }

    [Fact]
    public void MustNotBeNegative_Passes_WhenValueIsZero()
    {
        var result = new DoubleValidator()
            .MustNotBeNegative(0, "Offset")
            .Check();

        Assert.True(result.IsValid);
    }

    [Fact]
    public void MustBeLessThan_Fails_WhenValueEqualsMax()
    {
        var result = new DoubleValidator()
            .MustBeLessThan(5, 5, "value", "max")
            .Check();

        Assert.False(result.IsValid);
    }

    [Fact]
    public void MustBeLessThan_Fails_WhenValueExceedsMax()
    {
        var result = new DoubleValidator()
            .MustBeLessThan(10, 5, "value", "max")
            .Check();

        Assert.False(result.IsValid);
    }

    [Fact]
    public void MustBeLessThan_Passes_WhenValueIsLess()
    {
        var result = new DoubleValidator()
            .MustBeLessThan(3, 5, "value", "max")
            .Check();

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Check_AccumulatesMultipleErrors()
    {
        var result = new DoubleValidator()
            .MustBePositive(0, "Length")
            .MustBePositive(-1, "Height")
            .MustNotBeNegative(-5, "Offset")
            .Check();

        Assert.Equal(3, result.Errors.Count);
    }

    [Fact]
    public void ThrowIfInvalid_Throws_WhenInvalid()
    {
        var result = new DoubleValidator()
            .MustBePositive(0, "Length")
            .Check();

        Assert.Throws<ValidationException>(() => result.ThrowIfInvalid());
    }

    [Fact]
    public void ThrowIfInvalid_DoesNotThrow_WhenValid()
    {
        var result = new DoubleValidator()
            .MustBePositive(10, "Length")
            .Check();

        result.ThrowIfInvalid();
    }

    [Fact]
    public void MustBeInRange_Passes_WhenValueInRange()
    {
        var result = new DoubleValidator()
            .MustBeInRange(5, 1, 10, "value")
            .Check();

        Assert.True(result.IsValid);
    }

    [Fact]
    public void MustBeInRange_Passes_WhenValueAtLowerBound()
    {
        var result = new DoubleValidator()
            .MustBeInRange(1, 1, 10, "value")
            .Check();

        Assert.True(result.IsValid);
    }

    [Fact]
    public void MustBeInRange_Passes_WhenValueAtUpperBound()
    {
        var result = new DoubleValidator()
            .MustBeInRange(10, 1, 10, "value")
            .Check();

        Assert.True(result.IsValid);
    }

    [Fact]
    public void MustBeInRange_Fails_WhenValueBelowRange()
    {
        var result = new DoubleValidator()
            .MustBeInRange(0, 1, 10, "value")
            .Check();

        Assert.False(result.IsValid);
    }

    [Fact]
    public void MustBeInRange_Fails_WhenValueAboveRange()
    {
        var result = new DoubleValidator()
            .MustBeInRange(11, 1, 10, "value")
            .Check();

        Assert.False(result.IsValid);
    }
}
