using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Core.Layouts;
using Core.Processors;
using Core.Validators;
using Presentation.Models;

namespace Presentation.ViewModels;

public class PanelPointsViewModel : ViewModelBase
{
    private readonly PanelPointsCalculator _calculator = new(new PanelLayoutOptionsValidator());

    private string _length = "6000";
    private string _height = "1200";
    private string _panelWidth = "600";
    private string _sidePanelWidth = "600";
    private string _targetOffset = "50";
    private string _statusMessage = "";
    private bool _hasError;
    private PanelLayout? _lastOptions;
    private PanelPointCalculationResult? _lastResult;

    public string Length { get => _length; set => SetProperty(ref _length, value); }
    public string Height { get => _height; set => SetProperty(ref _height, value); }
    public string PanelWidth { get => _panelWidth; set => SetProperty(ref _panelWidth, value); }
    public string SidePanelWidth { get => _sidePanelWidth; set => SetProperty(ref _sidePanelWidth, value); }
    public string TargetOffset { get => _targetOffset; set => SetProperty(ref _targetOffset, value); }

    public ObservableCollection<PointDisplay> Points { get; } = [];

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

    public PanelLayout? LastOptions => _lastOptions;
    public PanelPointCalculationResult? LastResult => _lastResult;

    public ICommand BuildCommand => new RelayCommand(Build);

    private void Build()
    {
        HasError = false;
        StatusMessage = "";

        if (!TryParse(Length, "Length", out var length) ||
            !TryParse(Height, "Height", out var height) ||
            !TryParse(PanelWidth, "Panel Width", out var panelWidth) ||
            !TryParse(SidePanelWidth, "Side Panel Width", out var sidePanelWidth) ||
            !TryParse(TargetOffset, "Target Offset", out var targetOffset))
            return;

        try
        {
            var options = new PanelLayout(length, height, panelWidth, sidePanelWidth, targetOffset);
            var result = _calculator.Calculate(options);

            _lastOptions = options;
            _lastResult = result;

            Points.Clear();
            for (var i = 0; i < result.PanelCount; i++)
            {
                Points.Add(new PointDisplay
                {
                    Index = i + 1,
                    LocalX = result.TargetPoints[i].X,
                    LocalY = result.TargetPoints[i].Y,
                    GlobalX = result.GlobalTargetPoints[i].X,
                    GlobalY = result.GlobalTargetPoints[i].Y
                });
            }

            StatusMessage = $"OK — {result.PanelCount} points, {result.PanelCount - 2} internal panels";
            OnPropertyChanged(nameof(LastOptions));
            OnPropertyChanged(nameof(LastResult));
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
