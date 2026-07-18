using System.Collections.ObjectModel;
using Core.Validators;

namespace Core.Layouts;

/// <summary>
/// Represents the definition and calculation of a panel layout.
/// Encapsulates geometry rules and coordinate generation.
/// </summary>
public class PanelLayout
{
    private readonly double _length;
    private readonly double _height;
    private readonly double _panelWidth;
    private readonly double _sidePanelWidth;
    private readonly double _targetOffset;

    /// <summary>
    /// Initializes a new Panel Layout configuration.
    /// </summary>
    /// <param name="length">Total length of the area (L).</param>
    /// <param name="height">Total height of the area (B).</param>
    /// <param name="panelWidth">Width of internal panels (a).</param>
    /// <param name="sidePanelWidth">Width of side panels (b).</param>
    /// <param name="targetOffset">Distance from the edge (f).</param>
    /// <exception cref="ValidationException">Thrown when dimensions are invalid.</exception>
    public PanelLayout(double length, double height, double panelWidth, double sidePanelWidth, double targetOffset)
    {
        var internalWidth = length - 2 * sidePanelWidth;
        var panelCount = (int)(internalWidth / panelWidth);
        var remainder = internalWidth - panelCount * panelWidth;

        new DoubleValidator()
            .MustBePositive(length, "Length (L)")
            .MustBePositive(height, "Height (B)")
            .MustBePositive(panelWidth, "Panel width (a)")
            .MustBePositive(sidePanelWidth, "Side panel width (b)")
            .MustNotBeNegative(targetOffset, "Offset (f)")
            .MustBeLessThan(targetOffset, panelWidth, "Offset (f)", "panel width (a)")
            .MustBeInRange(sidePanelWidth, panelWidth / 2, panelWidth, "Side Panel width (b)")
            .Must(Math.Abs(remainder) <= 1e-6,
                $"Internal width ({internalWidth}) cannot be divided exactly " +
                $"into panels of width {panelWidth}. Remainder: {remainder:F4}")
            .Check()
            .ThrowIfInvalid();

        _length = length;
        _height = height;
        _panelWidth = panelWidth;
        _sidePanelWidth = sidePanelWidth;
        _targetOffset = targetOffset;

        InitializeLayout();
    }

    /// <summary>
    /// Gets the total number of internal panels calculated.
    /// </summary>
    public int PanelCount { get; private set; }

    /// <summary>
    /// Gets a read-only collection of calculated target coordinates.
    /// </summary>
    public IReadOnlyList<Point2D> TargetPoints { get; private set; } = [];

    /// <summary>
    /// Initializes the layout calculations.
    /// </summary>
    private void InitializeLayout()
    {
        var internalWidth = _length - 2 * _sidePanelWidth;

        PanelCount = (int)(internalWidth / _panelWidth) + 2;

        var innerPanelsPoints = new List<Point2D>(PanelCount) { new(_targetOffset, _targetOffset) };

        for (var i = 0; i < PanelCount - 2; i++)
        {
            var panelStartX = _sidePanelWidth + i * _panelWidth;
            var localOffset = i % 2 == 0 ? _targetOffset : _panelWidth - _targetOffset;
            var targetX = panelStartX + localOffset;

            innerPanelsPoints.Add(new Point2D(targetX, _targetOffset));
        }

        innerPanelsPoints.Add(new Point2D(_length - _targetOffset, _height - _targetOffset));

        TargetPoints = new ReadOnlyCollection<Point2D>(innerPanelsPoints);
    }

    /// <summary>
    /// Returns a string representation of the layout.
    /// </summary>
    public override string ToString()
    {
        return
            $"[PanelLayout: L={_length}, B={_height}, a={_panelWidth}, b={_sidePanelWidth}, f={_targetOffset}, Panels={PanelCount}]";
    }
}