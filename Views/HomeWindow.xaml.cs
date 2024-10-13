using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Services;
using System.Windows;
using System.Windows.Controls;
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
            // Eğer daha önce satır eklenmediyse, üst kısma label'lar ekleyelim.
            if (ThreatList.Children.Count == 0)
            {
                // Uçak, IFF ve Konum için label'lar ekleyelim
                StackPanel labelsPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                labelsPanel.Children.Add(new Label
                {
                    Content = "Aircraft:",
                    Width = 180,
                    Margin = new Thickness(0),
                    Padding = new Thickness(5)
                });

                labelsPanel.Children.Add(new Label
                {
                    Content = "IFF Mode:",
                    Width = 100,
                    Margin = new Thickness(0),
                    Padding = new Thickness(5)
                });

                labelsPanel.Children.Add(new Label
                {
                    Content = "Location (Latitude, Longitude, Altitude)",
                    Width = 240,
                    Margin = new Thickness(0),
                    Padding = new Thickness(5)
                });

                ThreatList.Children.Add(labelsPanel);
            }

            // Grid'in estetik olarak düzenlenmesi ve daha dengeli görünmesi için yeniden ayarladım.
            Grid threatGrid = new Grid
            {
                Margin = new Thickness(0, 10, 0, 10)
            };

            // Grid sütunlarını düzenliyoruz, genişlikleri sabitliyoruz
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(180) });
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(160) });
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

            // Uçak Seçimi
            ComboBox aircraftComboBox = new ComboBox
            {
                Width = 180,
                Height = 30,
                Margin = new Thickness(0),
                Padding = new Thickness(5)
            };
            // Placeholder olarak "Uçak seçin" ekleniyor, ancak seçilemeyecek
            aircraftComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Aircraft",
                IsEnabled = false, // Bu öğe seçilemez
                IsSelected = true  // Varsayılan olarak seçili
            });

            var aircrafts = _aircraftService.GetAllAircrafts();
            if (aircrafts != null && aircrafts.Any())
            {
                foreach (var aircraft in aircrafts)
                {
                    aircraftComboBox.Items.Add(aircraft["Name"].ToString());
                }
            }
            else
            {
                MessageBox.Show("Aircraft list is empty.");
            }

            // IFF Seçimi
            ComboBox iffComboBox = new ComboBox
            {
                Width = 100,
                Height = 30,
                Margin = new Thickness(0),
                Padding = new Thickness(5)
            };
            // Placeholder olarak "IFF seçin" ekleniyor, ancak seçilemeyecek
            iffComboBox.Items.Add(new ComboBoxItem
            {
                Content = "IFF mode",
                IsEnabled = false, // Bu öğe seçilemez
                IsSelected = true  // Varsayılan olarak seçili
            });

            var iffValues = Enum.GetValues(typeof(IFF));
            foreach (var iff in iffValues)
            {
                iffComboBox.Items.Add(iff);
            }

            // Konum Girdisi (Latitude, Longitude, Altitude)
            TextBox locationTextBox = new TextBox
            {
                Width = 160,
                Margin = new Thickness(0),
                Padding = new Thickness(5),
                MaxLength = 25
            };

            // Kaldır Butonu
            Button removeButton = new Button
            {
                Content = "Remove",
                Width = 80,
                Height = 30,
                Margin = new Thickness(0),
                Padding = new Thickness(5),
                Background = new SolidColorBrush(System.Windows.Media.Colors.IndianRed),
                Foreground = new SolidColorBrush(System.Windows.Media.Colors.White)
            };
            removeButton.Click += (s, ev) =>
            {
                ThreatList.Children.Remove(threatGrid);
                if (ThreatList.Children.Count == 1) // Yalnızca label'lar kaldıysa onları da kaldıralım
                {
                    ThreatList.Children.Clear();
                }
            };

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
            // Eğer daha önce satır eklenmediyse, üst kısma label'lar ekleyelim.
            if (DefenseList.Children.Count == 0)
            {
                // Savunma Sistemi ve Konum için label'lar ekleyelim
                StackPanel labelsPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                labelsPanel.Children.Add(new Label
                {
                    Content = "Air Defense System",
                    Width = 180,
                    Margin = new Thickness(0),
                    Padding = new Thickness(5)
                });

                labelsPanel.Children.Add(new Label
                {
                    Content = "Location (Latitude, Longitude, Altitude)",
                    Width = 240,
                    Margin = new Thickness(0),
                    Padding = new Thickness(5)
                });

                DefenseList.Children.Add(labelsPanel);
            }

            // Grid tasarımı tehditlerde olduğu gibi daha düzenli bir yapıda yapıldı
            Grid defenseGrid = new Grid
            {
                Margin = new Thickness(0, 10, 0, 10)
            };
            defenseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(180) });
            defenseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(160) });
            defenseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

            // Hava Savunma Seçimi
            ComboBox defenseComboBox = new ComboBox
            {
                Width = 180,
                Height = 30,
                Margin = new Thickness(0),
                Padding = new Thickness(5)
            };
            // Placeholder olarak "HSS seçin" ekleniyor, ancak seçilemeyecek
            defenseComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Air Defense System",
                IsEnabled = false, // Bu öğe seçilemez
                IsSelected = true  // Varsayılan olarak seçili
            });

            var airDefenseSystems = _airDefenseService.GetAllAirDefenseSystems();
            if (airDefenseSystems != null && airDefenseSystems.Any())
            {
                foreach (var system in airDefenseSystems)
                {
                    defenseComboBox.Items.Add(system["Name"].ToString());
                }
            }
            else
            {
                MessageBox.Show("Air Defense list is empty.");
            }

            // Konum Girdisi
            TextBox locationTextBox = new TextBox
            {
                Width = 160,
                Margin = new Thickness(0),
                Padding = new Thickness(5),
                MaxLength = 25
            };

            // Kaldır Butonu
            Button removeButton = new Button
            {
                Content = "Remove",
                Width = 80,
                Height = 30,
                Margin = new Thickness(0),
                Padding = new Thickness(5),
                Background = new SolidColorBrush(System.Windows.Media.Colors.IndianRed),
                Foreground = new SolidColorBrush(System.Windows.Media.Colors.White)
            };
            removeButton.Click += (s, ev) =>
            {
                DefenseList.Children.Remove(defenseGrid);
                if (DefenseList.Children.Count == 1) // Yalnızca label'lar kaldıysa onları da kaldıralım
                {
                    DefenseList.Children.Clear();
                }
            };

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
