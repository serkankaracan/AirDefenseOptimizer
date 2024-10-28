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

            try
            {
                _radarService = new RadarService(App.ConnectionManager!, App.DatabaseHelper!);
                LoadRadarData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing the Radar window: {ex.Message}");
            }
        }

        // Radar verilerini yükle
        private void LoadRadarData()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading radar data: {ex.Message}");
            }
        }

        // Arama yap
        private void SearchRadar_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while searching for radar data: {ex.Message}");
            }
        }

        private void SearchRadar_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    SearchRadar_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during search: {ex.Message}");
            }
        }

        // Yeni radar ekle
        private void AddNewRadar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Radar eklemek için RadarEditWindow açılır
                RadarEditWindow radarEditWindow = new RadarEditWindow(_radarService, null);  // Yeni radar eklerken boş veri ile gönderiyoruz
                radarEditWindow.ShowDialog();

                LoadRadarData();  // Verileri yenile
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding a new radar: {ex.Message}");
            }
        }

        // Radar bilgilerini önizle
        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedRadar = ((Button)sender).DataContext as dynamic;

                // Radar önizlemesi için RadarEditWindow açılır, veriler sadece okunur modda gösterilir
                RadarEditWindow radarEditWindow = new RadarEditWindow(_radarService, selectedRadar, isReadOnly: true);
                radarEditWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while previewing the radar: {ex.Message}");
            }
        }

        // Radar bilgilerini düzenle
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedRadar = ((Button)sender).DataContext as dynamic;

                // Radar düzenlemek için RadarEditWindow açılır
                RadarEditWindow radarEditWindow = new RadarEditWindow(_radarService, selectedRadar);
                radarEditWindow.ShowDialog();

                LoadRadarData();  // Verileri yenile
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while editing the radar: {ex.Message}");
            }
        }

        // Radar bilgilerini sil
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedRadar = ((Button)sender).DataContext as dynamic;
                MessageBoxResult result = MessageBox.Show($"Radar {selectedRadar.Name}'ı silmek istediğinizden emin misiniz?",
                                                          "Silme Onayı",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _radarService.DeleteRadar((int)selectedRadar.Id);
                    MessageBox.Show("Radar başarıyla silindi.");
                    LoadRadarData();  // Verileri yenile
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the radar: {ex.Message}");
            }
        }
    }
}
