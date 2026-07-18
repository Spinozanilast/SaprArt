namespace Core.Layouts;

public interface IOpeningPanel
{
    double Width { get; }

    double Length { get; }

    double OpeningMinX { get; }

    double OpeningWidth { get; }

    double OpeningMinY { get; }

    double OpeningHeight { get; }
}