using Core.Layouts;
using Core.Validators;

namespace Core.UnitTests.Validators;

public class ZonedPanelValidatorTests
{
    private const double DefaultWidth = 1000;
    private const double DefaultLength = 2000;
    private const double DefaultSocketH = 200;
    private const double DefaultSocketW = 100;
    private const double DefaultOpeningMinX = 300;
    private const double DefaultOpeningW = 400;
    private const double DefaultOpeningMinY = 500;
    private const double DefaultOpeningH = 300;

    private readonly ZonedPanelValidator _validator = new();

    private IZonedPanel CreateValid() =>
        new ZonedPanel(DefaultWidth, DefaultLength, DefaultSocketH, DefaultSocketW,
            DefaultSocketH, DefaultSocketW, DefaultOpeningMinX, DefaultOpeningW,
            DefaultOpeningMinY, DefaultOpeningH);

    [Fact]
    public void Validate_ValidParameters_IsValid()
    {
        Assert.True(_validator.Validate(CreateValid()).IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidWidth_NotValid(double width)
    {
        var panel = new ZonedPanel(width, DefaultLength, DefaultSocketH, DefaultSocketW,
            DefaultSocketH, DefaultSocketW, DefaultOpeningMinX, DefaultOpeningW,
            DefaultOpeningMinY, DefaultOpeningH);

        Assert.False(_validator.Validate(panel).IsValid);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_InvalidLength_NotValid(double length)
    {
        var panel = new ZonedPanel(DefaultWidth, length, DefaultSocketH, DefaultSocketW,
            DefaultSocketH, DefaultSocketW, DefaultOpeningMinX, DefaultOpeningW,
            DefaultOpeningMinY, DefaultOpeningH);

        Assert.False(_validator.Validate(panel).IsValid);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Validate_NegativeSocketDimensions_NotValid(double value)
    {
        var panel = new ZonedPanel(DefaultWidth, DefaultLength, value, DefaultSocketW,
            DefaultSocketH, DefaultSocketW, DefaultOpeningMinX, DefaultOpeningW,
            DefaultOpeningMinY, DefaultOpeningH);

        Assert.False(_validator.Validate(panel).IsValid);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Validate_NegativeOpeningDimensions_NotValid(double value)
    {
        var panel = new ZonedPanel(DefaultWidth, DefaultLength, DefaultSocketH, DefaultSocketW,
            DefaultSocketH, DefaultSocketW, value, DefaultOpeningW,
            DefaultOpeningMinY, DefaultOpeningH);

        Assert.False(_validator.Validate(panel).IsValid);
    }

    [Fact]
    public void Validate_ZeroSocketWidth_IsValid()
    {
        var panel = new ZonedPanel(DefaultWidth, DefaultLength, 0, 0, 0, 0,
            DefaultOpeningMinX, DefaultOpeningW, DefaultOpeningMinY, DefaultOpeningH);

        Assert.True(_validator.Validate(panel).IsValid);
    }

    [Fact]
    public void Validate_ZeroOpening_IsValid()
    {
        var panel = new ZonedPanel(DefaultWidth, DefaultLength, DefaultSocketH, DefaultSocketW,
            DefaultSocketH, DefaultSocketW, 0, 0, 0, 0);

        Assert.True(_validator.Validate(panel).IsValid);
    }

    [Fact]
    public void Validate_SocketWidthsExceedPanel_NotValid()
    {
        var panel = new ZonedPanel(DefaultWidth, DefaultLength, DefaultSocketH, 600,
            DefaultSocketH, 600, DefaultOpeningMinX, DefaultOpeningW,
            DefaultOpeningMinY, DefaultOpeningH);

        Assert.False(_validator.Validate(panel).IsValid);
    }

    [Fact]
    public void Validate_OpeningExceedsWidth_NotValid()
    {
        var panel = new ZonedPanel(DefaultWidth, DefaultLength, DefaultSocketH, DefaultSocketW,
            DefaultSocketH, DefaultSocketW, 800, 300,
            DefaultOpeningMinY, DefaultOpeningH);

        Assert.False(_validator.Validate(panel).IsValid);
    }

    [Fact]
    public void Validate_OpeningExceedsLength_NotValid()
    {
        var panel = new ZonedPanel(DefaultWidth, DefaultLength, DefaultSocketH, DefaultSocketW,
            DefaultSocketH, DefaultSocketW, DefaultOpeningMinX, DefaultOpeningW,
            1800, 300);

        Assert.False(_validator.Validate(panel).IsValid);
    }

    [Fact]
    public void Validate_OpeningBelowCutoutsWithClearance_IsValid()
    {
        var panel = new ZonedPanel(DefaultWidth, DefaultLength,
            leftSocketHeight: 200, leftSocketWidth: 100,
            rightSocketHeight: 150, rightSocketWidth: 80,
            openingMinX: 200, openingWidth: 400,
            openingMinY: 300, openingHeight: 200);

        Assert.True(_validator.Validate(panel).IsValid);
    }

    [Fact]
    public void Validate_OpeningTouchesClearanceBoundary_IsValid()
    {
        var panel = new ZonedPanel(1000, 2000,
            leftSocketHeight: 200, leftSocketWidth: 100,
            rightSocketHeight: 0, rightSocketWidth: 0,
            openingMinX: 200, openingWidth: 400,
            openingMinY: 300, openingHeight: 200);

        Assert.True(_validator.Validate(panel).IsValid);
    }

    [Fact]
    public void Validate_OpeningViolatesClearance_NotValid()
    {
        var panel = new ZonedPanel(DefaultWidth, DefaultLength,
            leftSocketHeight: 200, leftSocketWidth: 100,
            rightSocketHeight: 150, rightSocketWidth: 80,
            openingMinX: 200, openingWidth: 400,
            openingMinY: 250, openingHeight: 200);

        Assert.False(_validator.Validate(panel).IsValid);
    }

    [Fact]
    public void Validate_NoCutouts_SkipsClearanceCheck()
    {
        var panel = new ZonedPanel(DefaultWidth, DefaultLength, 0, 0, 0, 0,
            DefaultOpeningMinX, DefaultOpeningW, DefaultOpeningMinY, DefaultOpeningH);

        Assert.True(_validator.Validate(panel).IsValid);
    }

    [Fact]
    public void Validate_NoOpening_SkipsClearanceCheck()
    {
        var panel = new ZonedPanel(DefaultWidth, DefaultLength,
            leftSocketHeight: 200, leftSocketWidth: 100,
            rightSocketHeight: 0, rightSocketWidth: 0,
            0, 0, 0, 0);

        Assert.True(_validator.Validate(panel).IsValid);
    }

    [Fact]
    public void Validate_OpeningBelowSocketsWithClearance_IsValid()
    {
        var panel = new ZonedPanel(DefaultWidth, DefaultLength,
            leftSocketHeight: 200, leftSocketWidth: 100,
            rightSocketHeight: 0, rightSocketWidth: 0,
            openingMinX: 200, openingWidth: 400,
            openingMinY: 400, openingHeight: 300);

        Assert.True(_validator.Validate(panel).IsValid);
    }

    [Fact]
    public void Validate_MultipleErrors_AccumulatesAll()
    {
        var panel = new ZonedPanel(0, -1, DefaultSocketH, DefaultSocketW,
            DefaultSocketH, DefaultSocketW, DefaultOpeningMinX, DefaultOpeningW,
            DefaultOpeningMinY, DefaultOpeningH);

        var result = _validator.Validate(panel);

        Assert.False(result.IsValid);
        Assert.True(result.Errors.Count >= 2);
    }
}
