using System.Text;
using System.Globalization;
using Core.Layouts;

namespace SvgVisualization.Builders;

public static class ZonedPanelSvgBuilder
{
    private const double Margin = 40;

    private static readonly Dictionary<string, (string Fill, string Stroke)> ZoneColors = new()
    {
        ["PanelSolid"] = ("#1e293b", "#94a3b8"),
        ["LeftSocketTop"] = ("#78350f", "#f59e0b"),
        ["LeftSocketBottom"] = ("#78350f", "#f59e0b"),
        ["RightSocketTop"] = ("#831843", "#ec4899"),
        ["RightSocketBottom"] = ("#831843", "#ec4899"),
        ["OpeningTop"] = ("#134e4a", "#2dd4bf"),
        ["OpeningCutout"] = ("#0c0a09", "#f97316"),
        ["OpeningBottom"] = ("#134e4a", "#2dd4bf"),
    };

    public static string Build(IZonedPanel panel, IReadOnlyList<VerticalZone> zones)
    {
        var svgWidth = panel.Width + 2 * Margin;
        var svgHeight = panel.Length + 2 * Margin;

        var sb = new StringBuilder();
        sb.Append("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"").Append(svgWidth.ToString("F1", CultureInfo.InvariantCulture))
          .Append("\" height=\"").Append(svgHeight.ToString("F1", CultureInfo.InvariantCulture))
          .Append("\" viewBox=\"0 0 ").Append(svgWidth.ToString("F1", CultureInfo.InvariantCulture))
          .Append(' ').Append(svgHeight.ToString("F1", CultureInfo.InvariantCulture)).AppendLine("\">");
        sb.AppendLine("<defs>");
        sb.AppendLine("  <linearGradient id=\"bg\" x1=\"0\" y1=\"0\" x2=\"0\" y2=\"1\">");
        sb.AppendLine("    <stop offset=\"0%\" stop-color=\"#0f172a\"/>");
        sb.AppendLine("    <stop offset=\"100%\" stop-color=\"#1e293b\"/>");
        sb.AppendLine("  </linearGradient>");
        sb.AppendLine("  <style>");
        sb.AppendLine("    .zone-label { fill: #e2e8f0; font-size: 10px; font-family: Consolas, monospace; font-weight: 500; }");
        sb.AppendLine("    .dim-label { fill: #94a3b8; font-size: 11px; font-family: Consolas, monospace; }");
        sb.AppendLine("    .panel-border { fill: none; stroke: #475569; stroke-width: 1.5; }");
        sb.AppendLine("    .zone-border { stroke-width: 0.5; stroke-opacity: 0.4; }");
        sb.AppendLine("  </style>");
        sb.AppendLine("</defs>");
        sb.AppendLine("<rect width=\"100%\" height=\"100%\" fill=\"url(#bg)\"/>");

        sb.AppendLine($"  <g transform=\"translate({Margin},{Margin})\">");

        sb.AppendLine($"    <rect x=\"0\" y=\"0\" width=\"{panel.Width:F1}\" height=\"{panel.Length:F1}\" class=\"panel-border\"/>");

        var columns = zones.GroupBy(z => z.X).OrderBy(g => g.Key).ToList();
        foreach (var column in columns)
        {
            double currentY = 0;
            foreach (var zone in column)
            {
                var (fill, stroke) = ZoneColors.TryGetValue(zone.Name, out var c) ? c : ("#1e293b", "#64748b");
                var screenY = currentY;

                sb.AppendLine($"    <rect x=\"{zone.X:F1}\" y=\"{screenY:F1}\" width=\"{zone.Width:F1}\" height=\"{zone.Height:F1}\" fill=\"{fill}\" stroke=\"{stroke}\" class=\"zone-border\"/>");

                if (zone.Height > 20)
                {
                    var labelX = zone.X + zone.Width / 2;
                    var labelY = screenY + zone.Height / 2;
                    sb.AppendLine($"    <text x=\"{labelX:F1}\" y=\"{labelY:F1}\" text-anchor=\"middle\" dominant-baseline=\"middle\" class=\"zone-label\">{zone.Name}</text>");
                    if (zone.Height > 40)
                        sb.AppendLine($"    <text x=\"{labelX:F1}\" y=\"{labelY + 11:F1}\" text-anchor=\"middle\" dominant-baseline=\"middle\" fill=\"#64748b\" font-size=\"8\" font-family=\"Consolas, monospace\">{zone.Width:F0}×{zone.Height:F0}</text>");
                }

                currentY += zone.Height;
            }
        }

        sb.AppendLine($"    <text x=\"{panel.Width / 2:F1}\" y=\"{-12}\" text-anchor=\"middle\" class=\"dim-label\">{panel.Width} mm</text>");
        sb.AppendLine($"    <text x=\"{-14}\" y=\"{panel.Length / 2:F1}\" text-anchor=\"middle\" class=\"dim-label\" transform=\"rotate(-90,{-14},{panel.Length / 2:F1})\">{panel.Length} mm</text>");

        sb.AppendLine("  </g>");
        sb.AppendLine("</svg>");

        return sb.ToString();
    }
}
