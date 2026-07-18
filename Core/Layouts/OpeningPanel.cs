namespace Core.Layouts;

public class OpeningPanel(
    double width, double length,
    double openingMinX, double openingWidth,
    double openingMinY, double openingHeight) : IOpeningPanel
{
    public double Width { get; } = width;
    public double Length { get; } = length;
    public double OpeningMinX { get; } = openingMinX;
    public double OpeningWidth { get; } = openingWidth;
    public double OpeningMinY { get; } = openingMinY;
    public double OpeningHeight { get; } = openingHeight;
}
