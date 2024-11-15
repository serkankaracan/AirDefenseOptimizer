using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Services;
using System.Globalization;
using System.Windows;

namespace AirDefenseOptimizer.Views
{
    public partial class MunitionEditWindow : Window
    {
        private readonly bool _isReadOnly;
        private readonly dynamic? _munitionData;
        private readonly MunitionService _munitionService;

        public MunitionEditWindow(MunitionService munitionService, dynamic? munitionData = null, bool isReadOnly = false)
        {
            InitializeComponent();
            _munitionService = munitionService;
            _munitionData = munitionData;
            _isReadOnly = isReadOnly;

            try
            {
                // Munition Type enum'ını ComboBox'a doldur
                cbMunitionType.ItemsSource = Enum.GetValues(typeof(MunitionType))
                    .Cast<MunitionType>()
                    .Select(m => new KeyValuePair<MunitionType, string>(m, m.GetMunitionTypeName()))
                    .ToList();

                cbManeuverability.ItemsSource = Enum.GetValues(typeof(Maneuverability))
                   .Cast<Maneuverability>()
                   .Select(a => new KeyValuePair<Maneuverability, string>(a, a.GetManeuverabilityName()))
                   .ToList();

                // Eğer düzenleme modundaysa, mevcut mühimmat verilerini yükle
                if (_munitionData != null)
                {
                    LoadMunitionData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Munition Edit Window: {ex.Message}");
            }
        }

        private void LoadMunitionData()
        {
            try
            {
                txtMunitionName.Text = _munitionData.Name;
                cbMunitionType.SelectedValue = Enum.Parse<MunitionType>(_munitionData.MunitionType);
                txtWeight.Text = _munitionData.Weight.ToString();
                txtSpeed.Text = _munitionData.Speed.ToString();
                txtRange.Text = _munitionData.Range.ToString();
                cbManeuverability.SelectedValue = Enum.Parse<Maneuverability>(_munitionData.Maneuverability);
                txtExplosivePower.Text = _munitionData.ExplosivePower.ToString();
                txtCost.Text = _munitionData.Cost.ToString();

                if (_isReadOnly)
                {
                    txtMunitionName.IsEnabled = false;
                    cbMunitionType.IsEnabled = false;
                    txtWeight.IsEnabled = false;
                    txtSpeed.IsEnabled = false;
                    txtRange.IsEnabled = false;
                    cbManeuverability.IsEnabled = false;
                    txtExplosivePower.IsEnabled = false;
                    txtCost.IsEnabled = false;
                    btnSave.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading munition data: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verilerin doğruluğunu kontrol et
                ValidateInputs();

                if (_munitionData == null)
                {
                    // Yeni mühimmat ekleme işlemi
                    _munitionService.AddMunition(
                        txtMunitionName.Text,
                        ((MunitionType)cbMunitionType.SelectedValue).ToString(),
                        double.TryParse(txtWeight.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double weight) ? weight : 0,
                        double.TryParse(txtSpeed.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double speed) ? speed : 0,
                        double.TryParse(txtRange.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double range) ? range : 0,
                        ((Maneuverability)cbManeuverability.SelectedValue).ToString(),
                        double.TryParse(txtExplosivePower.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double explosivePower) ? explosivePower : 0,
                        double.Parse(txtCost.Text));
                }
                else
                {
                    // Var olan mühimmat güncelleme işlemi
                    _munitionService.UpdateMunition(
                        (int)_munitionData.Id, txtMunitionName.Text,
                        ((MunitionType)cbMunitionType.SelectedValue).ToString(),
                        double.TryParse(txtWeight.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double weight) ? weight : 0,
                        double.TryParse(txtSpeed.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double speed) ? speed : 0,
                        double.TryParse(txtRange.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double range) ? range : 0,
                        ((Maneuverability)cbManeuverability.SelectedValue).ToString(),
                        double.Parse(txtExplosivePower.Text),
                        double.Parse(txtCost.Text));
                }

                this.Close();
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Invalid data format: {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show($"Please ensure all fields are filled: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtMunitionName.Text) ||
                cbMunitionType.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtWeight.Text) ||
                string.IsNullOrWhiteSpace(txtSpeed.Text) ||
                string.IsNullOrWhiteSpace(txtRange.Text) ||
                cbManeuverability.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtExplosivePower.Text) ||
                string.IsNullOrWhiteSpace(txtCost.Text))
            {
                MessageBox.Show($"All fields must be filled.");
            }

            // Sayısal alanlar için ek doğrulama
            if (!double.TryParse(txtWeight.Text, out _) ||
                !double.TryParse(txtSpeed.Text, out _) ||
                !double.TryParse(txtRange.Text, out _) ||
                !double.TryParse(txtExplosivePower.Text, out _) ||
                !double.TryParse(txtCost.Text, out _))
            {
                MessageBox.Show($"Numeric fields must contain valid numbers.");
            }
        }
    }
}
