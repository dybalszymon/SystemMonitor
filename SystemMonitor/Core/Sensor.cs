namespace SystemMonitor.Core;

public interface ISensor
{
    string Name { get; }
    string DisplayText { get; }
    double BarValue { get; }
    
    void Update();
}