using Core.Layouts;

namespace Core.Validators;

public class OpeningPanelValidator : IOpeningPanelValidator
{
    public ValidationResult Validate(IOpeningPanel panel, double step)
    {
        return new DoubleValidator()
            .MustBePositive(panel.Width, "Width")
            .MustBePositive(panel.Length, "Length")
            .MustNotBeNegative(panel.OpeningMinX, "OpeningMinX")
            .MustNotBeNegative(panel.OpeningMinY, "OpeningMinY")
            .MustNotBeNegative(panel.OpeningWidth, "OpeningWidth")
            .MustNotBeNegative(panel.OpeningHeight, "OpeningHeight")
            .Must(panel.OpeningMinX + panel.OpeningWidth <= panel.Width,
                $"Opening ({panel.OpeningMinX} + {panel.OpeningWidth}) exceeds panel width ({panel.Width}).")
            .Must(panel.OpeningMinY + panel.OpeningHeight <= panel.Length,
                $"Opening ({panel.OpeningMinY} + {panel.OpeningHeight}) exceeds panel length ({panel.Length}).")
            .MustBePositive(step, "Step")
            .Check();
    }
}
