using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Services;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace AirDefenseOptimizer.Views
{
    public partial class AircraftEditWindow : Window
    {
        private readonly AircraftService _aircraftService;
        private readonly MunitionService _munitionService;
        private readonly RadarService _radarService;
        private readonly dynamic? _aircraftData;
        private readonly bool _isReadOnly;

        private readonly List<ComboBox> _munitionComboBoxes = new List<ComboBox>();
        private readonly List<TextBox> _munitionQuantityTextBoxes = new List<TextBox>();

        public AircraftEditWindow(AircraftService aircraftService, MunitionService munitionService, RadarService radarService, dynamic? aircraftData = null, bool isReadOnly = false)
        {
            InitializeComponent();
            _aircraftService = aircraftService;
            _radarService = radarService;
            _munitionService = munitionService;
            _aircraftData = aircraftData;
            _isReadOnly = isReadOnly;

            try
            {
                // Enum değerlerini ComboBox'a doldur
                cbAircraftType.ItemsSource = Enum.GetValues(typeof(AircraftType))
                    .Cast<AircraftType>()
                    .Select(a => new KeyValuePair<AircraftType, string>(a, a.GetAircraftTypeName()))
                    .ToList();

                cbManeuverability.ItemsSource = Enum.GetValues(typeof(Maneuverability))
                    .Cast<Maneuverability>()
                    .Select(a => new KeyValuePair<Maneuverability, string>(a, a.GetManeuverabilityName()))
                    .ToList();

                cbECMCapability.ItemsSource = Enum.GetValues(typeof(ECMCapability))
                    .Cast<ECMCapability>()
                    .Select(a => new KeyValuePair<ECMCapability, string>(a, a.GetECMCapabilityName()))
                    .ToList();

                var radars = _radarService.GetAllRadars();
                if (radars == null || radars.Count == 0)
                {
                    MessageBox.Show("No radars found in the database.");
                }
                else
                {
                    cbRadar.ItemsSource = radars.Select(r => new KeyValuePair<long, string>(
                        Convert.ToInt64(r["Id"]),
                        r["Name"]?.ToString() ?? "Unnamed Radar"
                    )).ToList();

                    cbRadar.DisplayMemberPath = "Value";
                    cbRadar.SelectedValuePath = "Key";
                }

                if (_aircraftData != null)
                {
                    // Var olan uçak verilerini doldur
                    txtAircraftName.Text = _aircraftData.Name;
                    cbAircraftType.SelectedValue = Enum.Parse<AircraftType>(_aircraftData.AircraftType.ToString());
                    txtSpeed.Text = _aircraftData.Speed.ToString();
                    txtRange.Text = _aircraftData.Range.ToString();
                    txtMaxAltitude.Text = _aircraftData.MaxAltitude.ToString();
                    cbManeuverability.SelectedValue = Enum.Parse<Maneuverability>(_aircraftData.Maneuverability.ToString());
                    txtPayloadCapacity.Text = _aircraftData.PayloadCapacity.ToString();
                    txtRadarCrossSection.Text = _aircraftData.RadarCrossSection.ToString();
                    cbECMCapability.SelectedValue = Enum.Parse<ECMCapability>(_aircraftData.ECMCapability);
                    cbRadar.SelectedValue = _aircraftData.RadarId;
                    txtCost.Text = _aircraftData.Cost.ToString();

                    LoadAircraftMunitions((int)_aircraftData.Id);
                }

                if (_isReadOnly)
                {
                    txtAircraftName.IsEnabled = false;
                    cbAircraftType.IsEnabled = false;
                    txtSpeed.IsEnabled = false;
                    txtRange.IsEnabled = false;
                    txtMaxAltitude.IsEnabled = false;
                    cbManeuverability.IsEnabled = false;
                    txtPayloadCapacity.IsEnabled = false;
                    txtRadarCrossSection.IsEnabled = false;
                    cbECMCapability.IsEnabled = false;
                    cbRadar.IsEnabled = false;
                    txtCost.IsEnabled = false;
                    btnAddMunition.IsEnabled = false;
                    btnSave.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Initialization error: {ex.Message}");
            }
        }

        // Mühimmatları yükleme işlemi
        private void LoadAircraftMunitions(int aircraftId)
        {
            try
            {
                var munitions = _aircraftService.GetAircraftMunitions(aircraftId);

                foreach (var munition in munitions)
                {
                    AddMunitionRow(munition["MunitionId"], munition["Quantity"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading munitions: {ex.Message}");
            }
        }

        private void AddMunition_Click(object sender, RoutedEventArgs e)
        {
            AddMunitionRow(null, null);
        }

        private void AddMunitionRow(object? selectedMunitionId, object? quantity)
        {
            try
            {
                ComboBox munitionComboBox = new ComboBox
                {
                    Width = 150,
                    Margin = new Thickness(0, 0, 10, 10)
                };

                var munitions = _munitionService.GetAllMunitions();
                munitionComboBox.ItemsSource = munitions.Select(m => new KeyValuePair<long, string>(Convert.ToInt64(m["Id"]), m["Name"]?.ToString() ?? "Unnamed Munition")).ToList();

                munitionComboBox.DisplayMemberPath = "Value";
                munitionComboBox.SelectedValuePath = "Key";

                if (selectedMunitionId != null)
                    munitionComboBox.SelectedValue = (long)selectedMunitionId;

                TextBox quantityTextBox = new TextBox
                {
                    Width = 50,
                    Margin = new Thickness(0, 0, 10, 10),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Text = quantity?.ToString() ?? string.Empty
                };

                Button removeButton = new Button
                {
                    Content = "Remove",
                    Width = 75,
                    Margin = new Thickness(0, 0, 10, 10)
                };

                StackPanel stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal
                };

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
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding munition row: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int aircraftId;
                int? radarIdAsInt = cbRadar.SelectedValue != null ? (int?)Convert.ToInt32(cbRadar.SelectedValue) : null;

                if (_aircraftData == null)
                {
                    aircraftId = _aircraftService.AddAircraft(
                        txtAircraftName.Text,
                        cbAircraftType.SelectedValue != null ? ((AircraftType)cbAircraftType.SelectedValue).ToString() : string.Empty,
                        double.TryParse(txtSpeed.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double speed) ? speed : 0,
                        double.TryParse(txtRange.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double range) ? range : 0,
                        double.TryParse(txtMaxAltitude.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double maxAltitude) ? maxAltitude : 0,
                        cbManeuverability.SelectedValue != null ? ((Maneuverability)cbManeuverability.SelectedValue).ToString() : string.Empty,
                        double.TryParse(txtPayloadCapacity.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double payloadCapacity) ? payloadCapacity : 0,
                        double.TryParse(txtRadarCrossSection.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double radarCrossSection) ? radarCrossSection : 0,
                        //double.TryParse(txtRadarCrossSection.Text, NumberStyles.Any, CultureInfo.GetCultureInfo("tr-TR"), out double radarCrossSection) ? radarCrossSection : 0,
                        cbECMCapability.SelectedValue != null ? ((ECMCapability)cbECMCapability.SelectedValue).ToString() : string.Empty,
                        double.TryParse(txtCost.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double cost) ? cost : 0,
                        radarIdAsInt);
                }
                else
                {
                    aircraftId = (int)_aircraftData.Id;
                    _aircraftService.UpdateAircraft(
                        aircraftId, txtAircraftName.Text,
                        cbAircraftType.SelectedValue != null ? ((AircraftType)cbAircraftType.SelectedValue).ToString() : string.Empty,
                        double.TryParse(txtSpeed.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double speed) ? speed : 0,
                        double.TryParse(txtRange.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double range) ? range : 0,
                        double.TryParse(txtMaxAltitude.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double maxAltitude) ? maxAltitude : 0,
                        cbManeuverability.SelectedValue != null ? ((Maneuverability)cbManeuverability.SelectedValue).ToString() : string.Empty,
                        double.TryParse(txtPayloadCapacity.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double payloadCapacity) ? payloadCapacity : 0,
                        double.TryParse(txtRadarCrossSection.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double radarCrossSection) ? radarCrossSection : 0,
                        //double.TryParse(txtRadarCrossSection.Text, NumberStyles.Any, CultureInfo.GetCultureInfo("tr-TR"), out double radarCrossSection) ? radarCrossSection : 0,
                        cbECMCapability.SelectedValue != null ? ((ECMCapability)cbECMCapability.SelectedValue).ToString() : string.Empty,
                        double.TryParse(txtCost.Text, out double cost) ? cost : 0,
                        radarIdAsInt);
                }

                SaveAircraftMunitions(aircraftId);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving aircraft: {ex.Message}");
            }
        }

        private void SaveAircraftMunitions(int aircraftId)
        {
            try
            {
                var existingMunitions = _aircraftService.GetAircraftMunitions(aircraftId);

                for (int i = 0; i < _munitionComboBoxes.Count; i++)
                {
                    if (_munitionComboBoxes[i].SelectedValue == null || string.IsNullOrEmpty(_munitionQuantityTextBoxes[i].Text))
                    {
                        MessageBox.Show("Mühimmat türünü ve miktarını seçtiğinizden emin olun.");
                        continue;
                    }

                    var selectedMunitionId = Convert.ToInt64(_munitionComboBoxes[i].SelectedValue);
                    int quantity = int.TryParse(_munitionQuantityTextBoxes[i].Text, out int qty) ? qty : 0;

                    var existingMunition = existingMunitions.FirstOrDefault(m => (long)m["MunitionId"] == selectedMunitionId);
                    if (existingMunition != null)
                    {
                        _aircraftService.UpdateAircraftMunition(aircraftId, (int)selectedMunitionId, quantity);
                    }
                    else
                    {
                        _aircraftService.AddMunitionToAircraft(aircraftId, (int)selectedMunitionId, quantity);
                    }
                }

                foreach (var munition in existingMunitions)
                {
                    var munitionId = (long)munition["MunitionId"];
                    if (!_munitionComboBoxes.Any(c => Convert.ToInt64(c.SelectedValue) == munitionId))
                    {
                        _aircraftService.DeleteAircraftMunition(aircraftId, (int)munitionId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving munitions: {ex.Message}");
            }
        }
    }
}
