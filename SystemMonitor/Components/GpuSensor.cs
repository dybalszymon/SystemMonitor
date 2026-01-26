using System.Diagnostics;
using SystemMonitor.Core;
using System.Linq;

namespace SystemMonitor.Components;

public class GpuSensor : BaseSensor
{
    public GpuSensor() : base("GPU")
    {
    }

    public override void Update()
    {
        try
        {
            var category = new PerformanceCounterCategory("GPU Engine");
            var instances = category.GetInstanceNames();
            float totalUsage = 0;

            foreach (var instance in instances)
            {
                if (instance.EndsWith("engtype_3D"))
                {
                    using var counter = new PerformanceCounter("GPU Engine", "Utilization Percentage", instance);
                    totalUsage += counter.NextValue();
                }
            }

            CurrentValue = totalUsage;
            DisplayText = $"{totalUsage:F1} %";
        }
        catch
        {
            DisplayText = "GPU Error";
            CurrentValue = 0;
        }
    }
}