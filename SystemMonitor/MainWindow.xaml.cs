using System.Collections.ObjectModel; 
using System.IO; 
using System.Threading.Tasks; 
using System.Windows;
using SystemMonitor.Components; 
using SystemMonitor.Core; 

namespace SystemMonitor
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ISensor> Sensors { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            
            Sensors = new ObservableCollection<ISensor>();
            Sensors.Add(new CpuSensor());
            Sensors.Add(new RamSensor());

            //hard drivesensors
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Fixed && drive.IsReady)
                {
                    Sensors.Add(new DiskSensor(drive.Name));
                }
            }
            //TODO
            // Sensors.Add(new NetworkSensor());
            // Sensors.Add(new BatterySensor());
            // and maybe ping sensor for internet connection
            
            DataContext = this;
            
            StartMonitoring();
        }

        private void StartMonitoring()
        {
            
            Task.Run(async () =>
            {
                while (true)
                {
                    foreach (var sensor in Sensors)
                    {
                        sensor.Update();
                    }
                    //refreshing every second
                    await Task.Delay(1000);
                }
            });
        }
    }
}