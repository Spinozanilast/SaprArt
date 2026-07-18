using System.Windows.Controls;
using Core.Layouts;
using Core.Processors;
using Core.Validators;

namespace Presentation.Controls;

public partial class SvgPreviewControl : UserControl
{
    private readonly PanelPointsCalculator _pointsCalculator = new(new PanelLayoutOptionsValidator());
    private readonly ZoneSplitter _zoneSplitter = new(new ZonedPanelValidator());
    private readonly HorizontalLinesStepCalculator _linesCalculator = new(new OpeningPanelValidator());

    public SvgPreviewControl()
    {
        InitializeComponent();
    }

    public void UpdateVisualization(PanelLayout options)
    {
        try
        {
            var result = _pointsCalculator.Calculate(options);
            PointsCanvas.DrawPanelPoints(options, result);
        }
        catch
        {
            PointsCanvas.Clear();
        }
    }

    public void UpdateVisualization(IZonedPanel panel)
    {
        try
        {
            var zones = _zoneSplitter.Split(panel);
            ZonedCanvas.DrawZonedPanel(panel, zones);
        }
        catch
        {
            ZonedCanvas.Clear();
        }
    }

    public void UpdateVisualization(IOpeningPanel panel, double step)
    {
        try
        {
            var lines = _linesCalculator.Calculate(panel, step);
            LinesCanvas.DrawHorizontalLines(panel, lines);
        }
        catch
        {
            LinesCanvas.Clear();
        }
    }
}
