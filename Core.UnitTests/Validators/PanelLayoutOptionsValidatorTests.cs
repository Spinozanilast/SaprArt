using Core.Layouts;
using Core.Validators;

namespace Core.UnitTests.Validators;

public class PanelLayoutOptionsValidatorTests
{
    private const double DefaultLength = 12;
    private const double DefaultHeight = 5;
    private const double DefaultPanelWidth = 1;
    private const double DefaultSidePanelWidth = 1;
    private const double DefaultOffset = 0.3;

    private readonly PanelLayoutOptionsValidator _validator = new();

    [Fact]
    public void Validate_ValidOptions_IsValid()
    {
        var options = new PanelLayoutOptions(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset);

        Assert.True(_validator.Validate(options).IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidLength_NotValid(double length)
    {
        var options = new PanelLayoutOptions(length, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset);

        Assert.False(_validator.Validate(options).IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Validate_InvalidHeight_NotValid(double height)
    {
        var options = new PanelLayoutOptions(DefaultLength, height, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset);

        Assert.False(_validator.Validate(options).IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidPanelWidth_NotValid(double panelWidth)
    {
        var options = new PanelLayoutOptions(DefaultLength, DefaultHeight, panelWidth, DefaultSidePanelWidth, DefaultOffset);

        Assert.False(_validator.Validate(options).IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidSidePanelWidth_NotValid(double sidePanelWidth)
    {
        var options = new PanelLayoutOptions(DefaultLength, DefaultHeight, DefaultPanelWidth, sidePanelWidth, DefaultOffset);

        Assert.False(_validator.Validate(options).IsValid);
    }

    [Theory]
    [InlineData(0.4)]
    [InlineData(1.5)]
    public void Validate_SidePanelWidthOutOfRange_NotValid(double sidePanelWidth)
    {
        var options = new PanelLayoutOptions(DefaultLength, DefaultHeight, DefaultPanelWidth, sidePanelWidth, DefaultOffset);

        Assert.False(_validator.Validate(options).IsValid);
    }

    [Theory]
    [InlineData(0.5)]
    [InlineData(1.0)]
    public void Validate_SidePanelWidthAtBoundaries_IsValid(double sidePanelWidth)
    {
        var options = new PanelLayoutOptions(DefaultLength, DefaultHeight, DefaultPanelWidth, sidePanelWidth, DefaultOffset);

        Assert.True(_validator.Validate(options).IsValid);
    }

    [Fact]
    public void Validate_NegativeOffset_NotValid()
    {
        var options = new PanelLayoutOptions(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, -0.1);

        Assert.False(_validator.Validate(options).IsValid);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void Validate_OffsetAtOrAbovePanelWidth_NotValid(double targetOffset)
    {
        var options = new PanelLayoutOptions(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, targetOffset);

        Assert.False(_validator.Validate(options).IsValid);
    }

    [Fact]
    public void Validate_NonDivisibleInternalWidth_NotValid()
    {
        var options = new PanelLayoutOptions(length: 10, height: 5, panelWidth: 3, sidePanelWidth: 1, targetOffset: 0.3);

        Assert.False(_validator.Validate(options).IsValid);
    }

    [Fact]
    public void Validate_MultipleInvalidParams_AccumulatesErrors()
    {
        var options = new PanelLayoutOptions(length: 0, height: -5, panelWidth: 1, sidePanelWidth: 1, targetOffset: 0.3);

        var result = _validator.Validate(options);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count >= 2);
    }
}
