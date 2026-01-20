using System.Diagnostics;
using SystemMonitor.Core;

namespace SystemMonitor.Components;

public class CpuSensor : BaseSensor
{
    private PerformanceCounter _cpuCounter;
    public CpuSensor() : base("CPU", "Initializing CPU sensor")
    {
        _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        _cpuCounter.NextValue();
    }

    public override void Update()
    {
        float cpuUsage = _cpuCounter.NextValue();
        
        CurrentValue = cpuUsage;
        DisplayText = $"{cpuUsage:F1} %";
    }
}