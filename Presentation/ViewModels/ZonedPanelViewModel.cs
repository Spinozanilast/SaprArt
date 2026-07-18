using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Core.Layouts;
using Core.Processors;
using Core.Validators;
using Presentation.Models;

namespace Presentation.ViewModels;

public class ZonedPanelViewModel : ViewModelBase
{
    private readonly ZoneSplitter _splitter = new(new ZonedPanelValidator());

    private string _width = "2000";
    private string _length = "3000";
    private string _leftSocketW = "200";
    private string _leftSocketH = "800";
    private string _rightSocketW = "200";
    private string _rightSocketH = "800";
    private string _openingMinX = "600";
    private string _openingWidth = "800";
    private string _openingMinY = "500";
    private string _openingHeight = "1200";
    private string _statusMessage = "";
    private bool _hasError;
    private IZonedPanel? _lastPanel;
    private IReadOnlyList<VerticalZone>? _lastZones;

    public string Width { get => _width; set => SetProperty(ref _width, value); }
    public string Length { get => _length; set => SetProperty(ref _length, value); }
    public string LeftSocketW { get => _leftSocketW; set => SetProperty(ref _leftSocketW, value); }
    public string LeftSocketH { get => _leftSocketH; set => SetProperty(ref _leftSocketH, value); }
    public string RightSocketW { get => _rightSocketW; set => SetProperty(ref _rightSocketW, value); }
    public string RightSocketH { get => _rightSocketH; set => SetProperty(ref _rightSocketH, value); }
    public string OpeningMinX { get => _openingMinX; set => SetProperty(ref _openingMinX, value); }
    public string OpeningWidth { get => _openingWidth; set => SetProperty(ref _openingWidth, value); }
    public string OpeningMinY { get => _openingMinY; set => SetProperty(ref _openingMinY, value); }
    public string OpeningHeight { get => _openingHeight; set => SetProperty(ref _openingHeight, value); }

    public ObservableCollection<ZoneDisplay> Zones { get; } = [];

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

    public IZonedPanel? LastPanel => _lastPanel;
    public IReadOnlyList<VerticalZone>? LastZones => _lastZones;

    public ICommand BuildCommand => new RelayCommand(Build);

    private void Build()
    {
        HasError = false;
        StatusMessage = "";

        if (!TryParse(Width, "Width", out var width) ||
            !TryParse(Length, "Length", out var length) ||
            !TryParse(LeftSocketW, "Left Socket W", out var leftSocketW) ||
            !TryParse(LeftSocketH, "Left Socket H", out var leftSocketH) ||
            !TryParse(RightSocketW, "Right Socket W", out var rightSocketW) ||
            !TryParse(RightSocketH, "Right Socket H", out var rightSocketH) ||
            !TryParse(OpeningMinX, "Opening MinX", out var openingMinX) ||
            !TryParse(OpeningWidth, "Opening Width", out var openingWidth) ||
            !TryParse(OpeningMinY, "Opening MinY", out var openingMinY) ||
            !TryParse(OpeningHeight, "Opening Height", out var openingHeight))
            return;

        try
        {
            var panel = new ZonedPanel(width, length, leftSocketH, leftSocketW, rightSocketH, rightSocketW,
                openingMinX, openingWidth, openingMinY, openingHeight);
            var zones = _splitter.Split(panel);

            _lastPanel = panel;
            _lastZones = zones;

            Zones.Clear();
            foreach (var zone in zones)
            {
                Zones.Add(new ZoneDisplay
                {
                    Name = zone.Name,
                    X = zone.X,
                    Width = zone.Width,
                    Height = zone.Height
                });
            }

            StatusMessage = $"OK — {zones.Count} zones";
            OnPropertyChanged(nameof(LastPanel));
            OnPropertyChanged(nameof(LastZones));
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
