namespace Core.Layouts;

public class VerticalZone(string name, double x, double width, double height)
{
    public string Name { get; } = name;
    public double X { get; } = x;
    public double Width { get; } = width;
    public double Height { get; } = height;

    public override string ToString() => $"{Name}: X={X}, W={Width}, H={Height}";
}
