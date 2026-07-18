using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Core.Layouts;

namespace Presentation.Controls;

public class DrawingPanel : Canvas
{
    private static readonly Brush PanelFill = new SolidColorBrush(Color.FromRgb(0x1e, 0x29, 0x3b));
    private static readonly Brush PanelStroke = new SolidColorBrush(Color.FromRgb(0x94, 0xa3, 0xb8));
    private static readonly Brush SeparatorStroke = new SolidColorBrush(Color.FromRgb(0x47, 0x55, 0x69));
    private static readonly Brush SidePanelStroke = new SolidColorBrush(Color.FromRgb(0x64, 0x74, 0x8b));
    private static readonly Brush PointFill = new SolidColorBrush(Color.FromRgb(0x22, 0xd3, 0xee));
    private static readonly Brush PointStroke = new SolidColorBrush(Color.FromRgb(0x0e, 0x74, 0x90));
    private static readonly Brush LabelBrush = new SolidColorBrush(Color.FromRgb(0x94, 0xa3, 0xb8));
    private static readonly Brush DimLabelBrush = new SolidColorBrush(Color.FromRgb(0x64, 0x74, 0x8b));
    private static readonly Brush LineBrush = new SolidColorBrush(Color.FromRgb(0x22, 0xd3, 0xee));
    private static readonly Brush OpeningFill = new SolidColorBrush(Color.FromRgb(0x0c, 0x0a, 0x09));
    private static readonly Brush OpeningStroke = new SolidColorBrush(Color.FromRgb(0xf9, 0x73, 0x16));
    private static readonly Brush LeftSocketFill = new SolidColorBrush(Color.FromRgb(0x78, 0x35, 0x0f));
    private static readonly Brush LeftSocketStroke = new SolidColorBrush(Color.FromRgb(0xf5, 0x9e, 0x0b));
    private static readonly Brush RightSocketFill = new SolidColorBrush(Color.FromRgb(0x83, 0x18, 0x43));
    private static readonly Brush RightSocketStroke = new SolidColorBrush(Color.FromRgb(0xec, 0x48, 0x99));
    private static readonly Brush OpeningZoneFill = new SolidColorBrush(Color.FromRgb(0x13, 0x4e, 0x4a));
    private static readonly Brush OpeningZoneStroke = new SolidColorBrush(Color.FromRgb(0x2d, 0xd4, 0xbf));

    public DrawingPanel()
    {
        Background = new SolidColorBrush(Color.FromRgb(0x0f, 0x17, 0x2a));
    }

    public void DrawPanelPoints(PanelLayout options, PanelPointCalculationResult result)
    {
        Clear();
        if (options.Length <= 0 || options.Height <= 0) return;

        var scale = CalculateScale(options.Length, options.Height);
        var ox = 40.0;
        var oy = 40.0;

        DrawRect(ox, oy, 0, 0, options.Length, options.Height, PanelFill, PanelStroke, 1.5, scale);

        var secondSideStart = options.Length - options.SidePanelWidth;
        DrawLine(ox, oy, options.SidePanelWidth, 0, options.SidePanelWidth, options.Height, SidePanelStroke, 1, scale);
        DrawLine(ox, oy, secondSideStart, 0, secondSideStart, options.Height, SidePanelStroke, 1, scale);

        var internalWidth = options.Length - 2 * options.SidePanelWidth;
        var internalPanelCount = (int)(internalWidth / options.PanelWidth);
        for (var i = 1; i < internalPanelCount; i++)
        {
            var x = options.SidePanelWidth + i * options.PanelWidth;
            DrawDashedLine(ox, oy, x, 0, x, options.Height, SeparatorStroke, 0.8, scale);
        }

        var idx = 1;
        foreach (var point in result.GlobalTargetPoints)
        {
            var screenY = options.Height - point.Y;
            DrawCircle(ox, oy, point.X, screenY, 4, PointFill, PointStroke, 1, scale);
            DrawText(ox + point.X * scale + 8, oy + screenY * scale - 4, idx.ToString(), 8, LabelBrush, false);
            idx++;
        }

        DrawDimLabel(ox, oy, options.Length / 2, -12, $"{options.Length} mm", scale, true);
        DrawDimLabel(ox - 30, oy, 0, options.Height / 2, $"{options.Height} mm", scale, false);

        DrawLegend(ox, oy, scale, options.Length, options.Height,
            ("#22d3ee", "Target Point"));
    }

