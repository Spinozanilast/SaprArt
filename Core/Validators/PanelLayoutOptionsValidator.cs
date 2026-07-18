using Core.Layouts;

namespace Core.Validators;

public class PanelLayoutOptionsValidator : IPanelLayoutOptionsValidator
{
    public ValidationResult Validate(PanelLayout layout)
    {
        var internalWidth = layout.Length - 2 * layout.SidePanelWidth;
        var internalPanelCount = (int)(internalWidth / layout.PanelWidth);
        var remainder = internalWidth - internalPanelCount * layout.PanelWidth;

        return new DoubleValidator()
            .MustBePositive(layout.Length, "Length (L)")
            .MustBePositive(layout.Height, "Height (B)")
            .MustBePositive(layout.PanelWidth, "Panel width (a)")
            .MustBePositive(layout.SidePanelWidth, "Side panel width (b)")
            .MustNotBeNegative(layout.TargetOffset, "Offset (f)")
            .MustBeLessThan(layout.TargetOffset, layout.PanelWidth, "Offset (f)", "panel width (a)")
            .MustBeInRange(layout.SidePanelWidth, layout.PanelWidth / 2, layout.PanelWidth, "Side Panel width (b)")
            .Must(Math.Abs(remainder) <= 1e-6,
                $"Internal width ({internalWidth}) cannot be divided exactly " +
                $"into panels of width {layout.PanelWidth}. Remainder: {remainder:F4}")
            .Check();
    }
}
