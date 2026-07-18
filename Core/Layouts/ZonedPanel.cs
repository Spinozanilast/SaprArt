namespace Core.Layouts;

public class ZonedPanel : IZonedPanel
{
    public double Width { get; }
    public double Length { get; }
    public double LeftSocketHeight { get; }
    public double LeftSocketWidth { get; }
    public double RightSocketHeight { get; }
    public double RightSocketWidth { get; }
    public double OpeningMinX { get; }
    public double OpeningWidth { get; }
    public double OpeningMinY { get; }
    public double OpeningHeight { get; }

    public ZonedPanel(
        double width, double length,
        double leftSocketHeight, double leftSocketWidth,
        double rightSocketHeight, double rightSocketWidth,
        double openingMinX, double openingWidth, double openingMinY, double openingHeight)
    {
        Width = width;
        Length = length;
        LeftSocketHeight = leftSocketHeight;
        LeftSocketWidth = leftSocketWidth;
        RightSocketHeight = rightSocketHeight;
        RightSocketWidth = rightSocketWidth;
        OpeningMinX = openingMinX;
        OpeningWidth = openingWidth;
        OpeningMinY = openingMinY;
        OpeningHeight = openingHeight;
    }
}
