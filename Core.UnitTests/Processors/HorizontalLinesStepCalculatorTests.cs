using Core.Layouts;
using Core.Processors;
using Core.Validators;

namespace Core.UnitTests.Processors;

public class HorizontalLinesStepCalculatorTests
{
    private const double DefaultWidth = 10;
    private const double DefaultLength = 10;
    private const double DefaultOpeningMinX = 3;
    private const double DefaultOpeningW = 4;
    private const double DefaultOpeningMinY = 3;
    private const double DefaultOpeningH = 4;

    private readonly HorizontalLinesStepCalculator _calculator;

    public HorizontalLinesStepCalculatorTests()
    {
        _calculator = new HorizontalLinesStepCalculator(new OpeningPanelValidator());
    }

    private IOpeningPanel CreatePanel(double? width = null, double? length = null,
        double? openingMinX = null, double? openingWidth = null,
        double? openingMinY = null, double? openingHeight = null) =>
        new OpeningPanel(
            width ?? DefaultWidth, length ?? DefaultLength,
            openingMinX ?? DefaultOpeningMinX, openingWidth ?? DefaultOpeningW,
            openingMinY ?? DefaultOpeningMinY, openingHeight ?? DefaultOpeningH);

    [Fact]
    public void Calculate_NoOpening_ReturnsLinesAtEveryStep()
    {
        var panel = CreatePanel(openingWidth: 0, openingHeight: 0);
        var result = _calculator.Calculate(panel, step: 2);

        Assert.Equal(5, result.Count);
        Assert.Contains(new Line2D(new Point2D(0, 2), new Point2D(10, 2)), result);
        Assert.Contains(new Line2D(new Point2D(0, 4), new Point2D(10, 4)), result);
        Assert.Contains(new Line2D(new Point2D(0, 6), new Point2D(10, 6)), result);
        Assert.Contains(new Line2D(new Point2D(0, 8), new Point2D(10, 8)), result);
        Assert.Contains(new Line2D(new Point2D(0, 10), new Point2D(10, 10)), result);
    }

    [Fact]
    public void Calculate_WithOpening_SplitsLinesInsideOpening()
    {
        var panel = CreatePanel(width: 10, length: 10,
            openingMinX: 2, openingWidth: 4,
            openingMinY: 2, openingHeight: 4);
        var result = _calculator.Calculate(panel, step: 4);

        Assert.Equal(3, result.Count);
        Assert.Contains(new Line2D(new Point2D(0, 4), new Point2D(2, 4)), result);
        Assert.Contains(new Line2D(new Point2D(6, 4), new Point2D(10, 4)), result);
        Assert.Contains(new Line2D(new Point2D(0, 8), new Point2D(10, 8)), result);
    }

    [Fact]
    public void Calculate_StepExceedsLength_ReturnsEmpty()
    {
        var panel = CreatePanel(length: 10);
        var result = _calculator.Calculate(panel, step: 15);

        Assert.Empty(result);
    }

    [Fact]
    public void Calculate_StepEqualsLength_ReturnsSingleLine()
    {
        var panel = CreatePanel(length: 10, openingWidth: 0, openingHeight: 0);
        var result = _calculator.Calculate(panel, step: 10);

        Assert.Single(result);
        Assert.Contains(new Line2D(new Point2D(0, 10), new Point2D(10, 10)), result);
    }

    [Fact]
    public void Calculate_LineAtOpeningBoundary_IsFullWidth()
    {
        var panel = CreatePanel(width: 10, length: 10,
            openingMinX: 3, openingWidth: 4,
            openingMinY: 3, openingHeight: 4);
        var result = _calculator.Calculate(panel, step: 3);

        Assert.Equal(4, result.Count);
        Assert.Contains(new Line2D(new Point2D(0, 3), new Point2D(10, 3)), result);
        Assert.Contains(new Line2D(new Point2D(0, 6), new Point2D(3, 6)), result);
        Assert.Contains(new Line2D(new Point2D(7, 6), new Point2D(10, 6)), result);
        Assert.Contains(new Line2D(new Point2D(0, 9), new Point2D(10, 9)), result);
    }

    [Fact]
    public void Calculate_OpeningAtLeftEdge_OnlyRightSegmentAdded()
    {
        var panel = CreatePanel(width: 10, length: 10,
            openingMinX: 0, openingWidth: 4,
            openingMinY: 2, openingHeight: 4);
        var result = _calculator.Calculate(panel, step: 4);

        Assert.Equal(2, result.Count);
        Assert.Contains(new Line2D(new Point2D(4, 4), new Point2D(10, 4)), result);
        Assert.Contains(new Line2D(new Point2D(0, 8), new Point2D(10, 8)), result);
    }

    [Fact]
    public void Calculate_OpeningAtRightEdge_OnlyLeftSegmentAdded()
    {
        var panel = CreatePanel(width: 10, length: 10,
            openingMinX: 6, openingWidth: 4,
            openingMinY: 2, openingHeight: 4);
        var result = _calculator.Calculate(panel, step: 4);

        Assert.Equal(2, result.Count);
        Assert.Contains(new Line2D(new Point2D(0, 4), new Point2D(6, 4)), result);
        Assert.Contains(new Line2D(new Point2D(0, 8), new Point2D(10, 8)), result);
    }

    [Fact]
    public void Calculate_NegativeStep_ThrowsValidationException()
    {
        var panel = CreatePanel();

        Assert.Throws<ValidationException>(() => _calculator.Calculate(panel, step: -1));
    }

    [Fact]
    public void Calculate_ZeroStep_ThrowsValidationException()
    {
        var panel = CreatePanel();

        Assert.Throws<ValidationException>(() => _calculator.Calculate(panel, step: 0));
    }

    [Fact]
    public void Calculate_InvalidPanel_ThrowsValidationException()
    {
        var panel = CreatePanel(width: 0);

        Assert.Throws<ValidationException>(() => _calculator.Calculate(panel, step: 1));
    }

    [Fact]
    public void Calculate_OpeningExceedsWidth_ThrowsValidationException()
    {
        var panel = CreatePanel(width: 10, openingMinX: 8, openingWidth: 5);

        Assert.Throws<ValidationException>(() => _calculator.Calculate(panel, step: 1));
    }

    [Fact]
    public void Calculate_OpeningExceedsLength_ThrowsValidationException()
    {
        var panel = CreatePanel(length: 10, openingMinY: 8, openingHeight: 5);

        Assert.Throws<ValidationException>(() => _calculator.Calculate(panel, step: 1));
    }

    [Fact]
    public void Calculate_MultipleStepsInsideOpening_AllLinesSplit()
    {
        var panel = CreatePanel(width: 10, length: 10,
            openingMinX: 2, openingWidth: 6,
            openingMinY: 2, openingHeight: 6);
        var result = _calculator.Calculate(panel, step: 2);

        Assert.Equal(7, result.Count);
        Assert.Contains(new Line2D(new Point2D(0, 2), new Point2D(10, 2)), result);
        Assert.Contains(new Line2D(new Point2D(0, 4), new Point2D(2, 4)), result);
        Assert.Contains(new Line2D(new Point2D(8, 4), new Point2D(10, 4)), result);
        Assert.Contains(new Line2D(new Point2D(0, 6), new Point2D(2, 6)), result);
        Assert.Contains(new Line2D(new Point2D(8, 6), new Point2D(10, 6)), result);
        Assert.Contains(new Line2D(new Point2D(0, 8), new Point2D(10, 8)), result);
        Assert.Contains(new Line2D(new Point2D(0, 10), new Point2D(10, 10)), result);
    }
}
