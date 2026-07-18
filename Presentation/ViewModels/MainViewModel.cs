namespace Presentation.ViewModels;

public class MainViewModel : ViewModelBase
{
    public PanelPointsViewModel PanelPoints { get; } = new();
    public ZonedPanelViewModel ZonedPanel { get; } = new();
    public HorizontalLinesViewModel HorizontalLines { get; } = new();
}
