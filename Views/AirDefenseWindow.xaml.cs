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
            try
            {
                _airDefenseService = new AirDefenseService(App.ConnectionManager!, App.DatabaseHelper!);
                _radarService = new RadarService(App.ConnectionManager!, App.DatabaseHelper!);
                _munitionService = new MunitionService(App.ConnectionManager!, App.DatabaseHelper!);
                LoadAirDefenseData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization error: {ex.Message}");
            }
        }

        private void LoadAirDefenseData()
        {
            try
            {
                var airDefenses = _airDefenseService.GetAllAirDefenseSystems();
                var allRadars = _radarService.GetAllRadars();
                var allMunitions = _munitionService.GetAllMunitions();

                if (airDefenses.Count <= 0)
                    return;

                AirDefenseDataGrid.ItemsSource = airDefenses.Select(airDefense =>
                {
                    var radars = _airDefenseService.GetAirDefenseRadars(Convert.ToInt32(airDefense["Id"]));
                    var radarDetails = radars.Select(r => $"{allRadars.FirstOrDefault(rad => rad["Id"].ToString() == r["RadarId"].ToString())?["Name"]?.ToString()}: {r["Quantity"]}").ToList();

                    var munitions = _airDefenseService.GetAirDefenseMunitions(Convert.ToInt32(airDefense["Id"]));
                    var munitionDetails = munitions.Select(m => $"{allMunitions.FirstOrDefault(mun => mun["Id"].ToString() == m["MunitionId"].ToString())?["Name"]?.ToString()}: {m["Quantity"]}").ToList();

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
                        Radars = string.Join(Environment.NewLine, radarDetails),
                        Munitions = string.Join(Environment.NewLine, munitionDetails)
                    };
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Air Defense data: {ex.Message}");
            }
        }

        private void AddNewAirDefense_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var airDefenseEditWindow = new AirDefenseEditWindow(_airDefenseService, _radarService, _munitionService, null);
                airDefenseEditWindow.ShowDialog();
                LoadAirDefenseData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding new Air Defense: {ex.Message}");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedAirDefense = ((Button)sender).DataContext as dynamic;
                var airDefenseEditWindow = new AirDefenseEditWindow(_airDefenseService, _radarService, _munitionService, selectedAirDefense);
                airDefenseEditWindow.ShowDialog();
                LoadAirDefenseData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing Air Defense: {ex.Message}");
            }
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedAirDefense = ((Button)sender).DataContext as dynamic;
                var airDefenseEditWindow = new AirDefenseEditWindow(_airDefenseService, _radarService, _munitionService, selectedAirDefense, isReadOnly: true);
                airDefenseEditWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error previewing Air Defense: {ex.Message}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting Air Defense: {ex.Message}");
            }
        }

        private void SearchAirDefense_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchTerm = txtSearch.Text.ToLower();
                var allRadars = _radarService.GetAllRadars();
                var allMunitions = _munitionService.GetAllMunitions();

                var filteredAirDefenses = _airDefenseService.GetAllAirDefenseSystems()
                    .Where(airDefense => airDefense["Name"].ToString()!.ToLower().Contains(searchTerm))
                    .Select(airDefense =>
                    {
                        var radars = _airDefenseService.GetAirDefenseRadars(Convert.ToInt32(airDefense["Id"]));
                        var radarDetails = radars.Select(r => $"{allRadars.FirstOrDefault(rad => rad["Id"].ToString() == r["RadarId"].ToString())?["Name"]?.ToString()}: {r["Quantity"]}").ToList();

                        var munitions = _airDefenseService.GetAirDefenseMunitions(Convert.ToInt32(airDefense["Id"]));
                        var munitionDetails = munitions.Select(m => $"{allMunitions.FirstOrDefault(mun => mun["Id"].ToString() == m["MunitionId"].ToString())?["Name"]?.ToString()}: {m["Quantity"]}").ToList();

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
                            Radars = string.Join(Environment.NewLine, radarDetails),
                            Munitions = string.Join(Environment.NewLine, munitionDetails)
                        };
                    }).ToList();

                AirDefenseDataGrid.ItemsSource = filteredAirDefenses;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching Air Defense: {ex.Message}");
            }
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
