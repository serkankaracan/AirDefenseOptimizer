using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Services;
using System.Globalization;
using System.Windows;

namespace AirDefenseOptimizer.Views
{
    public partial class RadarEditWindow : Window
    {
        private readonly bool _isReadOnly;
        private readonly dynamic? _radarData;
        private readonly RadarService _radarService;

        public RadarEditWindow(RadarService radarService, dynamic? radarData = null, bool isReadOnly = false)
        {
            InitializeComponent();

            try
            {
                _radarService = radarService;
                _radarData = radarData;
                _isReadOnly = isReadOnly;

                // Radar Type enum'ını ComboBox'a doldur
                cbRadarType.ItemsSource = Enum.GetValues(typeof(RadarType))
                    .Cast<RadarType>()
                    .Select(r => new KeyValuePair<RadarType, string>(r, r.GetRadarTypeName()))
                    .ToList();

                if (_radarData != null)
                {
                    // Var olan radar verilerini doldur
                    txtRadarName.Text = _radarData.Name;
                    cbRadarType.SelectedValue = Enum.Parse<RadarType>(_radarData.RadarType);
                    txtMaxDetectionTargets.Text = _radarData.MaxDetectionTargets.ToString();
                    txtMaxTrackingTargets.Text = _radarData.MaxTrackingTargets.ToString();
                    txtMinDetectionRange.Text = _radarData.MinDetectionRange.ToString();
                    txtMaxDetectionRange.Text = _radarData.MaxDetectionRange.ToString();
                    txtMinAltitude.Text = _radarData.MinAltitude.ToString();
                    txtMaxAltitude.Text = _radarData.MaxAltitude.ToString();
                    txtMaxTargetSpeed.Text = _radarData.MaxTargetSpeed.ToString();
                    txtMaxTargetVelocity.Text = _radarData.MaxTargetVelocity.ToString();
                    txtRedeploymentTime.Text = _radarData.RedeploymentTime.ToString();

                    // Eğer sadece önizleme modundaysak tüm alanları read-only yap
                    if (_isReadOnly)
                    {
                        txtRadarName.IsEnabled = false;
                        cbRadarType.IsEnabled = false;
                        txtMaxDetectionTargets.IsEnabled = false;
                        txtMaxTrackingTargets.IsEnabled = false;
                        txtMinDetectionRange.IsEnabled = false;
                        txtMaxDetectionRange.IsEnabled = false;
                        txtMinAltitude.IsEnabled = false;
                        txtMaxAltitude.IsEnabled = false;
                        txtMaxTargetSpeed.IsEnabled = false;
                        txtMaxTargetVelocity.IsEnabled = false;
                        txtRedeploymentTime.IsEnabled = false;
                        btnSave.IsEnabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing the radar edit window: {ex.Message}");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_radarData == null)
                {
                    // Yeni radar ekleme işlemi
                    _radarService.AddRadar(
                        txtRadarName.Text,
                        ((RadarType)cbRadarType.SelectedValue).ToString(),
                        int.Parse(txtMaxDetectionTargets.Text),
                        int.Parse(txtMaxTrackingTargets.Text),
                        double.TryParse(txtMinDetectionRange.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double minDetectionRange) ? minDetectionRange : 0,
                        double.TryParse(txtMaxDetectionRange.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double maxDetectionRange) ? maxDetectionRange : 0,
                        double.TryParse(txtMaxAltitude.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double maxAltitude) ? maxAltitude : 0,
                        double.TryParse(txtMinAltitude.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double minAltitude) ? minAltitude : 0,
                        double.TryParse(txtMaxTargetSpeed.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double maxTargetSpeed) ? maxTargetSpeed : 0,
                        double.TryParse(txtMaxTargetVelocity.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double maxTargetVelocity) ? maxTargetVelocity : 0,
                        int.Parse(txtRedeploymentTime.Text)
                    );
                }
                else
                {
                    // Var olan radar güncelleme işlemi
                    _radarService.UpdateRadar(
                        (int)_radarData.Id,
                        txtRadarName.Text,
                        ((RadarType)cbRadarType.SelectedValue).ToString(),
                        int.Parse(txtMaxDetectionTargets.Text),
                        int.Parse(txtMaxTrackingTargets.Text),
                        double.TryParse(txtMinDetectionRange.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double minDetectionRange) ? minDetectionRange : 0,
                        double.TryParse(txtMaxDetectionRange.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double maxDetectionRange) ? maxDetectionRange : 0,
                        double.TryParse(txtMaxAltitude.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double maxAltitude) ? maxAltitude : 0,
                        double.TryParse(txtMinAltitude.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double minAltitude) ? minAltitude : 0,
                        double.TryParse(txtMaxTargetSpeed.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double maxTargetSpeed) ? maxTargetSpeed : 0,
                        double.TryParse(txtMaxTargetVelocity.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double maxTargetVelocity) ? maxTargetVelocity : 0,
                        int.Parse(txtRedeploymentTime.Text)
                    );
                }

                MessageBox.Show("Radar information saved successfully.");
                this.Close();
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Input format is incorrect. Please check numeric fields. Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the radar information: {ex.Message}");
            }
        }
    }
}
