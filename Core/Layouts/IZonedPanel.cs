namespace Core.Layouts;

public interface IZonedPanel : IOpeningPanel
{
    double LeftSocketHeight { get; }
    double LeftSocketWidth { get; }
    double RightSocketHeight { get; }
    double RightSocketWidth { get; }
}