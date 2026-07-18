using Core.Layouts;

namespace Core.Processors;

public interface IPanelPointCalculator
{
    PanelPointCalculationResult Calculate(PanelForLayout options);
}
