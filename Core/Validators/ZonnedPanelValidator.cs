using Core.Layouts;

namespace Core.Validators;

public class ZonnedPanelValidator : IZonnedPanelValidator
{
    public const int MinClearance = 100;

    public ValidationResult Validate(IZonedPanel panel)
    {
        var openingExists = panel.OpeningWidth > 0 && panel.OpeningHeight > 0;
        var maxSocketHeight = Math.Max(panel.LeftSocketHeight, panel.RightSocketHeight);

        return new DoubleValidator()
            .MustBePositive(panel.Width, "Width")
            .MustBePositive(panel.Length, "Length")
            .MustNotBeNegative(panel.LeftSocketWidth, "Left socket width")
            .MustNotBeNegative(panel.LeftSocketHeight, "Left socket height")
            .MustNotBeNegative(panel.RightSocketWidth, "Right socket width")
            .MustNotBeNegative(panel.RightSocketHeight, "Right socket height")
            .MustNotBeNegative(panel.OpeningMinX, "Opening min X")
            .MustNotBeNegative(panel.OpeningMinY, "Opening min Y")
            .MustNotBeNegative(panel.OpeningWidth, "Opening width")
            .MustNotBeNegative(panel.OpeningHeight, "Opening height")
            .Must(panel.LeftSocketWidth + panel.RightSocketWidth <= panel.Width,
                $"Left and right socket widths ({panel.LeftSocketWidth} + {panel.RightSocketWidth}) exceed panel width ({panel.Width}).")
            .Must(panel.OpeningMinX + panel.OpeningWidth <= panel.Width,
                $"Opening ({panel.OpeningMinX} + {panel.OpeningWidth}) exceeds panel width ({panel.Width}).")
            .Must(panel.OpeningMinY + panel.OpeningHeight <= panel.Length,
                $"Opening ({panel.OpeningMinY} + {panel.OpeningHeight}) exceeds panel length ({panel.Length}).")
            .Must(!openingExists || maxSocketHeight <= 0 ||
                  panel.OpeningMinY + panel.OpeningHeight + MinClearance <= panel.Length - maxSocketHeight,
                $"Opening must be at least {MinClearance}mm below the top cutouts.")
            .Check();
    }
}
