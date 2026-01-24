using System.Diagnostics;
using SystemMonitor.Core;
using System.Management;

namespace SystemMonitor.Components;

public class RamSensor : BaseSensor
{
    private PerformanceCounter _ramCounter;
    private double _ramTotal;
    public RamSensor() : base("RAM")
    {
        _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        _ramTotal = GetTotalRam();
    }

    private double GetTotalRam()
    {
        try
        {
            var searcher = new ManagementObjectSearcher("SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
            //WMI always return list so thats why we need to loop through it
            foreach (ManagementObject obj in searcher.Get())
            {
                long bytes = Convert.ToInt64(obj["TotalPhysicalMemory"]);
                return bytes / 1024.0 / 1024.0; //in megabytes
            }
        }
        catch
        {
            // ignored now 
        }

        return -1.0;
    }
    public override void Update()
    {
        double freeMb = _ramCounter.NextValue();
        double usedMb = _ramTotal - freeMb;
        if (_ramTotal > 0)
        {
            CurrentValue = (usedMb / _ramTotal) * 100;
        }
        DisplayText = $"{usedMb / 1024:F1} / {_ramTotal / 1024:F1} GB ";
    }
}