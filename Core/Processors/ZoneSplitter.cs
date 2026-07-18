using System.Collections.ObjectModel;
using Core.Layouts;
using Core.Validators;

namespace Core.Processors;

public class ZoneSplitter : IZoneSplitter
{
    private readonly IZonedPanelValidator _validator;

    public ZoneSplitter(IZonedPanelValidator validator)
    {
        _validator = validator;
    }

    public IReadOnlyList<VerticalZone> Split(IZonedPanel panel)
    {
        _validator.Validate(panel).ThrowIfInvalid();

        var zones = new List<VerticalZone>();
        var W = panel.Width;
        var H = panel.Length;

        var xBoundaries = new SortedSet<double> { 0, W };

        if (panel.LeftSocketWidth > 0) xBoundaries.Add(panel.LeftSocketWidth);
        if (panel.RightSocketWidth > 0) xBoundaries.Add(W - panel.RightSocketWidth);

        if (panel.OpeningWidth > 0)
        {
            xBoundaries.Add(panel.OpeningMinX);
            xBoundaries.Add(panel.OpeningMinX + panel.OpeningWidth);
        }

        var boundariesList = xBoundaries.ToList();
        for (var i = 0; i < boundariesList.Count - 1; i++)
        {
            var xStart = boundariesList[i];
            var xEnd = boundariesList[i + 1];
            var width = xEnd - xStart;

            if (Math.Abs(width) < 1e-6) continue;

            var isInsideOpening = panel.OpeningWidth > 0 &&
                                  xStart >= panel.OpeningMinX &&
                                  xEnd <= panel.OpeningMinX + panel.OpeningWidth;

            var isInsideLeftSocket = panel.LeftSocketWidth > 0 &&
                                     xStart < panel.LeftSocketWidth;

            var isInsideRightSocket = panel.RightSocketWidth > 0 &&
                                      xStart >= W - panel.RightSocketWidth;

            if (isInsideOpening)
            {
                AddOpeningSegments(panel.OpeningMinY, panel.OpeningHeight, zones, xStart, width, H);
            }
            else if (isInsideLeftSocket)
            {
                AddSocketSegment(zones, "LeftSocket", xStart, width, H, panel.LeftSocketHeight);
            }
            else if (isInsideRightSocket)
            {
                AddSocketSegment(zones, "RightSocket", xStart, width, H, panel.RightSocketHeight);
            }
            else
            {
                zones.Add(new VerticalZone("PanelSolid", xStart, width, H));
            }
        }

        return new ReadOnlyCollection<VerticalZone>(zones);
    }

    private void AddOpeningSegments(double openingMinY, double openingHeight, List<VerticalZone> zones, double x,
        double width, double H)
    {
        var oTop = openingMinY + openingHeight;

        if (openingMinY > 0)
            zones.Add(new VerticalZone("OpeningTop", x, width, openingMinY));

        if (openingHeight > 0)
            zones.Add(new VerticalZone("OpeningCutout", x, width, openingHeight));

        if (H - oTop > 0)
            zones.Add(new VerticalZone("OpeningBottom", x, width, H - oTop));
    }

    private void AddSocketSegment(List<VerticalZone> zones, string name, double x, double width, double H,
        double socketH)
    {
        if (socketH > 0)
            zones.Add(new VerticalZone($"{name}Top", x, width, socketH));

        if (H - socketH > 0)
            zones.Add(new VerticalZone($"{name}Bottom", x, width, H - socketH));
    }
}