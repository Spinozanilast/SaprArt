using Core.Layouts;
using Core.Validators;

namespace Core.Test.Models;

public class PanelLayoutTests
{
    private const double DefaultLength = 12;
    private const double DefaultHeight = 5;
    private const double DefaultPanelWidth = 1;
    private const double DefaultSidePanelWidth = 1;
    private const double DefaultOffset = 0.3;

    [Fact]
    public void Constructor_ValidParameters_DoesNotThrow()
    {
        var exception = Record.Exception(() =>
            new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset));

        Assert.Null(exception);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidLength_Throws(double length)
    {
        var ex = Assert.Throws<ValidationException>(() =>
            new PanelLayout(length, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset));

        Assert.Contains("Length", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Constructor_InvalidHeight_Throws(double height)
    {
        var ex = Assert.Throws<ValidationException>(() =>
            new PanelLayout(DefaultLength, height, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset));

        Assert.Contains("Height", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidPanelWidth_Throws(double panelWidth)
    {
        var ex = Assert.Throws<ValidationException>(() =>
            new PanelLayout(DefaultLength, DefaultHeight, panelWidth, DefaultSidePanelWidth, DefaultOffset));

        Assert.Contains("Panel width", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_InvalidSidePanelWidth_Throws(double sidePanelWidth)
    {
        var ex = Assert.Throws<ValidationException>(() =>
            new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, sidePanelWidth, DefaultOffset));

        Assert.Contains("Side panel width", ex.Message);
    }

    [Theory]
    [InlineData(0.4)]
    [InlineData(1.5)]
    public void Constructor_SidePanelWidthOutOfRange_Throws(double sidePanelWidth)
    {
        var ex = Assert.Throws<ValidationException>(() =>
            new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, sidePanelWidth, DefaultOffset));

        Assert.Contains("Side Panel width", ex.Message);
        Assert.Contains("range", ex.Message);
    }

    [Theory]
    [InlineData(0.5)]
    [InlineData(1.0)]
    public void Constructor_SidePanelWidthAtBoundaries_DoesNotThrow(double sidePanelWidth)
    {
        var exception = Record.Exception(() =>
            new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, sidePanelWidth, DefaultOffset));

        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_NegativeOffset_Throws()
    {
        var ex = Assert.Throws<ValidationException>(() =>
            new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, -0.1));

        Assert.Contains("Offset", ex.Message);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void Constructor_OffsetAtOrAbovePanelWidth_Throws(double targetOffset)
    {
        var ex = Assert.Throws<ValidationException>(() =>
            new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, targetOffset));

        Assert.Contains("Offset", ex.Message);
        Assert.Contains("panel width", ex.Message);
    }

    [Fact]
    public void Constructor_NonDivisibleInternalWidth_Throws()
    {
        var ex = Assert.Throws<ValidationException>(() =>
            new PanelLayout(length: 10, height: 5, panelWidth: 3, sidePanelWidth: 1, targetOffset: 0.3));

        Assert.Contains("Internal width", ex.Message);
    }

    [Fact]
    public void Constructor_MultipleInvalidParams_MessageContainsAllErrors()
    {
        var ex = Assert.Throws<ValidationException>(() =>
            new PanelLayout(length: 0, height: -5, panelWidth: 1, sidePanelWidth: 1, targetOffset: 0.3));

        Assert.Contains("Length", ex.Message);
        Assert.Contains("Height", ex.Message);
    }

    [Fact]
    public void PanelCount_IncludesSidePanels()
    {
        var layout = new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset);

        Assert.Equal(12, layout.PanelCount);
    }

    [Fact]
    public void PanelCount_SingleInternalPanel()
    {
        var layout = new PanelLayout(length: 3, height: 5, panelWidth: 1, sidePanelWidth: 1, targetOffset: 0.3);

        Assert.Equal(3, layout.PanelCount);
    }

    [Fact]
    public void TargetPoints_Count_MatchesPanelCount()
    {
        var layout = new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset);

        Assert.Equal(layout.PanelCount, layout.TargetPoints.Count);
    }

    [Fact]
    public void FirstPoint_IsLeftSidePanel()
    {
        var layout = new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset);

        Assert.Equal(DefaultOffset, layout.TargetPoints[0].X, 10);
        Assert.Equal(DefaultOffset, layout.TargetPoints[0].Y, 10);
    }

    [Fact]
    public void LastPoint_IsRightSidePanel()
    {
        var layout = new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset);

        var last = layout.TargetPoints[^1];
        Assert.Equal(DefaultLength - DefaultOffset, last.X, 10);
        Assert.Equal(DefaultHeight - DefaultOffset, last.Y, 10);
    }

    [Fact]
    public void InternalPoints_HaveYEqualToOffset()
    {
        var layout = new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset);

        for (var i = 1; i < layout.TargetPoints.Count - 1; i++)
            Assert.Equal(DefaultOffset, layout.TargetPoints[i].Y, 10);
    }

    [Fact]
    public void FirstInternalPoint_UsesTargetOffset()
    {
        var layout = new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset);

        Assert.Equal(1.3, layout.TargetPoints[1].X, 10);
    }

    [Fact]
    public void SecondInternalPoint_UsesInverseOffset()
    {
        var layout = new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset);

        Assert.Equal(2.7, layout.TargetPoints[2].X, 10);
    }

    [Fact]
    public void InternalPoints_OffsetsAlternate()
    {
        var layout = new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset);

        for (var i = 0; i < layout.PanelCount - 2; i++)
        {
            var panelStartX = DefaultSidePanelWidth + i * DefaultPanelWidth;
            var expectedOffset = i % 2 == 0 ? DefaultOffset : DefaultPanelWidth - DefaultOffset;
            var expectedX = panelStartX + expectedOffset;

            Assert.Equal(expectedX, layout.TargetPoints[i + 1].X, 10);
        }
    }

    [Fact]
    public void ZeroOffset_OddPanelsUseFullPanelWidth()
    {
        var layout = new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, targetOffset: 0);

        Assert.Equal(1.0, layout.TargetPoints[1].X, 10);
        Assert.Equal(3.0, layout.TargetPoints[2].X, 10);
    }

    [Fact]
    public void OffsetCloseToPanelWidth()
    {
        var layout = new PanelLayout(length: 12, height: 5, panelWidth: 2, sidePanelWidth: 1, targetOffset: 1.999);

        Assert.Equal(2.999, layout.TargetPoints[1].X, 10);
    }

    [Fact]
    public void ToString_ReturnsExpectedFormat()
    {
        var layout = new PanelLayout(DefaultLength, DefaultHeight, DefaultPanelWidth, DefaultSidePanelWidth, DefaultOffset);

        Assert.Equal("[PanelLayout: L=12, B=5, a=1, b=1, f=0.3, Panels=12]", layout.ToString());
    }

    [Fact]
    public void LargeDimensions_CalculatesCorrectly()
    {
        var layout = new PanelLayout(length: 1000, height: 500, panelWidth: 10, sidePanelWidth: 10, targetOffset: 3);

        Assert.Equal(100, layout.PanelCount);
        Assert.Equal(3, layout.TargetPoints[0].X, 10);
        Assert.Equal(13.0, layout.TargetPoints[1].X, 10);
        Assert.Equal(27.0, layout.TargetPoints[2].X, 10);
        Assert.Equal(997.0, layout.TargetPoints[^1].X, 10);
        Assert.Equal(497.0, layout.TargetPoints[^1].Y, 10);
    }
}
