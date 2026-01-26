using System.Diagnostics;
using System.Linq;
using SystemMonitor.Core;

namespace SystemMonitor.Components;

public class NetworkSensor : BaseSensor
{
    private PerformanceCounter? _counter;

    public NetworkSensor() : base("Network")
    {
        try
        {
            var category = new PerformanceCounterCategory("Network Interface");
            var instance = category.GetInstanceNames().FirstOrDefault();
            if (instance != null)
            {
                _counter = new PerformanceCounter("Network Interface", "Bytes Total/sec", instance);
            }
        }
        catch
        {
            _counter = null;
        }
    }

    public override void Update()
    {
        if (_counter == null)
        {
            DisplayText = "No Interface";
            CurrentValue = 0;
            return;
        }

        float bytesPerSec = _counter.NextValue();
        double kbps = (bytesPerSec * 8) / 1024.0;

        CurrentValue = Math.Min(kbps / 100, 100);
        DisplayText = $"{kbps:F1} kbps";
    }
}