    public void DrawZonedPanel(IZonedPanel panel, IReadOnlyList<VerticalZone> zones)
    {
        Clear();
        if (panel.Width <= 0 || panel.Length <= 0) return;

        var scale = CalculateScale(panel.Width, panel.Length);
        var ox = 40.0;
        var oy = 40.0;

        DrawRect(ox, oy, 0, 0, panel.Width, panel.Length, Brushes.Transparent, PanelStroke, 1.5, scale);

        var columns = zones.GroupBy(z => z.X).OrderBy(g => g.Key).ToList();
        foreach (var column in columns)
        {
            double currentY = 0;
            foreach (var zone in column)
            {
                var (fill, stroke) = GetZoneBrushes(zone.Name);
                DrawRect(ox, oy, zone.X, currentY, zone.Width, zone.Height, fill, stroke, 0.8, scale);

                if (zone.Height * scale > 30)
                {
                    var cx = zone.X + zone.Width / 2;
                    var cy = currentY + zone.Height / 2;
                    DrawText(ox + cx * scale, oy + cy * scale - 5, zone.Name, 7, LabelBrush, true);
                    if (zone.Height * scale > 48)
                        DrawText(ox + cx * scale, oy + cy * scale + 7, $"{zone.Width:F0}×{zone.Height:F0}", 6, DimLabelBrush, true);
                }

                currentY += zone.Height;
            }
        }

        DrawDimLabel(ox, oy, panel.Width / 2, -12, $"{panel.Width} mm", scale, true);
        DrawDimLabel(ox - 30, oy, 0, panel.Length / 2, $"{panel.Length} mm", scale, false);

        DrawLegend(ox, oy, scale, panel.Width, panel.Length,
            ("#78350f", "Left Socket"),
            ("#831843", "Right Socket"),
            ("#0c0a09", "Opening Cutout"),
            ("#134e4a", "Opening Area"),
            ("#1e293b", "Solid Panel"));
    }

    public void DrawHorizontalLines(IOpeningPanel panel, HashSet<Line2D> lines)
    {
        Clear();
        if (panel.Width <= 0 || panel.Length <= 0) return;

        var scale = CalculateScale(panel.Width, panel.Length);
        var ox = 40.0;
        var oy = 40.0;

        DrawRect(ox, oy, 0, 0, panel.Width, panel.Length, PanelFill, PanelStroke, 1.5, scale);

        if (panel.OpeningWidth > 0 && panel.OpeningHeight > 0)
        {
            DrawRect(ox, oy, panel.OpeningMinX, panel.Length - panel.OpeningMinY - panel.OpeningHeight,
                panel.OpeningWidth, panel.OpeningHeight, OpeningFill, OpeningStroke, 1.2, scale, new DoubleCollection { 4, 2 });
        }

        var idx = 1;
        foreach (var line in lines)
        {
            var screenY1 = panel.Length - line.Start.Y;
            var screenY2 = panel.Length - line.End.Y;
            DrawLine(ox, oy, line.Start.X, screenY1, line.End.X, screenY2, LineBrush, 1, scale);

            var midScreenY = (screenY1 + screenY2) / 2;
            var midX = (line.Start.X + line.End.X) / 2;
            DrawText(ox + midX * scale, oy + midScreenY * scale - 10, idx.ToString(), 7, LabelBrush, true);
            idx++;
        }

        DrawDimLabel(ox, oy, panel.Width / 2, -12, $"{panel.Width} mm", scale, true);
        DrawDimLabel(ox - 30, oy, 0, panel.Length / 2, $"{panel.Length} mm", scale, false);

        DrawLegend(ox, oy, scale, panel.Width, panel.Length,
            ("#22d3ee", "Horizontal Line"),
            ("#0c0a09", "Opening Cutout"));
    }

    public void Clear() => Children.Clear();

    private double CalculateScale(double modelWidth, double modelHeight)
    {
        if (modelWidth <= 0 || modelHeight <= 0) return 1;
        var availableWidth = ActualWidth > 0 ? ActualWidth - 80 : 600;
        var availableHeight = ActualHeight > 0 ? ActualHeight - 80 : 400;
        return Math.Min(availableWidth / modelWidth, availableHeight / modelHeight);
    }

    private void DrawRect(double ox, double oy, double x, double y, double w, double h,
        Brush fill, Brush stroke, double strokeThickness, double scale,
        DoubleCollection? dashArray = null)
    {
        var rect = new Rectangle
        {
            Width = Math.Max(w * scale, 0.5),
            Height = Math.Max(h * scale, 0.5),
            Fill = fill,
            Stroke = stroke,
            StrokeThickness = strokeThickness
        };
        if (dashArray != null)
            rect.StrokeDashArray = dashArray;
        Canvas.SetLeft(rect, ox + x * scale);
        Canvas.SetTop(rect, oy + y * scale);
        Children.Add(rect);
    }

    private void DrawLine(double ox, double oy, double x1, double y1, double x2, double y2,
        Brush stroke, double strokeThickness, double scale)
    {
        Children.Add(new Line
        {
            X1 = ox + x1 * scale, Y1 = oy + y1 * scale,
            X2 = ox + x2 * scale, Y2 = oy + y2 * scale,
            Stroke = stroke, StrokeThickness = strokeThickness
        });
    }

