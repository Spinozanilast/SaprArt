namespace Core.Layouts;

public record Line2D(Point2D Start, Point2D End)
{
    public override string ToString() => $"(Start: {Start}, End: {End})";
}
