namespace Core.Layouts;

public class ZonedPanel(
    double width, double length,
    double leftSocketHeight, double leftSocketWidth,
    double rightSocketHeight, double rightSocketWidth,
    double openingMinX, double openingWidth,
    double openingMinY, double openingHeight) : IZonedPanel
{
    public double Width { get; } = width;
    public double Length { get; } = length;
    public double LeftSocketHeight { get; } = leftSocketHeight;
    public double LeftSocketWidth { get; } = leftSocketWidth;
    public double RightSocketHeight { get; } = rightSocketHeight;
    public double RightSocketWidth { get; } = rightSocketWidth;
    public double OpeningMinX { get; } = openingMinX;
    public double OpeningWidth { get; } = openingWidth;
    public double OpeningMinY { get; } = openingMinY;
    public double OpeningHeight { get; } = openingHeight;
}
