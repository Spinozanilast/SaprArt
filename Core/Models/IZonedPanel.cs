namespace Core.Layouts;

public interface IZonedPanel
{
    double Width { get; }
    double Length { get; }
    double LeftSocketHeight { get; }
    double LeftSocketWidth { get; }
    double RightSocketHeight { get; }
    double RightSocketWidth { get; }
    double OpeningMinX { get; }
    double OpeningWidth { get; }
    double OpeningMinY { get; }
    double OpeningHeight { get; }
}