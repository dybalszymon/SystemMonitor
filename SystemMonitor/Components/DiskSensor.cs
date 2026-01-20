using System.IO;
using SystemMonitor.Core;
namespace SystemMonitor.Components;

public class DiskSensor : BaseSensor
{
    private readonly DriveInfo _drive;
    public DiskSensor(string driveName) : base($"Disk {driveName}", $"Initializing disk {driveName} sensor")
    {
        _drive = new DriveInfo(driveName);
    }

    public override void Update()
    {
        if (_drive.IsReady)
        {
            try
            {
                DisplayText = "reading error";
                long total = _drive.TotalSize; // all data here in bytes
                long free = _drive.AvailableFreeSpace;
                long used = total - free;
                double usagePercent = (double)used / total * 100;

                CurrentValue = usagePercent;

                double usedGB = used / 1024.0 / 1024.0 / 1024.0;
                double totalGB = total / 1024.0 / 1024.0 / 1024.0;

                DisplayText = $"{usedGB:F1} GB / {totalGB:F0} GB";
            }
            catch
            {
                DisplayText = "Disk error";
                CurrentValue = 0;
            }
        }
        else
        {
            DisplayText = "Disk not ready";
            CurrentValue = 0;
        }
    }
}