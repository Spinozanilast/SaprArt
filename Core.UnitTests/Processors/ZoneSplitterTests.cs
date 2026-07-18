using Core.Layouts;
using Core.Processors;
using Core.Validators;

namespace Core.Test.Processors;

public class ZoneSplitterTests
{
    private const double DefaultWidth = 1000;
    private const double DefaultLength = 2000;
    private const double DefaultOpeningMinX = 200;
    private const double DefaultOpeningW = 400;
    private const double DefaultOpeningMinY = 500;
    private const double DefaultOpeningH = 300;

    private readonly ZonedPanelValidator _validator = new();
    private readonly ZoneSplitter _splitter;

    public ZoneSplitterTests()
    {
        _splitter = new ZoneSplitter(_validator);
    }

    private IZonedPanel CreatePanel(
        double leftSocketH = 200, double leftSocketW = 100,
        double rightSocketH = 150, double rightSocketW = 80,
        double openingMinY = 500, double openingH = 300) =>
        new ZonedPanel(DefaultWidth, DefaultLength, leftSocketH, leftSocketW,
            rightSocketH, rightSocketW, DefaultOpeningMinX, DefaultOpeningW,
            openingMinY, openingH);

    [Fact]
    public void Split_AllFeatures_CreatesHorizontalColumns()
    {
        var zones = _splitter.Split(CreatePanel());

        Assert.Equal(9, zones.Count);

        Assert.Equal("LeftSocketTop", zones[0].Name);
        Assert.Equal("LeftSocketBottom", zones[1].Name);
        Assert.Equal("PanelSolid", zones[2].Name);
        Assert.Equal("OpeningTop", zones[3].Name);
        Assert.Equal("OpeningCutout", zones[4].Name);
        Assert.Equal("OpeningBottom", zones[5].Name);
        Assert.Equal("PanelSolid", zones[6].Name);
        Assert.Equal("RightSocketTop", zones[7].Name);
        Assert.Equal("RightSocketBottom", zones[8].Name);
    }

    [Fact]
    public void Split_AllFeatures_HasCorrectPositionsAndDimensions()
    {
        var zones = _splitter.Split(CreatePanel());

        Assert.Equal(0, zones[0].X);
        Assert.Equal(100, zones[0].Width);
        Assert.Equal(200, zones[0].Height);

        Assert.Equal(0, zones[1].X);
        Assert.Equal(100, zones[1].Width);
        Assert.Equal(1800, zones[1].Height);

        Assert.Equal(100, zones[2].X);
        Assert.Equal(100, zones[2].Width);
        Assert.Equal(2000, zones[2].Height);

        Assert.Equal(200, zones[3].X);
        Assert.Equal(400, zones[3].Width);
        Assert.Equal(500, zones[3].Height);

        Assert.Equal(200, zones[4].X);
        Assert.Equal(400, zones[4].Width);
        Assert.Equal(300, zones[4].Height);

        Assert.Equal(200, zones[5].X);
        Assert.Equal(400, zones[5].Width);
        Assert.Equal(1200, zones[5].Height);

        Assert.Equal(600, zones[6].X);
        Assert.Equal(320, zones[6].Width);
        Assert.Equal(2000, zones[6].Height);

        Assert.Equal(920, zones[7].X);
        Assert.Equal(80, zones[7].Width);
        Assert.Equal(150, zones[7].Height);

        Assert.Equal(920, zones[8].X);
        Assert.Equal(80, zones[8].Width);
        Assert.Equal(1850, zones[8].Height);
    }

    [Fact]
    public void Split_NoSockets_OnlyOpeningZones()
    {
        var zones = _splitter.Split(CreatePanel(0, 0, 0, 0));

        Assert.Equal(5, zones.Count);

        Assert.Equal("PanelSolid", zones[0].Name);
        Assert.Equal(0, zones[0].X);
        Assert.Equal(200, zones[0].Width);

        Assert.Equal("OpeningTop", zones[1].Name);
        Assert.Equal("OpeningCutout", zones[2].Name);
        Assert.Equal("OpeningBottom", zones[3].Name);

        Assert.Equal("PanelSolid", zones[4].Name);
        Assert.Equal(600, zones[4].X);
        Assert.Equal(400, zones[4].Width);
    }

