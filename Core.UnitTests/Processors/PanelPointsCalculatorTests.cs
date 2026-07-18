using Core.Layouts;
using Core.Processors;
using Core.Validators;

namespace Core.UnitTests.Processors;

public class PanelPointsCalculatorTests
{
    private const double DefaultLength = 12;
    private const double DefaultHeight = 5;
    private const double DefaultPanelWidth = 1;
    private const double DefaultSidePanelWidth = 1;
    private const double DefaultOffset = 0.3;

    private readonly PanelLayoutOptionsValidator _validator = new();
    private readonly PanelPointsCalculator _calculator;

    public PanelPointsCalculatorTests()
    {
        _calculator = new PanelPointsCalculator(_validator);
    }

    private PanelLayout CreateOptions(double? length = null, double? height = null,
        double? panelWidth = null, double? sidePanelWidth = null, double? targetOffset = null) =>
        new(length ?? DefaultLength, height ?? DefaultHeight, panelWidth ?? DefaultPanelWidth,
            sidePanelWidth ?? DefaultSidePanelWidth, targetOffset ?? DefaultOffset);

    [Fact]
    public void Calculate_PanelCount_IncludesSidePanels()
    {
        var result = _calculator.Calculate(CreateOptions());

        Assert.Equal(12, result.PanelCount);
    }

    [Fact]
    public void Calculate_PanelCount_SingleInternalPanel()
    {
        var result = _calculator.Calculate(CreateOptions(length: 3));

        Assert.Equal(3, result.PanelCount);
    }

    [Fact]
    public void Calculate_TargetPoints_Count_MatchesPanelCount()
    {
        var result = _calculator.Calculate(CreateOptions());

        Assert.Equal(result.PanelCount, result.TargetPoints.Count);
        Assert.Equal(result.PanelCount, result.GlobalTargetPoints.Count);
    }

    [Fact]
    public void Calculate_FirstPoint_IsLeftSidePanel()
    {
        var result = _calculator.Calculate(CreateOptions());

        Assert.Equal(DefaultOffset, result.TargetPoints[0].X, 10);
        Assert.Equal(DefaultOffset, result.TargetPoints[0].Y, 10);
        Assert.Equal(DefaultOffset, result.GlobalTargetPoints[0].X, 10);
        Assert.Equal(DefaultOffset, result.GlobalTargetPoints[0].Y, 10);
    }

    [Fact]
    public void Calculate_LastPoint_IsRightSidePanel()
    {
        var result = _calculator.Calculate(CreateOptions());

        var localLast = result.TargetPoints[^1];
        Assert.Equal(DefaultSidePanelWidth - DefaultOffset, localLast.X, 10);
        Assert.Equal(DefaultHeight - DefaultOffset, localLast.Y, 10);

        var globalLast = result.GlobalTargetPoints[^1];
        Assert.Equal(DefaultLength - DefaultOffset, globalLast.X, 10);
        Assert.Equal(DefaultHeight - DefaultOffset, globalLast.Y, 10);
    }

    [Fact]
    public void Calculate_InternalPoints_HaveLocalYEqualToOffset()
    {
        var result = _calculator.Calculate(CreateOptions());

        for (var i = 1; i < result.TargetPoints.Count - 1; i++)
            Assert.Equal(DefaultOffset, result.TargetPoints[i].Y, 10);
    }

    [Fact]
    public void Calculate_InternalPoints_HaveGlobalYEqualToOffset()
    {
        var result = _calculator.Calculate(CreateOptions());

        for (var i = 1; i < result.GlobalTargetPoints.Count - 1; i++)
            Assert.Equal(DefaultOffset, result.GlobalTargetPoints[i].Y, 10);
    }

    [Fact]
    public void Calculate_LocalPoints_AllAtLeftOffset()
    {
        var result = _calculator.Calculate(CreateOptions());

        for (var i = 0; i < result.PanelCount - 2; i++)
        {
            Assert.Equal(DefaultOffset, result.TargetPoints[i + 1].X, 10);
        }
    }

