using Core.Layouts;
using Core.Validators;

namespace Core.Processors;

public class HorizontalLinesStepCalculator(IOpeningPanelValidator validator) : ILinesStepCalculator
{
    public HashSet<Line2D> Calculate(IOpeningPanel openingPanel, double step)
    {
        validator.Validate(openingPanel, step).ThrowIfInvalid();

        if (step > openingPanel.Length)
            return [];

        var linesSet = new HashSet<Line2D>();
        var hasOpening = openingPanel.OpeningWidth > 0 && openingPanel.OpeningHeight > 0;
        var openingMinY = openingPanel.OpeningMinY;
        var openingMaxY = openingMinY + openingPanel.OpeningHeight;
        var openingMinX = openingPanel.OpeningMinX;
        var openingMaxX = openingMinX + openingPanel.OpeningWidth;

        for (var y = step; y <= openingPanel.Length; y += step)
        {
            if (hasOpening && y > openingMinY && y < openingMaxY)
            {
                if (openingMinX > 0)
                    linesSet.Add(new Line2D(new Point2D(0, y), new Point2D(openingMinX, y)));
                if (openingMaxX < openingPanel.Width)
                    linesSet.Add(new Line2D(new Point2D(openingMaxX, y), new Point2D(openingPanel.Width, y)));
            }
            else
            {
                linesSet.Add(new Line2D(new Point2D(0, y), new Point2D(openingPanel.Width, y)));
            }
        }

        return linesSet;
    }
}
