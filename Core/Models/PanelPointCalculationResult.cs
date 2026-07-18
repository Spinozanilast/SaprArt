using System.Collections.ObjectModel;

namespace Core.Layouts;

public class PanelPointCalculationResult
{
    public int PanelCount { get; }
    public IReadOnlyList<Point2D> TargetPoints { get; }
    public IReadOnlyList<Point2D> GlobalTargetPoints { get; }

    public PanelPointCalculationResult(int panelCount, List<Point2D> targetPoints, List<Point2D> globalTargetPoints)
    {
        PanelCount = panelCount;
        TargetPoints = new ReadOnlyCollection<Point2D>(targetPoints);
        GlobalTargetPoints = new ReadOnlyCollection<Point2D>(globalTargetPoints);
    }
}