    private void DrawDashedLine(double ox, double oy, double x1, double y1, double x2, double y2,
        Brush stroke, double strokeThickness, double scale)
    {
        Children.Add(new Line
        {
            X1 = ox + x1 * scale, Y1 = oy + y1 * scale,
            X2 = ox + x2 * scale, Y2 = oy + y2 * scale,
            Stroke = stroke, StrokeThickness = strokeThickness,
            StrokeDashArray = new DoubleCollection { 4, 2 }
        });
    }

    private void DrawCircle(double ox, double oy, double cx, double cy, double r,
        Brush fill, Brush stroke, double strokeThickness, double scale)
    {
        var ellipse = new Ellipse
        {
            Width = r * 2, Height = r * 2,
            Fill = fill, Stroke = stroke, StrokeThickness = strokeThickness
        };
        Canvas.SetLeft(ellipse, ox + cx * scale - r);
        Canvas.SetTop(ellipse, oy + cy * scale - r);
        Children.Add(ellipse);
    }

    private void DrawText(double x, double y, string text, double fontSize, Brush foreground, bool center)
    {
        var tb = new TextBlock
        {
            Text = text, FontSize = fontSize, Foreground = foreground,
            FontFamily = new FontFamily("Consolas")
        };
        tb.Measure(Size.Empty);
        var left = center ? x - tb.DesiredSize.Width / 2 : x;
        var top = center ? y - tb.DesiredSize.Height / 2 : y - tb.DesiredSize.Height / 2;
        Canvas.SetLeft(tb, left);
        Canvas.SetTop(tb, top);
        Children.Add(tb);
    }

    private void DrawDimLabel(double ox, double oy, double modelX, double modelY, string text, double scale, bool horizontal)
    {
        var tb = new TextBlock
        {
            Text = text, FontSize = 10, Foreground = LabelBrush,
            FontFamily = new FontFamily("Consolas")
        };
        tb.Measure(Size.Empty);
        var x = horizontal ? ox + modelX * scale - tb.DesiredSize.Width / 2 : ox + modelX * scale;
        var y = horizontal ? oy + modelY * scale : oy + modelY * scale - tb.DesiredSize.Height / 2;
        Canvas.SetLeft(tb, x);
        Canvas.SetTop(tb, y);
        Children.Add(tb);
    }

    private static (Brush Fill, Brush Stroke) GetZoneBrushes(string name) => name switch
    {
        "PanelSolid" => (PanelFill, PanelStroke),
        "LeftSocketTop" or "LeftSocketBottom" => (LeftSocketFill, LeftSocketStroke),
        "RightSocketTop" or "RightSocketBottom" => (RightSocketFill, RightSocketStroke),
        "OpeningCutout" => (OpeningFill, OpeningStroke),
        "OpeningTop" or "OpeningBottom" => (OpeningZoneFill, OpeningZoneStroke),
        _ => (PanelFill, SeparatorStroke)
    };

    private void DrawLegend(double ox, double oy, double scale, double modelW, double modelH,
        params (string Color, string Label)[] items)
    {
        var legendX = ox + modelW * scale + 16;
        var legendY = oy;
        var swatch = 8.0;
        var lineH = 14.0;

        var tb = new TextBlock
        {
            Text = "Legend", FontSize = 9, Foreground = DimLabelBrush,
            FontFamily = new FontFamily("Consolas"), FontWeight = FontWeights.SemiBold
        };
        Canvas.SetLeft(tb, legendX);
        Canvas.SetTop(tb, legendY);
        Children.Add(tb);
        legendY += lineH + 2;

        foreach (var (hex, label) in items)
        {
            var r = byte.Parse(hex[1..3], System.Globalization.NumberStyles.HexNumber);
            var g = byte.Parse(hex[3..5], System.Globalization.NumberStyles.HexNumber);
            var b = byte.Parse(hex[5..7], System.Globalization.NumberStyles.HexNumber);
            var brush = new SolidColorBrush(Color.FromRgb(r, g, b));

            var rect = new Rectangle { Width = swatch, Height = swatch, Fill = brush, Stroke = DimLabelBrush, StrokeThickness = 0.5 };
            Canvas.SetLeft(rect, legendX);
            Canvas.SetTop(rect, legendY + 1);
            Children.Add(rect);

            var lbl = new TextBlock
            {
                Text = label, FontSize = 8, Foreground = LabelBrush,
                FontFamily = new FontFamily("Consolas")
            };
            Canvas.SetLeft(lbl, legendX + swatch + 5);
            Canvas.SetTop(lbl, legendY);
            Children.Add(lbl);

            legendY += lineH;
        }
    }
}
