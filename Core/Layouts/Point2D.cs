namespace Core.Layouts;

public record Point2D(double X, double Y)
{
    public override string ToString() => $"({X:F3}, {Y:F3})";
}