    [Fact]
    public void Calculate_GlobalPoints_AllAtLeftOffset()
    {
        var result = _calculator.Calculate(CreateOptions());

        for (var i = 0; i < result.PanelCount - 2; i++)
        {
            var panelStartX = DefaultSidePanelWidth + i * DefaultPanelWidth;
            var expectedGlobalX = panelStartX + DefaultOffset;

            Assert.Equal(expectedGlobalX, result.GlobalTargetPoints[i + 1].X, 10);
        }
    }

    [Fact]
    public void Calculate_GlobalFirstInternalPoint()
    {
        var result = _calculator.Calculate(CreateOptions());

        Assert.Equal(1.3, result.GlobalTargetPoints[1].X, 10);
    }

    [Fact]
    public void Calculate_GlobalSecondInternalPoint()
    {
        var result = _calculator.Calculate(CreateOptions());

        Assert.Equal(2.3, result.GlobalTargetPoints[2].X, 10);
    }

    [Fact]
    public void Calculate_LocalFirstInternalPoint()
    {
        var result = _calculator.Calculate(CreateOptions());

        Assert.Equal(0.3, result.TargetPoints[1].X, 10);
    }

    [Fact]
    public void Calculate_LocalSecondInternalPoint()
    {
        var result = _calculator.Calculate(CreateOptions());

        Assert.Equal(0.3, result.TargetPoints[2].X, 10);
    }

    [Fact]
    public void Calculate_ZeroOffset_AllPointsAtOrigin()
    {
        var result = _calculator.Calculate(CreateOptions(targetOffset: 0));

        Assert.Equal(0.0, result.TargetPoints[1].X, 10);
        Assert.Equal(0.0, result.TargetPoints[2].X, 10);
        Assert.Equal(1.0, result.GlobalTargetPoints[1].X, 10);
        Assert.Equal(2.0, result.GlobalTargetPoints[2].X, 10);
    }

    [Fact]
    public void Calculate_OffsetCloseToPanelWidth()
    {
        var result = _calculator.Calculate(CreateOptions(length: 12, height: 5, panelWidth: 2, sidePanelWidth: 1, targetOffset: 1.999));

        Assert.Equal(1.999, result.TargetPoints[1].X, 10);
        Assert.Equal(2.999, result.GlobalTargetPoints[1].X, 10);
    }

    [Fact]
    public void Calculate_LargeDimensions()
    {
        var result = _calculator.Calculate(CreateOptions(length: 1000, height: 500, panelWidth: 10, sidePanelWidth: 10, targetOffset: 3));

        Assert.Equal(100, result.PanelCount);

        Assert.Equal(3, result.TargetPoints[0].X, 10);
        Assert.Equal(3, result.GlobalTargetPoints[0].X, 10);

        Assert.Equal(3, result.TargetPoints[1].X, 10);
        Assert.Equal(13.0, result.GlobalTargetPoints[1].X, 10);

        Assert.Equal(3, result.TargetPoints[2].X, 10);
        Assert.Equal(23.0, result.GlobalTargetPoints[2].X, 10);

        Assert.Equal(7, result.TargetPoints[^1].X, 10);
        Assert.Equal(497.0, result.TargetPoints[^1].Y, 10);
        Assert.Equal(997.0, result.GlobalTargetPoints[^1].X, 10);
        Assert.Equal(497.0, result.GlobalTargetPoints[^1].Y, 10);
    }

    [Fact]
    public void Calculate_InvalidOptions_ThrowsValidationException()
    {
        var options = new PanelLayout(0, 5, 1, 1, 0.3);

        Assert.Throws<ValidationException>(() => _calculator.Calculate(options));
    }

    [Fact]
    public void Calculate_NegativeLength_ThrowsValidationException()
    {
        var options = new PanelLayout(-1, 5, 1, 1, 0.3);

        Assert.Throws<ValidationException>(() => _calculator.Calculate(options));
    }

    [Fact]
    public void Calculate_NegativeOffset_ThrowsValidationException()
    {
        var options = new PanelLayout(12, 5, 1, 1, -0.1);

        Assert.Throws<ValidationException>(() => _calculator.Calculate(options));
    }

    [Fact]
    public void Calculate_NonDivisibleWidth_ThrowsValidationException()
    {
        var options = new PanelLayout(10, 5, 3, 1, 0.3);

        Assert.Throws<ValidationException>(() => _calculator.Calculate(options));
    }
}
