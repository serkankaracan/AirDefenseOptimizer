using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Services;
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

            // Munition Type enum'ını ComboBox'a doldur
            cbMunitionType.ItemsSource = Enum.GetValues(typeof(MunitionType))
                .Cast<MunitionType>()
                .Select(m => new KeyValuePair<MunitionType, string>(m, m.GetMunitionTypeName()))
                .ToList();

            cbManeuverability.ItemsSource = Enum.GetValues(typeof(Maneuverability))
               .Cast<Maneuverability>()
               .Select(a => new KeyValuePair<Maneuverability, string>(a, a.GetManeuverabilityName()))
               .ToList();

            if (_munitionData != null)
            {
                // Var olan mühimmat verilerini doldur
                txtMunitionName.Text = _munitionData.Name;
                cbMunitionType.SelectedValue = Enum.Parse<MunitionType>(_munitionData.MunitionType);
                txtWeight.Text = _munitionData.Weight.ToString();
                txtSpeed.Text = _munitionData.Speed.ToString();
                txtRange.Text = _munitionData.Range.ToString();
                cbManeuverability.SelectedValue = Enum.Parse<Maneuverability>(_munitionData.Maneuverability);
                txtExplosivePower.Text = _munitionData.ExplosivePower.ToString();
                txtCost.Text = _munitionData.Cost.ToString();

                // Eğer sadece önizleme modundaysak tüm alanları read-only yap
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
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_munitionData == null)
                {
                    // Yeni mühimmat ekleme işlemi
                    _munitionService.AddMunition(
                        txtMunitionName.Text, 
                        ((MunitionType)cbMunitionType.SelectedValue).ToString(),
                        double.Parse(txtWeight.Text), 
                        double.Parse(txtSpeed.Text), 
                        double.Parse(txtRange.Text),
                        ((Maneuverability)cbManeuverability.SelectedValue).ToString(),
                        double.Parse(txtExplosivePower.Text), 
                        double.Parse(txtCost.Text));
                }
                else
                {
                    // Var olan mühimmat güncelleme işlemi
                    _munitionService.UpdateMunition(
                        (int)_munitionData.Id, txtMunitionName.Text,
                        ((MunitionType)cbMunitionType.SelectedValue).ToString(),
                        double.Parse(txtWeight.Text), 
                        double.Parse(txtSpeed.Text), 
                        double.Parse(txtRange.Text),
                        ((Maneuverability)cbManeuverability.SelectedValue).ToString(),
                        double.Parse(txtExplosivePower.Text), 
                        double.Parse(txtCost.Text));
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
