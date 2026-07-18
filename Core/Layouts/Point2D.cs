namespace Core.Layouts;

public readonly struct Point2D
{
    public double X { get; }
    public double Y { get; }

    public Point2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    public override string ToString() => $"({X:F3}, {Y:F3})";
}