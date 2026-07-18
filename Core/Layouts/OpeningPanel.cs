namespace Core.Layouts;

public class OpeningPanel : IOpeningPanel
{
    public double Width { get; }
    public double Length { get; }
    public double OpeningMinX { get; }
    public double OpeningWidth { get; }
    public double OpeningMinY { get; }
    public double OpeningHeight { get; }

    public OpeningPanel(double width, double length,
        double openingMinX, double openingWidth, double openingMinY, double openingHeight)
    {
        Width = width;
        Length = length;
        OpeningMinX = openingMinX;
        OpeningWidth = openingWidth;
        OpeningMinY = openingMinY;
        OpeningHeight = openingHeight;
    }
}