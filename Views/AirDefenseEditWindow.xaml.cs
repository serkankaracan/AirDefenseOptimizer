using AirDefenseOptimizer.Services;
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

            LoadRadarList();
            LoadMunitionList();

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
                txtECMCapability.Text = _airDefenseData.ECMCapability;
                txtCost.Text = _airDefenseData.Cost.ToString();

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
                txtECMCapability.IsEnabled = false;
                txtCost.IsEnabled = false;
                btnAddRadar.IsEnabled = false;
                btnAddMunition.IsEnabled = false;
                btnSave.IsEnabled = false;
            }
        }

        private void LoadRadarList()
        {
            var radars = _radarService.GetAllRadars();
        }

        private void LoadMunitionList()
        {
            var munitions = _munitionService.GetAllMunitions();
        }

        private void AddRadar_Click(object sender, RoutedEventArgs e)
        {
            AddRadarRow(null, 1);
        }

        private void AddRadarRow(string? selectedRadarId, int quantity)
        {
            ComboBox radarComboBox = new ComboBox
            {
                Width = 150,
                Margin = new Thickness(0, 5, 0, 5)
            };

            var radars = _radarService.GetAllRadars();
            radarComboBox.ItemsSource = radars.Select(r => new KeyValuePair<long, string>(Convert.ToInt64(r["Id"]), r["Name"]?.ToString() ?? "Unnamed Radar")).ToList();
            radarComboBox.DisplayMemberPath = "Value";
            radarComboBox.SelectedValuePath = "Key";

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
            AddMunitionRow(null, 1);
        }

        private void AddMunitionRow(string? selectedMunitionId, int quantity)
        {
            ComboBox munitionComboBox = new ComboBox
            {
                Width = 150,
                Margin = new Thickness(0, 5, 0, 5)
            };

            var munitions = _munitionService.GetAllMunitions();
            munitionComboBox.ItemsSource = munitions.Select(m => new KeyValuePair<long, string>(Convert.ToInt64(m["Id"]), m["Name"]?.ToString() ?? "Unnamed Munition")).ToList();
            munitionComboBox.DisplayMemberPath = "Value";
            munitionComboBox.SelectedValuePath = "Key";

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
            int airDefenseId;

            if (_airDefenseData == null)
            {
                // Yeni hava savunma sistemi ekliyorsunuz
                airDefenseId = _airDefenseService.AddAirDefense(txtAirDefenseName.Text, double.Parse(txtAerodynamicRangeMax.Text), double.Parse(txtAerodynamicRangeMin.Text),
                    double.Parse(txtBallisticRangeMax.Text), double.Parse(txtBallisticRangeMin.Text), int.Parse(txtMaxEngagements.Text),
                    int.Parse(txtMaxMissilesFired.Text), txtECMCapability.Text, double.Parse(txtCost.Text));
            }
            else
            {
                // Var olan hava savunma sistemini güncelliyorsunuz
                airDefenseId = (int)_airDefenseData.Id;
                _airDefenseService.UpdateAirDefense(airDefenseId, txtAirDefenseName.Text, double.Parse(txtAerodynamicRangeMax.Text),
                    double.Parse(txtAerodynamicRangeMin.Text), double.Parse(txtBallisticRangeMax.Text), double.Parse(txtBallisticRangeMin.Text),
                    int.Parse(txtMaxEngagements.Text), int.Parse(txtMaxMissilesFired.Text), txtECMCapability.Text, double.Parse(txtCost.Text));
            }

            // Radarı ve mühimmatı doğru airDefenseId ile kaydedin
            SaveRadars(airDefenseId);
            SaveMunitions(airDefenseId);

            this.Close();
        }

        private void SaveRadars(int airDefenseId)
        {
            var existingRadars = _airDefenseService.GetAirDefenseRadars(airDefenseId);

            // Mevcut radarları kontrol ederek güncelle veya sil
            foreach (var existingRadar in existingRadars)
            {
                var radarId = Convert.ToInt64(existingRadar["RadarId"]);
                var selectedRadarComboBox = _radarComboBoxes.FirstOrDefault(cb => Convert.ToInt64(cb.SelectedValue) == radarId);

                if (selectedRadarComboBox == null)
                {
                    // Eğer radar listede yoksa, radarı sil
                    _airDefenseService.DeleteAirDefenseRadar(airDefenseId, (int)radarId);
                }
                else
                {
                    // Eğer radar listede varsa, miktarı güncelle
                    int index = _radarComboBoxes.IndexOf(selectedRadarComboBox);
                    int quantity = int.Parse(_radarQuantityTextBoxes[index].Text);
                    _airDefenseService.UpdateAirDefenseRadar(airDefenseId, (int)radarId, quantity);
                }
            }

            // Yeni eklenen radarları ekleyin
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

        private void SaveMunitions(int airDefenseId)
        {
            var existingMunitions = _airDefenseService.GetAirDefenseMunitions(airDefenseId);

            // Mevcut mühimmatları kontrol ederek güncelle veya sil
            foreach (var existingMunition in existingMunitions)
            {
                var munitionId = Convert.ToInt64(existingMunition["MunitionId"]);
                var selectedMunitionComboBox = _munitionComboBoxes.FirstOrDefault(cb => Convert.ToInt64(cb.SelectedValue) == munitionId);

                if (selectedMunitionComboBox == null)
                {
                    // Eğer mühimmat listede yoksa, mühimmatı sil
                    _airDefenseService.DeleteAirDefenseMunition(airDefenseId, (int)munitionId);
                }
                else
                {
                    // Eğer mühimmat listede varsa, miktarı güncelle
                    int index = _munitionComboBoxes.IndexOf(selectedMunitionComboBox);
                    int quantity = int.Parse(_munitionQuantityTextBoxes[index].Text);
                    _airDefenseService.UpdateAirDefenseMunition(airDefenseId, (int)munitionId, quantity);
                }
            }

            // Yeni eklenen mühimmatları ekleyin
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



    }
}
