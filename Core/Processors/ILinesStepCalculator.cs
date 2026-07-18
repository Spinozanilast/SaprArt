using Core.Layouts;

namespace Core.Processors;

public interface ILinesStepCalculator
{
    HashSet<Line2D> Calculate(IOpeningPanel openingPanel, double step);
}