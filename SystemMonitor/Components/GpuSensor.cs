using System.Diagnostics;
using SystemMonitor.Core;
using System.Collections.Generic;

namespace SystemMonitor.Components;

public class GpuSensor : BaseSensor
{
    private List<PerformanceCounter> _counters = new();

    public GpuSensor() : base("GPU")
    {
        InitializeCounters();
    }

    private void InitializeCounters()
    {
        try
        {
            var category = new PerformanceCounterCategory("GPU Engine");
            var instances = category.GetInstanceNames();

            foreach (var instance in instances)
            {
                if (instance.EndsWith("engtype_3D") || instance.EndsWith("engtype_VideoDecode"))
                {
                    var counter = new PerformanceCounter("GPU Engine", "Utilization Percentage", instance);
                    counter.NextValue(); 
                    _counters.Add(counter);
                }
            }
        }
        catch 
        {
        }
    }

    public override void Update()
    {
        if (_counters.Count == 0)
        {
            InitializeCounters();
        }

        float totalUsage = 0;
        foreach (var counter in _counters)
        {
            try 
            {
                totalUsage += counter.NextValue(); 
            } 
            catch 
            {
            }
        }

        CurrentValue = Math.Min(totalUsage, 100);
        DisplayText = $"{CurrentValue:F1} %";
    }
}