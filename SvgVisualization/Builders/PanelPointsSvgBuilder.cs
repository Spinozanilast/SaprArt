using System.Text;
using System.Globalization;
using Core.Layouts;

namespace SvgVisualization.Builders;

public static class PanelPointsSvgBuilder
{
    private const double Margin = 40;
    private const double PointRadius = 4;

    public static string Build(PanelLayout options, PanelPointCalculationResult result)
    {
        var svgWidth = options.Length + 2 * Margin;
        var svgHeight = options.Height + 2 * Margin;

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
        sb.AppendLine("  <filter id=\"glow\">");
        sb.AppendLine("    <feGaussianBlur stdDeviation=\"2\" result=\"blur\"/>");
        sb.AppendLine("    <feMerge><feMergeNode in=\"blur\"/><feMergeNode in=\"SourceGraphic\"/></feMerge>");
        sb.AppendLine("  </filter>");
        sb.AppendLine("  <style>");
        sb.AppendLine("    .panel-outline { fill: #1e293b; stroke: #94a3b8; stroke-width: 1.5; }");
        sb.AppendLine("    .panel-separator { stroke: #475569; stroke-width: 0.8; stroke-dasharray: 6,3; }");
        sb.AppendLine("    .side-panel { fill: none; stroke: #64748b; stroke-width: 1; }");
        sb.AppendLine("    .target-point { fill: #22d3ee; stroke: #0e7490; stroke-width: 1; filter: url(#glow); }");
        sb.AppendLine("    .label { fill: #94a3b8; font-size: 11px; font-family: Consolas, monospace; }");
        sb.AppendLine("  </style>");
        sb.AppendLine("</defs>");
        sb.AppendLine("<rect width=\"100%\" height=\"100%\" fill=\"url(#bg)\"/>");

        sb.AppendLine($"  <g transform=\"translate({Margin},{Margin})\">");

        sb.AppendLine($"    <rect x=\"0\" y=\"0\" width=\"{options.Length:F1}\" height=\"{options.Height:F1}\" class=\"panel-outline\"/>");

        var sidePanelEndX = options.SidePanelWidth;
        var secondSidePanelStartX = options.Length - options.SidePanelWidth;
        sb.AppendLine($"    <line x1=\"{sidePanelEndX:F1}\" y1=\"0\" x2=\"{sidePanelEndX:F1}\" y2=\"{options.Height:F1}\" class=\"side-panel\"/>");
        sb.AppendLine($"    <line x1=\"{secondSidePanelStartX:F1}\" y1=\"0\" x2=\"{secondSidePanelStartX:F1}\" y2=\"{options.Height:F1}\" class=\"side-panel\"/>");

        var internalWidth = options.Length - 2 * options.SidePanelWidth;
        var internalPanelCount = (int)(internalWidth / options.PanelWidth);
        for (var i = 1; i < internalPanelCount; i++)
        {
            var x = options.SidePanelWidth + i * options.PanelWidth;
            sb.AppendLine($"    <line x1=\"{x:F1}\" y1=\"0\" x2=\"{x:F1}\" y2=\"{options.Height:F1}\" class=\"panel-separator\"/>");
        }

        var idx = 1;
        foreach (var point in result.GlobalTargetPoints)
        {
            var screenY = options.Height - point.Y;
            sb.AppendLine($"    <circle cx=\"{point.X:F1}\" cy=\"{screenY:F1}\" r=\"{PointRadius}\" class=\"target-point\"/>");
            sb.AppendLine($"    <text x=\"{point.X + 8:F1}\" y=\"{screenY - 4:F1}\" class=\"label\" font-size=\"9\">{idx}</text>");
            idx++;
        }

        sb.AppendLine($"    <text x=\"{options.Length / 2:F1}\" y=\"{-12}\" text-anchor=\"middle\" class=\"label\">{options.Length} mm</text>");
        sb.AppendLine($"    <text x=\"{-14}\" y=\"{options.Height / 2:F1}\" text-anchor=\"middle\" class=\"label\" transform=\"rotate(-90,{-14},{options.Height / 2:F1})\">{options.Height} mm</text>");

        sb.AppendLine("  </g>");
        sb.AppendLine("</svg>");

        return sb.ToString();
    }
}
