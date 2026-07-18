namespace Core.Layouts;

public class PanelForLayout
{
    public double Length { get; }
    public double Height { get; }
    public double PanelWidth { get; }
    public double SidePanelWidth { get; }
    public double TargetOffset { get; }

    public PanelForLayout(double length, double height, double panelWidth, double sidePanelWidth, double targetOffset)
    {
        Length = length;
        Height = height;
        PanelWidth = panelWidth;
        SidePanelWidth = sidePanelWidth;
        TargetOffset = targetOffset;
    }
}
