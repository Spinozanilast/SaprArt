namespace Core.Layouts;

public class VerticalZone
{
    public string Name { get; }
    public double X { get; }
    public double Width { get; }
    public double Height { get; }

    public VerticalZone(string name, double x, double width, double height)
    {
        Name = name;
        X = x;
        Width = width;
        Height = height;
    }

    public override string ToString() => $"{Name}: X={X}, W={Width}, H={Height}";
}