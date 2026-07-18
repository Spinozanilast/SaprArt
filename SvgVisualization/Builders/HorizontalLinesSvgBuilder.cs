using System.Text;
using System.Globalization;
using Core.Layouts;

namespace SvgVisualization.Builders;

public static class HorizontalLinesSvgBuilder
{
    private const double Margin = 40;

    public static string Build(IOpeningPanel panel, HashSet<Line2D> lines)
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
        sb.AppendLine("    .hline { stroke: #22d3ee; stroke-width: 1; stroke-linecap: round; }");
        sb.AppendLine("    .opening-rect { fill: #0c0a09; stroke: #f97316; stroke-width: 1.2; stroke-dasharray: 6,3; }");
        sb.AppendLine("    .panel-outline { fill: #1e293b; stroke: #94a3b8; stroke-width: 1.5; }");
        sb.AppendLine("    .dim-label { fill: #94a3b8; font-size: 11px; font-family: Consolas, monospace; }");
        sb.AppendLine("  </style>");
        sb.AppendLine("</defs>");
        sb.AppendLine("<rect width=\"100%\" height=\"100%\" fill=\"url(#bg)\"/>");

        sb.AppendLine($"  <g transform=\"translate({Margin},{Margin})\">");

        sb.AppendLine($"    <rect x=\"0\" y=\"0\" width=\"{panel.Width:F1}\" height=\"{panel.Length:F1}\" class=\"panel-outline\"/>");

        if (panel.OpeningWidth > 0 && panel.OpeningHeight > 0)
        {
            var screenY = panel.Length - panel.OpeningMinY - panel.OpeningHeight;
            sb.AppendLine($"    <rect x=\"{panel.OpeningMinX:F1}\" y=\"{screenY:F1}\" width=\"{panel.OpeningWidth:F1}\" height=\"{panel.OpeningHeight:F1}\" class=\"opening-rect\"/>");
        }

        var idx = 1;
        foreach (var line in lines)
        {
            var screenY1 = panel.Length - line.Start.Y;
            var screenY2 = panel.Length - line.End.Y;
            sb.AppendLine($"    <line x1=\"{line.Start.X:F1}\" y1=\"{screenY1:F1}\" x2=\"{line.End.X:F1}\" y2=\"{screenY2:F1}\" class=\"hline\"/>");
            var midX = (line.Start.X + line.End.X) / 2;
            var midScreenY = (screenY1 + screenY2) / 2;
            sb.AppendLine($"    <text x=\"{midX:F1}\" y=\"{midScreenY - 6:F1}\" text-anchor=\"middle\" fill=\"#94a3b8\" font-size=\"8\" font-family=\"Consolas, monospace\">{idx}</text>");
            idx++;
        }

        sb.AppendLine($"    <text x=\"{panel.Width / 2:F1}\" y=\"{-12}\" text-anchor=\"middle\" class=\"dim-label\">{panel.Width} mm</text>");
        sb.AppendLine($"    <text x=\"{-14}\" y=\"{panel.Length / 2:F1}\" text-anchor=\"middle\" class=\"dim-label\" transform=\"rotate(-90,{-14},{panel.Length / 2:F1})\">{panel.Length} mm</text>");

        sb.AppendLine("  </g>");
        sb.AppendLine("</svg>");

        return sb.ToString();
    }
}
