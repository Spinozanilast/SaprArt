namespace Core.Layouts;

public class PanelLayoutOptions(
    double length, double height,
    double panelWidth, double sidePanelWidth,
    double targetOffset)
{
    public double Length { get; } = length;
    public double Height { get; } = height;
    public double PanelWidth { get; } = panelWidth;
    public double SidePanelWidth { get; } = sidePanelWidth;
    public double TargetOffset { get; } = targetOffset;
}
