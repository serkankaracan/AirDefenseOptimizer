using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Services;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Media;

namespace AirDefenseOptimizer.Views
{
    public partial class HomeWindow : UserControl
    {
        private readonly AirDefenseService _airDefenseService;
        private readonly AircraftService _aircraftService;

        public HomeWindow()
        {
            InitializeComponent();

            _airDefenseService = new AirDefenseService(App.ConnectionManager!, App.DatabaseHelper!);
            _aircraftService = new AircraftService(App.ConnectionManager!, App.DatabaseHelper!);
        }

        // Uçak Tehdidi Ekleme Butonuna Tıklanınca Çalışacak
        private void AddAircraftThreat_Click(object sender, RoutedEventArgs e)
        {
            // Grid'in estetik olarak düzenlenmesi ve daha dengeli görünmesi için yeniden ayarladım.
            Grid threatGrid = new Grid
            {
                Margin = new Thickness(0, 10, 0, 10) // Grid'i yukarı-aşağı boşluklarla ayırıyoruz
            };

            // Grid sütunlarını düzenliyoruz
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

            // Uçak Seçimi
            ComboBox aircraftComboBox = new ComboBox
            {
                Width = 180,
                Height = 30,
                Margin = new Thickness(5)
            };

            var aircrafts = _aircraftService.GetAllAircrafts();
            if (aircrafts != null && aircrafts.Any())
            {
                aircraftComboBox.ItemsSource = aircrafts.Select(a => new { Id = a["Id"], Name = a["Name"].ToString() }).ToList();
                aircraftComboBox.DisplayMemberPath = "Name";
                aircraftComboBox.SelectedValuePath = "Id";
            }
            else
            {
                MessageBox.Show("Aircraft list is empty.");
            }

            // IFF Seçimi
            ComboBox iffComboBox = new ComboBox
            {
                Width = 120,
                Height = 30,
                Margin = new Thickness(5),
                ItemsSource = Enum.GetValues(typeof(IFF))
            };

            // Konum Girdisi (X, Y, Z)
            TextBox locationTextBox = new TextBox
            {
                Width = 220,
                Margin = new Thickness(5)
            };

            // Kaldır Butonu
            Button removeButton = new Button
            {
                Content = "Remove",
                Width = 80,
                Height = 30,
                Margin = new Thickness(5),
                Background = new SolidColorBrush(System.Windows.Media.Colors.IndianRed), // Renk ile daha belirgin hale getiriyoruz
                Foreground = new SolidColorBrush(System.Windows.Media.Colors.White)      // Yazı rengini beyaz yapıyoruz
            };
            removeButton.Click += (s, ev) => ThreatList.Children.Remove(threatGrid);

            // Grid'e elemanları ekliyoruz
            threatGrid.Children.Add(aircraftComboBox);
            Grid.SetColumn(aircraftComboBox, 0);

            threatGrid.Children.Add(iffComboBox);
            Grid.SetColumn(iffComboBox, 1);

            threatGrid.Children.Add(locationTextBox);
            Grid.SetColumn(locationTextBox, 2);

            threatGrid.Children.Add(removeButton);
            Grid.SetColumn(removeButton, 3);

            // Tehdit Listesine ekliyoruz
            ThreatList.Children.Add(threatGrid);
        }

        // Hava Savunma Sistemi Ekleme Butonuna Tıklanınca Çalışacak
        private void AddAirDefenseSystem_Click(object sender, RoutedEventArgs e)
        {
            // Grid tasarımı tehditlerde olduğu gibi daha düzenli bir yapıda yapıldı
            Grid defenseGrid = new Grid
            {
                Margin = new Thickness(0, 10, 0, 10)
            };
            defenseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            defenseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(250) });
            defenseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

            // Hava Savunma Seçimi
            ComboBox defenseComboBox = new ComboBox
            {
                Width = 180,
                Height = 30,
                Margin = new Thickness(5)
            };

            var airDefenseSystems = _airDefenseService.GetAllAirDefenseSystems();
            if (airDefenseSystems != null && airDefenseSystems.Any())
            {
                defenseComboBox.ItemsSource = airDefenseSystems.Select(ad => new { Id = ad["Id"], Name = ad["Name"].ToString() }).ToList();
                defenseComboBox.DisplayMemberPath = "Name";
                defenseComboBox.SelectedValuePath = "Id";
            }
            else
            {
                MessageBox.Show("Air Defense list is empty.");
            }

            // Konum Girdisi
            TextBox locationTextBox = new TextBox
            {
                Width = 220,
                Margin = new Thickness(5)
            };

            // Kaldır Butonu
            Button removeButton = new Button
            {
                Content = "Remove",
                Width = 80,
                Height = 30,
                Margin = new Thickness(5),
                Background = new SolidColorBrush(System.Windows.Media.Colors.IndianRed),
                Foreground = new SolidColorBrush(System.Windows.Media.Colors.White)
            };
            removeButton.Click += (s, ev) => DefenseList.Children.Remove(defenseGrid);

            // Grid'e elemanları ekliyoruz
            defenseGrid.Children.Add(defenseComboBox);
            Grid.SetColumn(defenseComboBox, 0);

            defenseGrid.Children.Add(locationTextBox);
            Grid.SetColumn(locationTextBox, 1);

            defenseGrid.Children.Add(removeButton);
            Grid.SetColumn(removeButton, 2);

            // Savunma Listesine ekliyoruz
            DefenseList.Children.Add(defenseGrid);
        }
    }
}
