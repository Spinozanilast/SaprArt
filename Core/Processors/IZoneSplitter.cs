using Core.Layouts;

namespace Core.Processors;

public interface IZoneSplitter
{
    IReadOnlyList<VerticalZone> Split(IZonedPanel panel);
}
