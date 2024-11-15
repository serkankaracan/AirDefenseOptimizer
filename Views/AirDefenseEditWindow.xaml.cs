using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Services;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace AirDefenseOptimizer.Views
{
    public partial class AirDefenseEditWindow : Window
    {
        private readonly AirDefenseService _airDefenseService;
        private readonly RadarService _radarService;
        private readonly MunitionService _munitionService;
        private readonly dynamic? _airDefenseData;
        private readonly bool _isReadOnly;

        private readonly List<ComboBox> _radarComboBoxes = new List<ComboBox>();
        private readonly List<TextBox> _radarQuantityTextBoxes = new List<TextBox>();
        private readonly List<ComboBox> _munitionComboBoxes = new List<ComboBox>();
        private readonly List<TextBox> _munitionQuantityTextBoxes = new List<TextBox>();

        public AirDefenseEditWindow(AirDefenseService airDefenseService, RadarService radarService, MunitionService munitionService, dynamic? airDefenseData = null, bool isReadOnly = false)
        {
            InitializeComponent();
            _airDefenseService = airDefenseService;
            _radarService = radarService;
            _munitionService = munitionService;
            _airDefenseData = airDefenseData;
            _isReadOnly = isReadOnly;

            cbECMCapability.ItemsSource = Enum.GetValues(typeof(ECMCapability))
               .Cast<ECMCapability>()
               .Select(a => new KeyValuePair<ECMCapability, string>(a, a.GetECMCapabilityName()))
               .ToList();

            try
            {
                LoadRadarList();
                LoadMunitionList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }

            if (_airDefenseData != null)
            {
                // Air Defense bilgilerini doldur
                txtAirDefenseName.Text = _airDefenseData.Name;
                txtAerodynamicRangeMax.Text = _airDefenseData.AerodynamicTargetRangeMax.ToString();
                txtAerodynamicRangeMin.Text = _airDefenseData.AerodynamicTargetRangeMin.ToString();
                txtBallisticRangeMax.Text = _airDefenseData.BallisticTargetRangeMax.ToString();
                txtBallisticRangeMin.Text = _airDefenseData.BallisticTargetRangeMin.ToString();
                txtMaxEngagements.Text = _airDefenseData.MaxEngagements.ToString();
                txtMaxMissilesFired.Text = _airDefenseData.MaxMissilesFired.ToString();
                cbECMCapability.SelectedValue = Enum.Parse<ECMCapability>(_airDefenseData.ECMCapability);
                txtCost.Text = _airDefenseData.Cost.ToString();

                try
                {
                    // Radarlar ekle
                    var radars = _airDefenseService.GetAirDefenseRadars((int)_airDefenseData.Id);
                    foreach (var radar in radars)
                    {
                        AddRadarRow(radar["RadarId"].ToString(), Convert.ToInt32(radar["Quantity"]));
                    }

                    // Mühimmatları ekle
                    var munitions = _airDefenseService.GetAirDefenseMunitions((int)_airDefenseData.Id);
                    foreach (var munition in munitions)
                    {
                        AddMunitionRow(munition["MunitionId"].ToString(), Convert.ToInt32(munition["Quantity"]));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading radar and munition data: {ex.Message}");
                }
            }

            if (_isReadOnly)
            {
                // Alanları salt okunur yap
                txtAirDefenseName.IsEnabled = false;
                txtAerodynamicRangeMax.IsEnabled = false;
                txtAerodynamicRangeMin.IsEnabled = false;
                txtBallisticRangeMax.IsEnabled = false;
                txtBallisticRangeMin.IsEnabled = false;
                txtMaxEngagements.IsEnabled = false;
                txtMaxMissilesFired.IsEnabled = false;
                cbECMCapability.IsEnabled = false;
                txtCost.IsEnabled = false;
                btnAddRadar.IsEnabled = false;
                btnAddMunition.IsEnabled = false;
                btnSave.IsEnabled = false;
            }
        }

        private void LoadRadarList()
        {
            try
            {
                var radars = _radarService.GetAllRadars();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading radar list: {ex.Message}");
            }
        }

        private void LoadMunitionList()
        {
            try
            {
                var munitions = _munitionService.GetAllMunitions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading munition list: {ex.Message}");
            }
        }

        private void AddRadar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddRadarRow(null, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding radar row: {ex.Message}");
            }
        }

        private void AddRadarRow(string? selectedRadarId, int quantity)
        {
            ComboBox radarComboBox = new ComboBox
            {
                Width = 150,
                Margin = new Thickness(0, 5, 0, 5)
            };

            try
            {
                var radars = _radarService.GetAllRadars();
                radarComboBox.ItemsSource = radars.Select(r => new KeyValuePair<long, string>(Convert.ToInt64(r["Id"]), r["Name"]?.ToString() ?? "Unnamed Radar")).ToList();
                radarComboBox.DisplayMemberPath = "Value";
                radarComboBox.SelectedValuePath = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading radar data: {ex.Message}");
            }

            if (!string.IsNullOrEmpty(selectedRadarId))
            {
                radarComboBox.SelectedValue = selectedRadarId;
            }

            TextBox quantityTextBox = new TextBox
            {
                Width = 50,
                Text = quantity.ToString(),
                Margin = new Thickness(10, 5, 0, 5)
            };

            Button removeButton = new Button
            {
                Content = "Remove",
                Width = 75,
                Margin = new Thickness(10, 5, 0, 5)
            };

            StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(radarComboBox);
            stackPanel.Children.Add(quantityTextBox);
            stackPanel.Children.Add(removeButton);

            RadarList.Children.Add(stackPanel);
            _radarComboBoxes.Add(radarComboBox);
            _radarQuantityTextBoxes.Add(quantityTextBox);

            removeButton.Click += (s, ev) =>
            {
                RadarList.Children.Remove(stackPanel);
                _radarComboBoxes.Remove(radarComboBox);
                _radarQuantityTextBoxes.Remove(quantityTextBox);
            };
        }

        private void AddMunition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AddMunitionRow(null, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding munition row: {ex.Message}");
            }
        }

        private void AddMunitionRow(string? selectedMunitionId, int quantity)
        {
            ComboBox munitionComboBox = new ComboBox
            {
                Width = 150,
                Margin = new Thickness(0, 5, 0, 5)
            };

            try
            {
                var munitions = _munitionService.GetAllMunitions();
                munitionComboBox.ItemsSource = munitions.Select(m => new KeyValuePair<long, string>(Convert.ToInt64(m["Id"]), m["Name"]?.ToString() ?? "Unnamed Munition")).ToList();
                munitionComboBox.DisplayMemberPath = "Value";
                munitionComboBox.SelectedValuePath = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading munition data: {ex.Message}");
            }

            if (!string.IsNullOrEmpty(selectedMunitionId))
            {
                munitionComboBox.SelectedValue = selectedMunitionId;
            }

            TextBox quantityTextBox = new TextBox
            {
                Width = 50,
                Text = quantity.ToString(),
                Margin = new Thickness(10, 5, 0, 5)
            };

            Button removeButton = new Button
            {
                Content = "Remove",
                Width = 75,
                Margin = new Thickness(10, 5, 0, 5)
            };

            StackPanel stackPanel = new StackPanel { Orientation = Orientation.Horizontal };
            stackPanel.Children.Add(munitionComboBox);
            stackPanel.Children.Add(quantityTextBox);
            stackPanel.Children.Add(removeButton);

            MunitionList.Children.Add(stackPanel);
            _munitionComboBoxes.Add(munitionComboBox);
            _munitionQuantityTextBoxes.Add(quantityTextBox);

            removeButton.Click += (s, ev) =>
            {
                MunitionList.Children.Remove(stackPanel);
                _munitionComboBoxes.Remove(munitionComboBox);
                _munitionQuantityTextBoxes.Remove(quantityTextBox);
            };
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int airDefenseId;

                if (_airDefenseData == null)
                {
                    airDefenseId = _airDefenseService.AddAirDefense(
                        txtAirDefenseName.Text,
                        double.TryParse(txtAerodynamicRangeMax.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double aerodynamicRangeMax) ? aerodynamicRangeMax : 0,
                        double.TryParse(txtAerodynamicRangeMin.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double aerodynamicRangeMin) ? aerodynamicRangeMin : 0,
                        double.TryParse(txtBallisticRangeMax.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double ballisticRangeMax) ? ballisticRangeMax : 0,
                        double.TryParse(txtBallisticRangeMin.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double ballisticRangeMin) ? ballisticRangeMin : 0,
                        int.Parse(txtMaxEngagements.Text),
                        int.Parse(txtMaxMissilesFired.Text),
                        ((ECMCapability)cbECMCapability.SelectedValue).ToString(),
                        double.Parse(txtCost.Text));
                }
                else
                {
                    airDefenseId = (int)_airDefenseData.Id;
                    _airDefenseService.UpdateAirDefense(
                        airDefenseId,
                        txtAirDefenseName.Text,
                        double.TryParse(txtAerodynamicRangeMax.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double aerodynamicRangeMax) ? aerodynamicRangeMax : 0,
                        double.TryParse(txtAerodynamicRangeMin.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double aerodynamicRangeMin) ? aerodynamicRangeMin : 0,
                        double.TryParse(txtBallisticRangeMax.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double ballisticRangeMax) ? ballisticRangeMax : 0,
                        double.TryParse(txtBallisticRangeMin.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double ballisticRangeMin) ? ballisticRangeMin : 0,
                        int.Parse(txtMaxEngagements.Text),
                        int.Parse(txtMaxMissilesFired.Text),
                        ((ECMCapability)cbECMCapability.SelectedValue).ToString(),
                        double.Parse(txtCost.Text));
                }

                SaveRadars(airDefenseId);
                SaveMunitions(airDefenseId);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving Air Defense: {ex.Message}");
            }
        }

        private void SaveRadars(int airDefenseId)
        {
            try
            {
                var existingRadars = _airDefenseService.GetAirDefenseRadars(airDefenseId);

                foreach (var existingRadar in existingRadars)
                {
                    var radarId = Convert.ToInt64(existingRadar["RadarId"]);
                    var selectedRadarComboBox = _radarComboBoxes.FirstOrDefault(cb => Convert.ToInt64(cb.SelectedValue) == radarId);

                    if (selectedRadarComboBox == null)
                    {
                        _airDefenseService.DeleteAirDefenseRadar(airDefenseId, (int)radarId);
                    }
                    else
                    {
                        int index = _radarComboBoxes.IndexOf(selectedRadarComboBox);
                        int quantity = int.Parse(_radarQuantityTextBoxes[index].Text);
                        _airDefenseService.UpdateAirDefenseRadar(airDefenseId, (int)radarId, quantity);
                    }
                }

                foreach (var radarComboBox in _radarComboBoxes)
                {
                    var selectedRadarId = Convert.ToInt64(radarComboBox.SelectedValue);
                    if (!existingRadars.Any(r => Convert.ToInt64(r["RadarId"]) == selectedRadarId))
                    {
                        int index = _radarComboBoxes.IndexOf(radarComboBox);
                        int quantity = int.Parse(_radarQuantityTextBoxes[index].Text);
                        _airDefenseService.AddRadarToAirDefense(airDefenseId, (int)selectedRadarId, quantity);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving radar data: {ex.Message}");
            }
        }

        private void SaveMunitions(int airDefenseId)
        {
            try
            {
                var existingMunitions = _airDefenseService.GetAirDefenseMunitions(airDefenseId);

                foreach (var existingMunition in existingMunitions)
                {
                    var munitionId = Convert.ToInt64(existingMunition["MunitionId"]);
                    var selectedMunitionComboBox = _munitionComboBoxes.FirstOrDefault(cb => Convert.ToInt64(cb.SelectedValue) == munitionId);

                    if (selectedMunitionComboBox == null)
                    {
                        _airDefenseService.DeleteAirDefenseMunition(airDefenseId, (int)munitionId);
                    }
                    else
                    {
                        int index = _munitionComboBoxes.IndexOf(selectedMunitionComboBox);
                        int quantity = int.Parse(_munitionQuantityTextBoxes[index].Text);
                        _airDefenseService.UpdateAirDefenseMunition(airDefenseId, (int)munitionId, quantity);
                    }
                }

                foreach (var munitionComboBox in _munitionComboBoxes)
                {
                    var selectedMunitionId = Convert.ToInt64(munitionComboBox.SelectedValue);
                    if (!existingMunitions.Any(m => Convert.ToInt64(m["MunitionId"]) == selectedMunitionId))
                    {
                        int index = _munitionComboBoxes.IndexOf(munitionComboBox);
                        int quantity = int.Parse(_munitionQuantityTextBoxes[index].Text);
                        _airDefenseService.AddMunitionToAirDefense(airDefenseId, (int)selectedMunitionId, quantity);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving munition data: {ex.Message}");
            }
        }
    }
}
