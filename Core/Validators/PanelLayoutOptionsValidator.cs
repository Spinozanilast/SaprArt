using Core.Layouts;

namespace Core.Validators;

public class PanelLayoutOptionsValidator : IPanelLayoutOptionsValidator
{
    public ValidationResult Validate(PanelLayoutOptions options)
    {
        var internalWidth = options.Length - 2 * options.SidePanelWidth;
        var internalPanelCount = (int)(internalWidth / options.PanelWidth);
        var remainder = internalWidth - internalPanelCount * options.PanelWidth;

        return new DoubleValidator()
            .MustBePositive(options.Length, "Length (L)")
            .MustBePositive(options.Height, "Height (B)")
            .MustBePositive(options.PanelWidth, "Panel width (a)")
            .MustBePositive(options.SidePanelWidth, "Side panel width (b)")
            .MustNotBeNegative(options.TargetOffset, "Offset (f)")
            .MustBeLessThan(options.TargetOffset, options.PanelWidth, "Offset (f)", "panel width (a)")
            .MustBeInRange(options.SidePanelWidth, options.PanelWidth / 2, options.PanelWidth, "Side Panel width (b)")
            .Must(Math.Abs(remainder) <= 1e-6,
                $"Internal width ({internalWidth}) cannot be divided exactly " +
                $"into panels of width {options.PanelWidth}. Remainder: {remainder:F4}")
            .Check();
    }
}
