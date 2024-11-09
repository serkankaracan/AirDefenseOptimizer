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
        public string? ThreatLevel { get; set; }
        public double? ThreatScore { get; set; }
    }
}
