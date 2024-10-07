using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Services;
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
                    txtMinDetectionRange.IsEnabled = false;
                    txtMaxDetectionRange.IsEnabled = false;
                    txtMinAltitude.IsEnabled = false;
                    txtMaxAltitude.IsEnabled = false;
                    txtMaxTargetSpeed.IsEnabled = false;
                    txtMaxTargetVelocity.IsEnabled = false;
                    txtRedeploymentTime.IsEnabled = false;
                    btnSave.IsEnabled = false;  // Kaydet butonunu devre dışı bırak
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_radarData == null)
            {
                // Yeni radar ekleme işlemi
                _radarService.AddRadar(txtRadarName.Text, ((RadarType)cbRadarType.SelectedValue).ToString(),
                    double.Parse(txtMinDetectionRange.Text), double.Parse(txtMaxDetectionRange.Text),
                    double.Parse(txtMaxAltitude.Text), double.Parse(txtMinAltitude.Text),
                    double.Parse(txtMaxTargetSpeed.Text), double.Parse(txtMaxTargetVelocity.Text), int.Parse(txtRedeploymentTime.Text));
            }
            else
            {
                // Var olan radar güncelleme işlemi
                _radarService.UpdateRadar((int)_radarData.Id, txtRadarName.Text,
                    ((RadarType)cbRadarType.SelectedValue).ToString(),
                    double.Parse(txtMinDetectionRange.Text), double.Parse(txtMaxDetectionRange.Text),
                    double.Parse(txtMaxAltitude.Text), double.Parse(txtMinAltitude.Text),
                    double.Parse(txtMaxTargetSpeed.Text), double.Parse(txtMaxTargetVelocity.Text), int.Parse(txtRedeploymentTime.Text));
            }

            this.Close();
        }
    }
}
