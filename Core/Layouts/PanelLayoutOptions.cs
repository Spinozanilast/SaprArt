namespace Core.Layouts;

public class PanelLayoutOptions
{
    public double Length { get; }
    public double Height { get; }
    public double PanelWidth { get; }
    public double SidePanelWidth { get; }
    public double TargetOffset { get; }

    public PanelLayoutOptions(double length, double height, double panelWidth, double sidePanelWidth, double targetOffset)
    {
        Length = length;
        Height = height;
        PanelWidth = panelWidth;
        SidePanelWidth = sidePanelWidth;
        TargetOffset = targetOffset;
    }
}