    [Fact]
    public void Split_OnlyLeftSocket_ProducesLeftSocketZones()
    {
        var zones = _splitter.Split(CreatePanel(200, 100, 0, 0));

        Assert.Equal(7, zones.Count);
        Assert.Equal("LeftSocketTop", zones[0].Name);
        Assert.Equal("LeftSocketBottom", zones[1].Name);
        Assert.Equal("PanelSolid", zones[2].Name);
        Assert.Equal("OpeningTop", zones[3].Name);
        Assert.Equal("OpeningCutout", zones[4].Name);
        Assert.Equal("OpeningBottom", zones[5].Name);
        Assert.Equal("PanelSolid", zones[6].Name);
    }

    [Fact]
    public void Split_OnlyRightSocket_ProducesRightSocketZones()
    {
        var zones = _splitter.Split(CreatePanel(0, 0, 100, 80));

        Assert.Equal(7, zones.Count);
        Assert.Equal("PanelSolid", zones[0].Name);
        Assert.Equal("OpeningTop", zones[1].Name);
        Assert.Equal("OpeningCutout", zones[2].Name);
        Assert.Equal("OpeningBottom", zones[3].Name);
        Assert.Equal("PanelSolid", zones[4].Name);
        Assert.Equal("RightSocketTop", zones[5].Name);
        Assert.Equal("RightSocketBottom", zones[6].Name);
    }

    [Fact]
    public void Split_OpeningAtBottom_OmitsOpeningTop()
    {
        var zones = _splitter.Split(CreatePanel(openingMinY: 0));

        Assert.Equal(8, zones.Count);
        Assert.Equal("OpeningCutout", zones[3].Name);
        Assert.Equal("OpeningBottom", zones[4].Name);
    }

    [Fact]
    public void Split_OpeningFullHeight_OmitsOpeningTopAndBottom()
    {
        var zones = _splitter.Split(CreatePanel(0, 0, 0, 0, openingMinY: 0, openingH: 2000));

        Assert.Equal(3, zones.Count);
        Assert.Equal("OpeningCutout", zones[1].Name);
        Assert.Equal(2000, zones[1].Height);
        Assert.Equal(200, zones[1].X);
        Assert.Equal(400, zones[1].Width);
    }

    [Fact]
    public void Split_NoFeatures_ReturnsSingleSolidZone()
    {
        var panel = new ZonedPanel(1000, 2000, 0, 0, 0, 0, 0, 0, 0, 0);
        var zones = _splitter.Split(panel);

        Assert.Single(zones);
        Assert.Equal("PanelSolid", zones[0].Name);
        Assert.Equal(0, zones[0].X);
        Assert.Equal(1000, zones[0].Width);
        Assert.Equal(2000, zones[0].Height);
    }

    [Fact]
    public void Split_OpeningHeight_MatchesCutoutZone()
    {
        var zones = _splitter.Split(CreatePanel());
        Assert.Equal(300, zones[4].Height);
        Assert.Equal("OpeningCutout", zones[4].Name);
    }

    [Fact]
    public void Split_InvalidPanel_ThrowsValidationException()
    {
        var panel = new ZonedPanel(0, 2000, 0, 0, 0, 0, 0, 0, 0, 0);

        Assert.Throws<ValidationException>(() => _splitter.Split(panel));
    }

    [Fact]
    public void Split_NegativeDimensions_ThrowsValidationException()
    {
        var panel = new ZonedPanel(1000, -1, 0, 0, 0, 0, 0, 0, 0, 0);

        Assert.Throws<ValidationException>(() => _splitter.Split(panel));
    }
}
