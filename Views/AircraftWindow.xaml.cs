using AirDefenseOptimizer.Services;
using System.Windows;
using System.Windows.Controls;

namespace AirDefenseOptimizer.Views
{
    public partial class AircraftWindow : UserControl
    {
        private readonly AircraftService _aircraftService;
        private readonly RadarService _radarService;
        private readonly MunitionService _munitionService;

        public AircraftWindow()
        {
            InitializeComponent();
            _aircraftService = new AircraftService(App.ConnectionManager!, App.DatabaseHelper!);
            _radarService = new RadarService(App.ConnectionManager!, App.DatabaseHelper!);
            _munitionService = new MunitionService(App.ConnectionManager!, App.DatabaseHelper!);

            LoadAircraftData();
        }

        // Uçak verilerini yükle
        private void LoadAircraftData()
        {
            var aircrafts = _aircraftService.GetAllAircrafts();

            if (aircrafts.Count <= 0)
                return;

            AircraftDataGrid.ItemsSource = aircrafts.Select(aircraft => new
            {
                Id = aircraft["Id"],
                Name = aircraft["Name"],
                AircraftType = aircraft["AircraftType"],
                Speed = aircraft["Speed"],
                Range = aircraft["Range"],
                MaxAltitude = aircraft["MaxAltitude"],
                PayloadCapacity = aircraft["PayloadCapacity"],
                Cost = aircraft["Cost"],
                RadarId = aircraft["RadarId"] != DBNull.Value ? aircraft["RadarId"] : null
            }).ToList();
        }

        // Yeni uçak ekle
        private void AddNewAircraft_Click(object sender, RoutedEventArgs e)
        {
            AircraftEditWindow aircraftEditWindow = new AircraftEditWindow(_aircraftService, _munitionService, _radarService, null);
            aircraftEditWindow.ShowDialog();
            LoadAircraftData();  // Verileri yenile
        }

        // Uçak bilgilerini düzenle
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAircraft = ((Button)sender).DataContext as dynamic;
            AircraftEditWindow aircraftEditWindow = new AircraftEditWindow(_aircraftService, _munitionService, _radarService, selectedAircraft);
            aircraftEditWindow.ShowDialog();
            LoadAircraftData();  // Verileri yenile
        }

        // Uçak bilgilerini önizle
        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAircraft = ((Button)sender).DataContext as dynamic;

            // Uçak önizlemesi için AircraftEditWindow açılır, veriler sadece okunur modda gösterilir
            AircraftEditWindow aircraftEditWindow = new AircraftEditWindow(_aircraftService, _munitionService, _radarService, selectedAircraft, isReadOnly: true);
            aircraftEditWindow.ShowDialog();
        }

        // Uçak bilgilerini sil
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAircraft = ((Button)sender).DataContext as dynamic;
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete aircraft {selectedAircraft.Name}?",
                                                      "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                _aircraftService.DeleteAircraft((int)selectedAircraft.Id);
                LoadAircraftData();  // Verileri yenile
            }
        }

        // Arama yap
        private void SearchAircraft_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();
            var filteredAircrafts = _aircraftService.GetAllAircrafts()
                .Where(aircraft => aircraft["Name"].ToString()!.ToLower().Contains(searchTerm) ||
                                   aircraft["AircraftType"].ToString()!.ToLower().Contains(searchTerm))
                .Select(aircraft => new
                {
                    Id = aircraft["Id"],
                    Name = aircraft["Name"],
                    AircraftType = aircraft["AircraftType"],
                    Speed = aircraft["Speed"],
                    Range = aircraft["Range"],
                    MaxAltitude = aircraft["MaxAltitude"],
                    PayloadCapacity = aircraft["PayloadCapacity"],
                    Cost = aircraft["Cost"],
                    RadarId = aircraft["RadarId"] != DBNull.Value ? aircraft["RadarId"] : null
                }).ToList();

            AircraftDataGrid.ItemsSource = filteredAircrafts;
        }
    }
}
