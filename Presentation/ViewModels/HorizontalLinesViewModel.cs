using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Core.Layouts;
using Core.Processors;
using Core.Validators;
using Presentation.Models;

namespace Presentation.ViewModels;

public class HorizontalLinesViewModel : ViewModelBase
{
    private readonly HorizontalLinesStepCalculator _calculator = new(new OpeningPanelValidator());

    private string _width = "2000";
    private string _length = "3000";
    private string _openingMinX = "600";
    private string _openingWidth = "800";
    private string _openingMinY = "500";
    private string _openingHeight = "1200";
    private string _step = "200";
    private string _statusMessage = "";
    private bool _hasError;
    private IOpeningPanel? _lastPanel;
    private HashSet<Line2D>? _lastLines;

    public string Width { get => _width; set => SetProperty(ref _width, value); }
    public string Length { get => _length; set => SetProperty(ref _length, value); }
    public string OpeningMinX { get => _openingMinX; set => SetProperty(ref _openingMinX, value); }
    public string OpeningWidth { get => _openingWidth; set => SetProperty(ref _openingWidth, value); }
    public string OpeningMinY { get => _openingMinY; set => SetProperty(ref _openingMinY, value); }
    public string OpeningHeight { get => _openingHeight; set => SetProperty(ref _openingHeight, value); }
    public string Step { get => _step; set => SetProperty(ref _step, value); }

    public ObservableCollection<LineDisplay> Lines { get; } = [];

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public bool HasError
    {
        get => _hasError;
        set => SetProperty(ref _hasError, value);
    }

    public IOpeningPanel? LastPanel => _lastPanel;
    public HashSet<Line2D>? LastLines => _lastLines;

    public ICommand BuildCommand => new RelayCommand(Build);

    private void Build()
    {
        HasError = false;
        StatusMessage = "";

        if (!TryParse(Width, "Width", out var width) ||
            !TryParse(Length, "Length", out var length) ||
            !TryParse(OpeningMinX, "Opening MinX", out var openingMinX) ||
            !TryParse(OpeningWidth, "Opening Width", out var openingWidth) ||
            !TryParse(OpeningMinY, "Opening MinY", out var openingMinY) ||
            !TryParse(OpeningHeight, "Opening Height", out var openingHeight) ||
            !TryParse(Step, "Step", out var step))
            return;

        try
        {
            var panel = new OpeningPanel(width, length, openingMinX, openingWidth, openingMinY, openingHeight);
            var lines = _calculator.Calculate(panel, step);

            _lastPanel = panel;
            _lastLines = lines;

            Lines.Clear();
            var idx = 1;
            foreach (var line in lines.OrderBy(l => l.Start.Y).ThenBy(l => l.Start.X))
            {
                var dx = line.End.X - line.Start.X;
                var dy = line.End.Y - line.Start.Y;
                Lines.Add(new LineDisplay
                {
                    Index = idx++,
                    StartX = line.Start.X,
                    StartY = line.Start.Y,
                    EndX = line.End.X,
                    EndY = line.End.Y,
                    Length = Math.Sqrt(dx * dx + dy * dy)
                });
            }

            StatusMessage = $"OK — {lines.Count} line segments";
            OnPropertyChanged(nameof(LastPanel));
            OnPropertyChanged(nameof(LastLines));
        }
        catch (ValidationException ex)
        {
            HasError = true;
            StatusMessage = ex.Message;
        }
        catch (Exception ex)
        {
            HasError = true;
            StatusMessage = ex.Message;
        }
    }

    private bool TryParse(string text, string field, out double value)
    {
        value = 0;
        if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
            return true;

        if (double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value))
            return true;

        HasError = true;
        StatusMessage = $"Invalid value for \"{field}\": \"{text}\"";
        return false;
    }
}
