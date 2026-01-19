namespace SystemMonitor.Core;

public abstract class BaseSensor : ISensor
{
    public string Name { get; protected set;}
    public string DisplayText { get; protected set;}
    public double BarValue { get; protected set; }
    
    public abstract void Update();

    protected BaseSensor(string name, string displayText)
    {
        Name = name;
        DisplayText = displayText;
        BarValue = 0;
    }
}