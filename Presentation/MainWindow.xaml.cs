using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Presentation.Controls;
using Presentation.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Appearance;


namespace Presentation;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = _vm;
        SystemThemeWatcher.Watch(this);

        _vm.PanelPoints.PropertyChanged += OnPointsChanged;
        _vm.ZonedPanel.PropertyChanged += OnZonedChanged;
        _vm.HorizontalLines.PropertyChanged += OnLinesChanged;

        PointsDataGrid.ItemsSource = _vm.PanelPoints.Points;
        ZonesDataGrid.ItemsSource = _vm.ZonedPanel.Zones;
        LinesDataGrid.ItemsSource = _vm.HorizontalLines.Lines;

        Loaded += (_, _) => ForceRedrawActiveTab();
    }

    private void OnTabSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!IsLoaded) return;
        ForceRedrawActiveTab();
    }

    private void ForceRedrawActiveTab()
    {
        switch (MainTabs.SelectedIndex)
        {
            case 0:
                if (_vm.PanelPoints.LastOptions is { } o && _vm.PanelPoints.LastResult is { } r)
                    PointsCanvas.DrawPanelPoints(o, r);
                break;
            case 1:
                if (_vm.ZonedPanel.LastPanel is { } p && _vm.ZonedPanel.LastZones is { } z)
                    ZonedCanvas.DrawZonedPanel(p, z);
                break;
            case 2:
                if (_vm.HorizontalLines.LastPanel is { } lp && _vm.HorizontalLines.LastLines is { } ll)
                    LinesCanvas.DrawHorizontalLines(lp, ll);
                break;
        }
    }

    private void OnPointsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(PanelPointsViewModel.LastResult) or nameof(PanelPointsViewModel.LastOptions))
        {
            if (_vm.PanelPoints.LastOptions is { } o && _vm.PanelPoints.LastResult is { } r)
                PointsCanvas.DrawPanelPoints(o, r);
        }
    }

    private void OnZonedChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ZonedPanelViewModel.LastZones) or nameof(ZonedPanelViewModel.LastPanel))
        {
            if (_vm.ZonedPanel.LastPanel is { } p && _vm.ZonedPanel.LastZones is { } z)
                ZonedCanvas.DrawZonedPanel(p, z);
        }
    }

    private void OnLinesChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(HorizontalLinesViewModel.LastLines) or nameof(HorizontalLinesViewModel.LastPanel))
        {
            if (_vm.HorizontalLines.LastPanel is { } lp && _vm.HorizontalLines.LastLines is { } ll)
                LinesCanvas.DrawHorizontalLines(lp, ll);
        }
    }
}
