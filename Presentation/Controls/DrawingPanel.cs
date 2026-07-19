using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Core.Layouts;

namespace Presentation.Controls;

public class DrawingPanel : Canvas
{
    public DrawingPanel()
    {
        Background = PanelColors.CanvasBackground;
    }

    public void DrawPanelPoints(PanelLayout options, PanelPointCalculationResult result)
    {
        Clear();
        if (options.Length <= 0 || options.Height <= 0) return;

        var scale = CalculateScale(options.Length, options.Height);
        const double ox = 40.0;
        const double oy = 40.0;

        DrawRect(ox, oy, 0, 0, options.Length, options.Height, PanelColors.PanelFill, PanelColors.PanelStroke, 1.5,
            scale);

        var secondSideStart = options.Length - options.SidePanelWidth;
        DrawLine(ox, oy, options.SidePanelWidth, 0, options.SidePanelWidth, options.Height, PanelColors.SidePanelStroke,
            1, scale);
        DrawLine(ox, oy, secondSideStart, 0, secondSideStart, options.Height, PanelColors.SidePanelStroke, 1, scale);

        var internalWidth = options.Length - 2 * options.SidePanelWidth;
        var internalPanelCount = (int)(internalWidth / options.PanelWidth);
        for (var i = 1; i < internalPanelCount; i++)
        {
            var x = options.SidePanelWidth + i * options.PanelWidth;
            DrawDashedLine(ox, oy, x, 0, x, options.Height, PanelColors.SeparatorStroke, 0.8, scale);
        }

        var idx = 1;
        foreach (var point in result.GlobalTargetPoints)
        {
            var screenY = options.Height - point.Y;
            DrawCircle(ox, oy, point.X, screenY, 4, PanelColors.PointFill, PanelColors.PointStroke, 1, scale);
            DrawText(ox + point.X * scale + 8, oy + screenY * scale - 4, idx.ToString(), 8, PanelColors.LabelBrush,
                false);
            idx++;
        }

        DrawDimLabel(ox, oy, options.Length / 2, -12, $"{options.Length} mm", scale, true);
        DrawDimLabel(ox - 30, oy, 0, options.Height / 2, $"{options.Height} mm", scale, false);
    }

    public void DrawZonedPanel(IZonedPanel panel, IReadOnlyList<VerticalZone> zones)
    {
        Clear();
        if (panel.Width <= 0 || panel.Length <= 0) return;

        var scale = CalculateScale(panel.Width, panel.Length);
        const double ox = 40.0;
        const double oy = 40.0;

        DrawRect(ox, oy, 0, 0, panel.Width, panel.Length, Brushes.Transparent, PanelColors.PanelStroke, 1.5, scale);

        var columns = zones.GroupBy(z => z.X).OrderBy(g => g.Key).ToList();

        foreach (var column in columns)
        {
            double currentY = 0;
            foreach (var zone in column)
            {
                var (fill, stroke) = PanelColors.GetZoneBrushes(zone.Name);
                DrawRect(ox, oy, zone.X, currentY, zone.Width, zone.Height, fill, stroke, 0.8, scale);
                currentY += zone.Height;
            }
        }

        foreach (var column in columns)
        {
            double currentY = 0;
            foreach (var zone in column)
            {
                if (zone.Height * scale > 30)
                {
                    var cx = zone.X + zone.Width / 2;
                    var cy = currentY + zone.Height / 2;
                    DrawText(ox + cx * scale, oy + cy * scale - 5, zone.Name, 7, PanelColors.LabelBrush, true);
                    if (zone.Height * scale > 48)
                        DrawText(ox + cx * scale, oy + cy * scale + 7, $"{zone.Width:F0}×{zone.Height:F0}", 6,
                            PanelColors.DimLabelBrush, true);
                }

                currentY += zone.Height;
            }
        }

        DrawDimLabel(ox, oy, panel.Width / 2, -12, $"{panel.Width} mm", scale, true);
        DrawDimLabel(ox - 30, oy, 0, panel.Length / 2, $"{panel.Length} mm", scale, false);

        DrawLegend(ox, oy, scale, panel.Width, panel.Length,
            ("#1e293b", "Solid Panel"),
            ("#0c0a09", "Left Socket Top"),
            ("#3b0764", "Left Socket Bottom"),
            ("#0c0a09", "Right Socket Top"),
            ("#881337", "Right Socket Bottom"),
            ("#134e4a", "Opening Top"),
            ("#0c0a09", "Opening Cutout"),
            ("#064e3b", "Opening Bottom"));
    }

