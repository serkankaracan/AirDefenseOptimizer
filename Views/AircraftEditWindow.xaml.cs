using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Services;
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

            // AircraftType enum'ını ComboBox'a doldur
            cbAircraftType.ItemsSource = Enum.GetValues(typeof(AircraftType))
                .Cast<AircraftType>()
                .Select(a => new KeyValuePair<AircraftType, string>(a, a.GetAircraftTypeName()))
                .ToList();

            cbManeuverability.ItemsSource = Enum.GetValues(typeof(Maneuverability))
               .Cast<Maneuverability>()
               .Select(a => new KeyValuePair<Maneuverability, string>(a, a.GetManeuverabilityName()))
               .ToList();

            // Radarları combobox'a doldur
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

                // Radarın adını göstermek için DisplayMemberPath ve SelectedValuePath ayarlarını yapın
                cbRadar.DisplayMemberPath = "Value"; // Radar adı
                cbRadar.SelectedValuePath = "Key"; // Radar ID
            }

            if (_aircraftData != null)
            {
                // Var olan uçak verilerini doldur
                txtAircraftName.Text = _aircraftData.Name;
                cbAircraftType.SelectedValue = _aircraftData.AircraftType != null ? Enum.Parse<AircraftType>(_aircraftData.AircraftType.ToString()) : null;
                txtSpeed.Text = _aircraftData.Speed.ToString();
                txtRange.Text = _aircraftData.Range.ToString();
                txtMaxAltitude.Text = _aircraftData.MaxAltitude.ToString();
                cbManeuverability.SelectedValue = _aircraftData.Maneuverability != null ? Enum.Parse<Maneuverability>(_aircraftData.Maneuverability.ToString()) : null; 
                txtPayloadCapacity.Text = _aircraftData.PayloadCapacity.ToString();
                string radarName = radars.FirstOrDefault(r => r["Id"].ToString() == _aircraftData.RadarId?.ToString())?["Name"]?.ToString() ?? "No Radar";
                cbRadar.SelectedValue = radars.FirstOrDefault(r => r["Name"].ToString() == radarName)?["Id"];
                txtCost.Text = _aircraftData.Cost.ToString();

                // Mühimmatları yükleme
                LoadAircraftMunitions((int)_aircraftData.Id);
            }

            // Eğer sadece önizleme modundaysak tüm alanları read-only yap
            if (_isReadOnly)
            {
                txtAircraftName.IsEnabled = false;
                cbAircraftType.IsEnabled = false;
                txtSpeed.IsEnabled = false;
                txtRange.IsEnabled = false;
                txtMaxAltitude.IsEnabled = false;
                cbManeuverability.IsEnabled = false;
                txtPayloadCapacity.IsEnabled = false;
                cbRadar.IsEnabled = false;
                txtCost.IsEnabled = false;
                btnAddMunition.IsEnabled = false;
                btnSave.IsEnabled = false;  // Kaydet butonunu devre dışı bırak
            }
        }

        // Mühimmatları yükleme işlemi
        private void LoadAircraftMunitions(int aircraftId)
        {
            var munitions = _aircraftService.GetAircraftMunitions(aircraftId);

            foreach (var munition in munitions)
            {
                // Her mühimmat için satır ekle
                AddMunitionRow(munition["MunitionId"], munition["Quantity"]);
            }
        }

        // Yeni mühimmat eklemek için butona tıklandığında çalışacak metot
        private void AddMunition_Click(object sender, RoutedEventArgs e)
        {
            AddMunitionRow(null, null);
        }

        // Mühimmat ekleme satırı oluşturma
        private void AddMunitionRow(object? selectedMunitionId, object? quantity)
        {
            // Mühimmat türü seçmek için bir ComboBox oluştur
            ComboBox munitionComboBox = new ComboBox
            {
                Width = 150,
                Margin = new Thickness(0, 0, 10, 10)
            };

            // Mühimmat türlerini ComboBox'a ekle
            var munitions = _munitionService.GetAllMunitions();
            munitionComboBox.ItemsSource = munitions.Select(m => new KeyValuePair<long, string>(Convert.ToInt64(m["Id"]), m["Name"]?.ToString() ?? "Unnamed Munition")).ToList();

            munitionComboBox.DisplayMemberPath = "Value";
            munitionComboBox.SelectedValuePath = "Key";

            if (selectedMunitionId != null)
                munitionComboBox.SelectedValue = (long)selectedMunitionId;

            // Mühimmat miktarını girmek için bir TextBox oluştur
            TextBox quantityTextBox = new TextBox
            {
                Width = 50,
                Margin = new Thickness(0, 0, 10, 10),
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = quantity?.ToString() ?? string.Empty
            };

            // Mühimmat satırını kaldırmak için bir Buton oluştur
            Button removeButton = new Button
            {
                Content = "Remove",
                Width = 75,
                Margin = new Thickness(0, 0, 10, 10)
            };

            // Yeni mühimmat satırını içeren StackPanel
            StackPanel stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            // ComboBox, TextBox ve Remove Button'u StackPanel'e ekle
            stackPanel.Children.Add(munitionComboBox);
            stackPanel.Children.Add(quantityTextBox);
            stackPanel.Children.Add(removeButton);

            // StackPanel'i MunitionList'e ekle
            MunitionList.Children.Add(stackPanel);

            // ComboBox ve TextBox'ı listelere ekle
            _munitionComboBoxes.Add(munitionComboBox);
            _munitionQuantityTextBoxes.Add(quantityTextBox);

            // Mühimmat satırını silmek için event handler
            removeButton.Click += (s, ev) =>
            {
                MunitionList.Children.Remove(stackPanel);
                _munitionComboBoxes.Remove(munitionComboBox);
                _munitionQuantityTextBoxes.Remove(quantityTextBox);
            };
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            int aircraftId;

            // Radar ID'sini nullable int olarak al
            int? radarIdAsInt = null;
            if (cbRadar.SelectedValue != null)
            {
                long selectedRadarId;
                if (long.TryParse(cbRadar.SelectedValue.ToString(), out selectedRadarId))
                {
                    radarIdAsInt = Convert.ToInt32(selectedRadarId);
                }
            }

            if (_aircraftData == null)
            {
                // Yeni uçak ekleme işlemi
                aircraftId = _aircraftService.AddAircraft(
                    txtAircraftName.Text,
                    ((AircraftType)cbAircraftType.SelectedValue).ToString(),
                    double.Parse(txtSpeed.Text),
                    double.Parse(txtRange.Text),
                    double.Parse(txtMaxAltitude.Text),
                    ((Maneuverability)cbManeuverability.SelectedValue).ToString(),
                    double.Parse(txtPayloadCapacity.Text),
                    double.Parse(txtCost.Text),
                    radarIdAsInt);
            }
            else
            {
                // Var olan uçak güncelleme işlemi
                aircraftId = (int)_aircraftData.Id;
                _aircraftService.UpdateAircraft(aircraftId, txtAircraftName.Text,
                    ((AircraftType)cbAircraftType.SelectedValue).ToString(),
                    double.Parse(txtSpeed.Text),
                    double.Parse(txtRange.Text),
                    double.Parse(txtMaxAltitude.Text),
                    ((Maneuverability)cbManeuverability.SelectedValue).ToString(),
                    double.Parse(txtPayloadCapacity.Text),
                    double.Parse(txtCost.Text),
                    radarIdAsInt);
            }

            // Mühimmatları kaydet
            SaveAircraftMunitions(aircraftId);

            this.Close();
        }

        // Mühimmatları kaydetme işlemi
        private void SaveAircraftMunitions(int aircraftId)
        {
            var existingMunitions = _aircraftService.GetAircraftMunitions(aircraftId); // Önceden eklenmiş mühimmatlar

            for (int i = 0; i < _munitionComboBoxes.Count; i++)
            {
                var selectedMunitionId = Convert.ToInt64(_munitionComboBoxes[i].SelectedValue);
                int quantity = int.Parse(_munitionQuantityTextBoxes[i].Text);

                // Var olan mühimmat mı yoksa yeni mi kontrol et
                var existingMunition = existingMunitions.FirstOrDefault(m => (long)m["MunitionId"] == selectedMunitionId);
                if (existingMunition != null)
                {
                    // Var olan mühimmat için güncelleme
                    _aircraftService.UpdateAircraftMunition(aircraftId, (int)selectedMunitionId, quantity);
                }
                else
                {
                    // Yeni mühimmat ekleme
                    _aircraftService.AddMunitionToAircraft(aircraftId, (int)selectedMunitionId, quantity);
                }
            }

            // Silinen mühimmatları tespit et
            foreach (var munition in existingMunitions)
            {
                var munitionId = (long)munition["MunitionId"];
                if (!_munitionComboBoxes.Any(c => Convert.ToInt64(c.SelectedValue) == munitionId))
                {
                    _aircraftService.DeleteAircraftMunition(aircraftId, (int)munitionId);
                }
            }
        }
    }
}
