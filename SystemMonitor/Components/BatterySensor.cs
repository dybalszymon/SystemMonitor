using System.Management;
using SystemMonitor.Core;

namespace SystemMonitor.Components;

public class BatterySensor : BaseSensor
{
    public BatterySensor() : base("Battery")
    {
    }

    public override void Update()
    {
        try
        {
            var searcher = new ManagementObjectSearcher("SELECT EstimatedChargeRemaining FROM Win32_Battery");
            foreach (ManagementObject obj in searcher.Get())
            {
                ushort charge = (ushort)obj["EstimatedChargeRemaining"];
                CurrentValue = charge;
                DisplayText = $"{charge}%";
                return;
            }
            
            DisplayText = "No Battery";
            CurrentValue = 0;
        }
        catch
        {
            DisplayText = "Error";
            CurrentValue = 0;
        }
    }
}