    public void DrawHorizontalLines(IOpeningPanel panel, HashSet<Line2D> lines)
    {
        Clear();
        if (panel.Width <= 0 || panel.Length <= 0) return;

        var scale = CalculateScale(panel.Width, panel.Length);
        const double ox = 40.0;
        const double oy = 40.0;

        DrawRect(ox, oy, 0, 0, panel.Width, panel.Length, PanelColors.PanelFill, PanelColors.PanelStroke, 1.5, scale);

        if (panel.OpeningWidth > 0 && panel.OpeningHeight > 0)
        {
            DrawRect(ox, oy, panel.OpeningMinX, panel.Length - panel.OpeningMinY - panel.OpeningHeight,
                panel.OpeningWidth, panel.OpeningHeight, PanelColors.OpeningFill, PanelColors.OpeningStroke, 1.2, scale,
                new DoubleCollection { 4, 2 });
        }

        var idx = 1;
        foreach (var line in lines)
        {
            var screenY1 = panel.Length - line.Start.Y;
            var screenY2 = panel.Length - line.End.Y;
            DrawLine(ox, oy, line.Start.X, screenY1, line.End.X, screenY2, PanelColors.LineBrush, 1, scale);

            var midScreenY = (screenY1 + screenY2) / 2;
            var midX = (line.Start.X + line.End.X) / 2;
            DrawText(ox + midX * scale, oy + midScreenY * scale - 10, idx.ToString(), 7, PanelColors.LabelBrush, true);
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
        SetLeft(rect, ox + x * scale);
        SetTop(rect, oy + y * scale);
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
        SetLeft(ellipse, ox + cx * scale - r);
        SetTop(ellipse, oy + cy * scale - r);
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
        SetLeft(tb, left);
        SetTop(tb, top);
        Panel.SetZIndex(tb, 10);
        Children.Add(tb);
    }

    private void DrawDimLabel(double ox, double oy, double modelX, double modelY, string text, double scale,
        bool horizontal)
    {
        var tb = new TextBlock
        {
            Text = text, FontSize = 10, Foreground = PanelColors.LabelBrush,
            FontFamily = new FontFamily("Consolas")
        };
        tb.Measure(Size.Empty);
        var x = horizontal ? ox + modelX * scale - tb.DesiredSize.Width / 2 : ox + modelX * scale;
        var y = horizontal ? oy + modelY * scale : oy + modelY * scale - tb.DesiredSize.Height / 2;
        SetLeft(tb, x);
        SetTop(tb, y);
        Panel.SetZIndex(tb, 10);
        Children.Add(tb);
    }

    private void DrawLegend(double ox, double oy, double scale, double modelW, double modelH,
        params (string Color, string Label)[] items)
    {
        var legendX = ox + modelW * scale + 24;
        var legendY = oy;
        const double swatch = 10.0;
        const double lineH = 18.0;

        var tb = new TextBlock
        {
            Text = "Legend", FontSize = 10, Foreground = PanelColors.DimLabelBrush,
            FontFamily = new FontFamily("Consolas"), FontWeight = FontWeights.SemiBold
        };
        SetLeft(tb, legendX);
        SetTop(tb, legendY);
        Panel.SetZIndex(tb, 10);
        Children.Add(tb);
        legendY += lineH + 4;

        foreach (var (hex, label) in items)
        {
            var r = byte.Parse(hex[1..3], System.Globalization.NumberStyles.HexNumber);
            var g = byte.Parse(hex[3..5], System.Globalization.NumberStyles.HexNumber);
            var b = byte.Parse(hex[5..7], System.Globalization.NumberStyles.HexNumber);
            var brush = new SolidColorBrush(Color.FromRgb(r, g, b));

            var rect = new Rectangle
            {
                Width = swatch, Height = swatch, Fill = brush, Stroke = PanelColors.DimLabelBrush, StrokeThickness = 0.5
            };
            SetLeft(rect, legendX);
            SetTop(rect, legendY + 2);
            Children.Add(rect);

            var lbl = new TextBlock
            {
                Text = label, FontSize = 9, Foreground = PanelColors.LabelBrush,
                FontFamily = new FontFamily("Consolas")
            };
            SetLeft(lbl, legendX + swatch + 6);
            SetTop(lbl, legendY);
            Panel.SetZIndex(lbl, 10);
            Children.Add(lbl);

            legendY += lineH;
        }
    }
}