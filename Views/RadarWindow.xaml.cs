using AirDefenseOptimizer.Services;
using System.Windows;
using System.Windows.Controls;

namespace AirDefenseOptimizer.Views
{
    public partial class RadarWindow : UserControl
    {
        private readonly RadarService _radarService;

        public RadarWindow()
        {
            InitializeComponent();

            _radarService = new RadarService(App.ConnectionManager!, App.DatabaseHelper!);

            LoadRadarData();
        }

        // Radar verilerini yükle
        private void LoadRadarData()
        {
            List<Dictionary<string, object>> radars = _radarService.GetAllRadars();

            if (radars.Count <= 0)
                return;

            RadarDataGrid.ItemsSource = radars.Select(radar => new
            {
                Id = radar["Id"],
                Name = radar["Name"],
                RadarType = radar["RadarType"],
                MinDetectionRange = radar["MinDetectionRange"],
                MaxDetectionRange = radar["MaxDetectionRange"],
                MinAltitude = radar["MinAltitude"],
                MaxAltitude = radar["MaxAltitude"],
                MaxTargetSpeed = radar["MaxTargetSpeed"],
                MaxTargetVelocity = radar["MaxTargetVelocity"],
                RedeploymentTime = radar["RedeploymentTime"]
            }).ToList();
        }

        // Arama yap
        private void SearchRadar_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();
            var filteredRadars = _radarService.GetAllRadars()
                .Where(radar => radar["Name"].ToString()!.ToLower().Contains(searchTerm) ||
                                radar["RadarType"].ToString()!.ToLower().Contains(searchTerm))
                .Select(radar => new
                {
                    Id = radar["Id"],
                    Name = radar["Name"],
                    RadarType = radar["RadarType"],
                    MinDetectionRange = radar["MinDetectionRange"],
                    MaxDetectionRange = radar["MaxDetectionRange"],
                    MinAltitude = radar["MinAltitude"],
                    MaxAltitude = radar["MaxAltitude"],
                    MaxTargetSpeed = radar["MaxTargetSpeed"],
                    MaxTargetVelocity = radar["MaxTargetVelocity"],
                    RedeploymentTime = radar["RedeploymentTime"]
                }).ToList();

            RadarDataGrid.ItemsSource = filteredRadars;
        }

        private void SearchRadar_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SearchRadar_Click(sender, e);
            }
        }

        // Yeni radar ekle
        private void AddNewRadar_Click(object sender, RoutedEventArgs e)
        {
            // Radar eklemek için RadarEditWindow açılır
            RadarEditWindow radarEditWindow = new RadarEditWindow(_radarService, null);  // Yeni radar eklerken boş veri ile gönderiyoruz
            radarEditWindow.ShowDialog();

            LoadRadarData();  // Verileri yenile
        }

        // Radar bilgilerini önizle
        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRadar = ((Button)sender).DataContext as dynamic;

            // Radar önizlemesi için RadarEditWindow açılır, veriler sadece okunur modda gösterilir
            RadarEditWindow radarEditWindow = new RadarEditWindow(_radarService, selectedRadar, isReadOnly: true);
            radarEditWindow.ShowDialog();
        }

        // Radar bilgilerini düzenle
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRadar = ((Button)sender).DataContext as dynamic;

            // Radar düzenlemek için RadarEditWindow açılır
            RadarEditWindow radarEditWindow = new RadarEditWindow(_radarService, selectedRadar);
            radarEditWindow.ShowDialog();

            LoadRadarData();  // Verileri yenile
        }

        // Radar bilgilerini sil
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRadar = ((Button)sender).DataContext as dynamic;
            MessageBoxResult result = MessageBox.Show($"Radar {selectedRadar.Name}'ı silmek istediğinizden emin misiniz?",
                                                      "Silme Onayı",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _radarService.DeleteRadar((int)selectedRadar.Id);
                    MessageBox.Show("Radar başarıyla silindi.");
                    LoadRadarData();  // Verileri yenile
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hata oluştu: {ex.Message}");
                }
            }
        }
    }
}
