using System.Windows.Media;

namespace Presentation.Controls;

public static class PanelColors
{
    public static readonly Brush PanelFill = new SolidColorBrush(Color.FromRgb(0x1e, 0x29, 0x3b));
    public static readonly Brush PanelStroke = new SolidColorBrush(Color.FromRgb(0x94, 0xa3, 0xb8));
    public static readonly Brush SeparatorStroke = new SolidColorBrush(Color.FromRgb(0x47, 0x55, 0x69));
    public static readonly Brush SidePanelStroke = new SolidColorBrush(Color.FromRgb(0x64, 0x74, 0x8b));
    public static readonly Brush PointFill = new SolidColorBrush(Color.FromRgb(0x22, 0xd3, 0xee));
    public static readonly Brush PointStroke = new SolidColorBrush(Color.FromRgb(0x0e, 0x74, 0x90));
    public static readonly Brush LabelBrush = new SolidColorBrush(Color.FromRgb(0x94, 0xa3, 0xb8));
    public static readonly Brush DimLabelBrush = new SolidColorBrush(Color.FromRgb(0x64, 0x74, 0x8b));
    public static readonly Brush LineBrush = new SolidColorBrush(Color.FromRgb(0x22, 0xd3, 0xee));
    public static readonly Brush OpeningFill = new SolidColorBrush(Color.FromRgb(0x0c, 0x0a, 0x09));
    public static readonly Brush OpeningStroke = new SolidColorBrush(Color.FromRgb(0xf9, 0x73, 0x16));
    public static readonly Brush LeftSocketTopFill = OpeningFill;
    public static readonly Brush LeftSocketTopStroke = OpeningStroke;
    public static readonly Brush LeftSocketBottomFill = new SolidColorBrush(Color.FromRgb(0x3b, 0x07, 0x64));
    public static readonly Brush LeftSocketBottomStroke = new SolidColorBrush(Color.FromRgb(0xa8, 0x55, 0xf7));
    public static readonly Brush RightSocketTopFill = OpeningFill;
    public static readonly Brush RightSocketTopStroke = OpeningStroke;
    public static readonly Brush RightSocketBottomFill = new SolidColorBrush(Color.FromRgb(0x88, 0x13, 0x37));
    public static readonly Brush RightSocketBottomStroke = new SolidColorBrush(Color.FromRgb(0xf4, 0x3f, 0x5e));
    public static readonly Brush OpeningTopFill = new SolidColorBrush(Color.FromRgb(0x13, 0x4e, 0x4a));
    public static readonly Brush OpeningTopStroke = new SolidColorBrush(Color.FromRgb(0x2d, 0xd4, 0xbf));
    public static readonly Brush OpeningBottomFill = new SolidColorBrush(Color.FromRgb(0x06, 0x4e, 0x3b));
    public static readonly Brush OpeningBottomStroke = new SolidColorBrush(Color.FromRgb(0x34, 0xd3, 0x99));

    public static readonly Brush CanvasBackground = new SolidColorBrush(Color.FromRgb(0x0f, 0x17, 0x2a));

    public static (Brush Fill, Brush Stroke) GetZoneBrushes(string name) => name switch
    {
        "PanelSolid" => (PanelFill, PanelStroke),
        "LeftSocketTop" => (LeftSocketTopFill, LeftSocketTopStroke),
        "LeftSocketBottom" => (LeftSocketBottomFill, LeftSocketBottomStroke),
        "RightSocketTop" => (RightSocketTopFill, RightSocketTopStroke),
        "RightSocketBottom" => (RightSocketBottomFill, RightSocketBottomStroke),
        "OpeningCutout" => (OpeningFill, OpeningStroke),
        "OpeningTop" => (OpeningTopFill, OpeningTopStroke),
        "OpeningBottom" => (OpeningBottomFill, OpeningBottomStroke),
        _ => (PanelFill, SeparatorStroke)
    };
}
