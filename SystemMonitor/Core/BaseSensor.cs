namespace SystemMonitor.Core;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using System.Windows;
using SkiaSharp;
public abstract class BaseSensor : ISensor, INotifyPropertyChanged
{
    private readonly ObservableCollection<double> _chartValues;
    public string Name { get; protected set;}
    private string _displayText;
    public string DisplayText
    {
        get => _displayText;
        protected set { _displayText = value; OnPropertyChanged(); }
    }
    private double _currentValue;

    public double CurrentValue
    {
        get => _currentValue;
        protected set
        {
            _currentValue = value;
            OnPropertyChanged();
            
            AddToHistory(value);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public ISeries[] Series { get; set; }
    public Axis[] XAxes { get; set; } = { new Axis { IsVisible = false } };
    public Axis[] YAxes { get; set; } = { new Axis { MinLimit = 0, MaxLimit = 100, IsVisible = false } };
    public abstract void Update();

    protected BaseSensor(string name)
    {
        Name = name;
        _chartValues = new ObservableCollection<double>();
        
        Series = new ISeries[]
        {
            new LineSeries<double>
            {
                Values = _chartValues,
                Fill = new LinearGradientPaint(
                    new SKColor(50, 50, 200, 50),
                    new SKColor(50, 50, 200, 10)), 
                Stroke = new SolidColorPaint(new SKColor(50, 50, 200)) { StrokeThickness = 2 },
                GeometrySize = 0, // Brak kropek
                LineSmoothness = 1 // Wygładzanie
            }
        };
    }

    

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    private void AddToHistory(double value)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            _chartValues.Add(value);

            if (_chartValues.Count > 40)
            {
                _chartValues.RemoveAt(0);
            }
        });
    }
    
}