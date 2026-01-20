namespace SystemMonitor.Core;

public interface ISensor
{
    string Name { get; }
    string DisplayText { get; }
    double CurrentValue { get; }
    
    void Update();
}