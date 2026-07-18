using Core.Layouts;
using Core.Validators;

namespace Core.Processors;

public class PanelPointsCalculator(IPanelLayoutOptionsValidator validator) : IPanelPointsCalculator
{
    public PanelPointCalculationResult Calculate(PanelLayout options)
    {
        validator.Validate(options).ThrowIfInvalid();

        var internalWidth = options.Length - 2 * options.SidePanelWidth;
        var internalPanelCount = (int)(internalWidth / options.PanelWidth);
        var panelCount = internalPanelCount + 2;

        var localPoints = new List<Point2D>(panelCount);
        var globalPoints = new List<Point2D>(panelCount);

        localPoints.Add(new Point2D(options.TargetOffset, options.TargetOffset));
        globalPoints.Add(new Point2D(options.TargetOffset, options.TargetOffset));

        for (var i = 0; i < internalPanelCount; i++)
        {
            var panelStartX = options.SidePanelWidth + i * options.PanelWidth;

            localPoints.Add(new Point2D(options.TargetOffset, options.TargetOffset));
            globalPoints.Add(new Point2D(panelStartX + options.TargetOffset, options.TargetOffset));
        }

        localPoints.Add(new Point2D(options.SidePanelWidth - options.TargetOffset, options.Height - options.TargetOffset));
        globalPoints.Add(new Point2D(options.Length - options.TargetOffset, options.Height - options.TargetOffset));

        return new PanelPointCalculationResult(panelCount, localPoints, globalPoints);
    }
}
