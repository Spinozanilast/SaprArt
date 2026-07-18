using Core.Layouts;
using Core.Validators;

namespace Core.Processors;

public class PanelPointCalculator : IPanelPointCalculator
{
    public PanelPointCalculationResult Calculate(PanelForLayout options)
    {
        new PanelLayoutOptionsValidator().Validate(options).ThrowIfInvalid();

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
            var localX = i % 2 == 0 ? options.TargetOffset : options.PanelWidth - options.TargetOffset;

            localPoints.Add(new Point2D(localX, options.TargetOffset));
            globalPoints.Add(new Point2D(panelStartX + localX, options.TargetOffset));
        }

        localPoints.Add(new Point2D(options.SidePanelWidth - options.TargetOffset, options.Height - options.TargetOffset));
        globalPoints.Add(new Point2D(options.Length - options.TargetOffset, options.Height - options.TargetOffset));

        return new PanelPointCalculationResult(panelCount, localPoints, globalPoints);
    }
}
