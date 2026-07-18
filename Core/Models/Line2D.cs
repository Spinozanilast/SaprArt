namespace Core.Layouts;

public readonly struct Line2D
{
    public Point2D Start { get; }
    public Point2D End { get; }

    public Line2D(Point2D startPoint, Point2D endPoint)
    {
        Start = startPoint;
        End = endPoint;
    }

    public override string ToString() => $"(Start: {Start}, End: {End})";
}