using Core.Layouts;

namespace Core.Processors;

public interface IPanelPointsCalculator
{
    PanelPointCalculationResult Calculate(PanelLayoutOptions options);
}
