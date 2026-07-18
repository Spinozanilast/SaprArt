using System.Collections.ObjectModel;

namespace Core.Layouts;

public class PanelPointCalculationResult(
    int panelCount,
    List<Point2D> targetPoints,
    List<Point2D> globalTargetPoints)
{
    public int PanelCount { get; } = panelCount;
    public IReadOnlyList<Point2D> TargetPoints { get; } = new ReadOnlyCollection<Point2D>(targetPoints);
    public IReadOnlyList<Point2D> GlobalTargetPoints { get; } = new ReadOnlyCollection<Point2D>(globalTargetPoints);
}
