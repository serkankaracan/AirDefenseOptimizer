using AirDefenseOptimizer.Services;
using System.Windows;
using System.Windows.Controls;

namespace AirDefenseOptimizer.Views
{
    public partial class AirDefenseWindow : UserControl
    {
        private readonly AirDefenseService _airDefenseService;
        private readonly RadarService _radarService;
        private readonly MunitionService _munitionService;

        public AirDefenseWindow()
        {
            InitializeComponent();
            _airDefenseService = new AirDefenseService(App.ConnectionManager!, App.DatabaseHelper!);
            _radarService = new RadarService(App.ConnectionManager!, App.DatabaseHelper!);
            _munitionService = new MunitionService(App.ConnectionManager!, App.DatabaseHelper!);

            LoadAirDefenseData();
        }

        // Air Defense verilerini yükle
        private void LoadAirDefenseData()
        {
            var airDefenses = _airDefenseService.GetAllAirDefenseSystems();
            var allRadars = _radarService.GetAllRadars(); // Tüm radarları çek
            var allMunitions = _munitionService.GetAllMunitions(); // Tüm mühimmatları çek

            if (airDefenses.Count <= 0)
                return;

            AirDefenseDataGrid.ItemsSource = airDefenses.Select(airDefense =>
            {
                // Radar ve mühimmatları ayrı olarak çek
                var radars = _airDefenseService.GetAirDefenseRadars(Convert.ToInt32(airDefense["Id"]));
                //var radarNames = radars.Select(r => allRadars.FirstOrDefault(rad => rad["Id"].ToString() == r["RadarId"].ToString())?["Name"]?.ToString()).ToList();
                var radarDetails = radars.Select(r => $"{allRadars.FirstOrDefault(rad => rad["Id"].ToString() == r["RadarId"].ToString())?["Name"]?.ToString()} (Quantity: {r["Quantity"]})").ToList();

                var munitions = _airDefenseService.GetAirDefenseMunitions(Convert.ToInt32(airDefense["Id"]));
                var munitionDetails = munitions.Select(m => $"{allMunitions.FirstOrDefault(mun => mun["Id"].ToString() == m["MunitionId"].ToString())?["Name"]?.ToString()} (Quantity: {m["Quantity"]})").ToList();

                return new
                {
                    Id = airDefense["Id"],
                    Name = airDefense["Name"],
                    AerodynamicTargetRangeMax = airDefense.ContainsKey("AerodynamicTargetRangeMax") ? airDefense["AerodynamicTargetRangeMax"] : null,
                    AerodynamicTargetRangeMin = airDefense.ContainsKey("AerodynamicTargetRangeMin") ? airDefense["AerodynamicTargetRangeMin"] : null,
                    BallisticTargetRangeMax = airDefense.ContainsKey("BallisticTargetRangeMax") ? airDefense["BallisticTargetRangeMax"] : null,
                    BallisticTargetRangeMin = airDefense.ContainsKey("BallisticTargetRangeMin") ? airDefense["BallisticTargetRangeMin"] : null,
                    MaxEngagements = airDefense.ContainsKey("MaxEngagements") ? airDefense["MaxEngagements"] : null,
                    MaxMissilesFired = airDefense.ContainsKey("MaxMissilesFired") ? airDefense["MaxMissilesFired"] : null,
                    ECMCapability = airDefense["ECMCapability"],
                    Cost = airDefense["Cost"],
                    Radars = string.Join(", ", radarDetails), // Radar isimlerini birleştirip göster
                    Munitions = string.Join(", ", munitionDetails) // Mühimmat detaylarını birleştirip göster
                };
            }).ToList();
        }

        // Yeni Air Defense ekle
        private void AddNewAirDefense_Click(object sender, RoutedEventArgs e)
        {
            var airDefenseEditWindow = new AirDefenseEditWindow(_airDefenseService, _radarService, _munitionService, null);
            airDefenseEditWindow.ShowDialog();
            LoadAirDefenseData();
        }

        // Air Defense bilgilerini düzenle
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAirDefense = ((Button)sender).DataContext as dynamic;
            var airDefenseEditWindow = new AirDefenseEditWindow(_airDefenseService, _radarService, _munitionService, selectedAirDefense);
            airDefenseEditWindow.ShowDialog();
            LoadAirDefenseData();
        }

        // Air Defense bilgilerini önizle
        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAirDefense = ((Button)sender).DataContext as dynamic;
            var airDefenseEditWindow = new AirDefenseEditWindow(_airDefenseService, _radarService, _munitionService, selectedAirDefense, isReadOnly: true);
            airDefenseEditWindow.ShowDialog();
        }

        // Air Defense bilgilerini sil
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAirDefense = ((Button)sender).DataContext as dynamic;
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete Air Defense {selectedAirDefense.Name}?",
                                                      "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _airDefenseService.DeleteAirDefense((int)selectedAirDefense.Id);
                LoadAirDefenseData();
            }
        }

        // Arama işlemi
        private void SearchAirDefense_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();
            var filteredAirDefenses = _airDefenseService.GetAllAirDefenseSystems()
                .Where(airDefense => airDefense["Name"].ToString()!.ToLower().Contains(searchTerm))
                .Select(airDefense => new
                {
                    Id = airDefense["Id"],
                    Name = airDefense["Name"],
                    AerodynamicTargetRangeMax = airDefense["AerodynamicTargetRangeMax"],
                    BallisticTargetRangeMax = airDefense["BallisticTargetRangeMax"],
                    ECMCapability = airDefense["ECMCapability"],
                    Cost = airDefense["Cost"]
                }).ToList();

            AirDefenseDataGrid.ItemsSource = filteredAirDefenses;
        }

        private void SearchAirDefense_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SearchAirDefense_Click(sender, e);
            }
        }
    }
}
