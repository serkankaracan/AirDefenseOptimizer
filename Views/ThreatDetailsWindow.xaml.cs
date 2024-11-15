using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Models;
using System.Windows;

namespace AirDefenseOptimizer.Views
{
    /// <summary>
    /// Interaction logic for ThreatDetailsWindow.xaml
    /// </summary>
    public partial class ThreatDetailsWindow : Window
    {
        public ThreatDetailsWindow(List<ThreatDetail> threatDetails)
        {
            InitializeComponent();
            ThreatDataGrid.ItemsSource = threatDetails;
        }
    }

    public class ThreatDetail
    {
        public int Index { get; set; }
        public Aircraft? Aircraft { get; set; }
        public IFF? IFFMode { get; set; }
        public double Speed { get; set; }
        public string? Location { get; set; }
        public double Distance { get; set; }
        public double Altitude { get; set; }
        public string? ThreatLevel { get; set; }
        public double? ThreatScore { get; set; }
        public Radar[]? DetectedByRadar { get; set; }
        public string DetectedByRadarNames => DetectedByRadar != null && DetectedByRadar.Length > 0 ? string.Join(", \n", DetectedByRadar.Select(radar => radar.Name + " " + "(" + radar.MaxDetectionRange + ")")) : "None";
        public AirDefense? AssignedADS { get; set; }
    }
}
