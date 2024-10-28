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
            try
            {
                _aircraftService = new AircraftService(App.ConnectionManager!, App.DatabaseHelper!);
                _radarService = new RadarService(App.ConnectionManager!, App.DatabaseHelper!);
                _munitionService = new MunitionService(App.ConnectionManager!, App.DatabaseHelper!);

                LoadAircraftData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization error: {ex.Message}");
            }
        }

        // Uçak verilerini yükle
        private void LoadAircraftData()
        {
            try
            {
                // Önce DataGrid'i temizle
                AircraftDataGrid.ItemsSource = null;

                // Verileri yükle
                var aircrafts = _aircraftService.GetAllAircrafts();
                var radars = _radarService.GetAllRadars(); // Tüm radarları çek

                if (aircrafts.Count <= 0)
                    return;

                AircraftDataGrid.ItemsSource = aircrafts.Select(aircraft =>
                {
                    var radar = _aircraftService.GetAircraftRadar(Convert.ToInt32(aircraft["Id"]));
                    string radarName = string.Empty;

                    if (radar != null && radar.ContainsKey("RadarName"))
                    {
                        radarName = radar["RadarName"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Radar not found.");
                    }

                    // Uçağa ait mühimmatları çek
                    var munitions = _aircraftService.GetAircraftMunitions(Convert.ToInt32(aircraft["Id"]));
                    string munitionsDetails = string.Join(Environment.NewLine, munitions.Select(m => $"{m["MunitionName"]}: {m["Quantity"]}"));

                    return new
                    {
                        Id = aircraft["Id"],
                        Name = aircraft["Name"],
                        AircraftType = aircraft["AircraftType"],
                        Speed = aircraft["Speed"],
                        Range = aircraft["Range"],
                        MaxAltitude = aircraft["MaxAltitude"],
                        Maneuverability = aircraft["Maneuverability"],
                        PayloadCapacity = aircraft["PayloadCapacity"],
                        RadarCrossSection = aircraft["RadarCrossSection"],
                        ECMCapability = aircraft["ECMCapability"],
                        Cost = aircraft["Cost"],
                        RadarId = aircraft["RadarId"],
                        RadarName = radarName,
                        Munitions = munitionsDetails
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading aircraft data: {ex.Message}");
            }
        }

        // Yeni uçak ekle
        private void AddNewAircraft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AircraftEditWindow aircraftEditWindow = new AircraftEditWindow(_aircraftService, _munitionService, _radarService, null);
                aircraftEditWindow.ShowDialog();
                LoadAircraftData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding new aircraft: {ex.Message}");
            }
        }

        // Uçak bilgilerini düzenle
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedAircraft = ((Button)sender).DataContext as dynamic;
                AircraftEditWindow aircraftEditWindow = new AircraftEditWindow(_aircraftService, _munitionService, _radarService, selectedAircraft);
                aircraftEditWindow.ShowDialog();
                LoadAircraftData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing aircraft: {ex.Message}");
            }
        }

        // Uçak bilgilerini önizle
        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedAircraft = ((Button)sender).DataContext as dynamic;

                AircraftEditWindow aircraftEditWindow = new AircraftEditWindow(_aircraftService, _munitionService, _radarService, selectedAircraft, isReadOnly: true);
                aircraftEditWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error previewing aircraft: {ex.Message}");
            }
        }

        // Uçak bilgilerini sil
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedAircraft = ((Button)sender).DataContext as dynamic;
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete aircraft {selectedAircraft.Name}?",
                                                          "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _aircraftService.DeleteAircraft((int)selectedAircraft.Id);
                    LoadAircraftData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting aircraft: {ex.Message}");
            }
        }

        // Arama yap
        private void SearchAircraft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.ToLower();
                var filteredAircrafts = _aircraftService.GetAllAircrafts()
                    .Where(aircraft => aircraft["Name"].ToString()!.ToLower().Contains(searchTerm) ||
                                       aircraft["AircraftType"].ToString()!.ToLower().Contains(searchTerm))
                    .Select(aircraft =>
                    {
                        var radar = _aircraftService.GetAircraftRadar(Convert.ToInt32(aircraft["Id"]));
                        string radarName = string.Empty;

                        if (radar != null && radar.ContainsKey("RadarName"))
                        {
                            radarName = radar["RadarName"].ToString();
                        }

                        var munitions = _aircraftService.GetAircraftMunitions(Convert.ToInt32(aircraft["Id"]));
                        string munitionsDetails = string.Join(Environment.NewLine, munitions.Select(m => $"{m["MunitionName"]}: {m["Quantity"]}"));

                        return new
                        {
                            Id = aircraft["Id"],
                            Name = aircraft["Name"],
                            AircraftType = aircraft["AircraftType"],
                            Speed = aircraft["Speed"],
                            Range = aircraft["Range"],
                            MaxAltitude = aircraft["MaxAltitude"],
                            Maneuverability = aircraft["Maneuverability"],
                            PayloadCapacity = aircraft["PayloadCapacity"],
                            RadarCrossSection = aircraft["RadarCrossSection"],
                            ECMCapability = aircraft["ECMCapability"],
                            Cost = aircraft["Cost"],
                            RadarId = aircraft["RadarId"],
                            RadarName = radarName,
                            Munitions = munitionsDetails
                        };
                    }).ToList();

                AircraftDataGrid.ItemsSource = filteredAircrafts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching aircraft: {ex.Message}");
            }
        }
    }
}
