using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Models;
using AirDefenseOptimizer.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Maneuverability = AirDefenseOptimizer.Enums.Maneuverability;

namespace AirDefenseOptimizer.Views
{
    public partial class HomeWindow : UserControl
    {
        private readonly AirDefenseService _airDefenseService;
        private readonly AircraftService _aircraftService;

        private List<Aircraft> _aircraftList = new List<Aircraft>();
        private List<AirDefense> _airDefenseList = new List<AirDefense>();

        public HomeWindow()
        {
            InitializeComponent();

            _airDefenseService = new AirDefenseService(App.ConnectionManager!, App.DatabaseHelper!);
            _aircraftService = new AircraftService(App.ConnectionManager!, App.DatabaseHelper!);

            LoadAircrafts();
            LoadAirDefenseSystems();
        }

        private void LoadAircrafts()
        {
            var aircrafts = _aircraftService.GetAllAircrafts();
            if (aircrafts != null && aircrafts.Any())
            {
                foreach (var mAircraft in aircrafts)
                {
                    var aircraft = new Aircraft
                    {
                        Id = Convert.ToInt32(mAircraft["Id"].ToString()),
                        Name = mAircraft["Name"].ToString(),
                        AircraftType = (AircraftType)Enum.Parse(typeof(AircraftType), mAircraft["AircraftType"].ToString()),
                        Speed = Convert.ToDouble(mAircraft["Speed"]),
                        Range = Convert.ToDouble(mAircraft["Range"]),
                        MaxAltitude = Convert.ToDouble(mAircraft["MaxAltitude"]),
                        Maneuverability = (Maneuverability)Enum.Parse(typeof(Maneuverability), mAircraft["Maneuverability"].ToString()),
                        PayloadCapacity = Convert.ToDouble(mAircraft["PayloadCapacity"]),
                        Cost = Convert.ToDouble(mAircraft["Cost"]),
                        Munitions = new List<AircraftMunition>(),
                        Radar = null
                    };

                    // Mühimmatları veritabanından çekip ekleyelim
                    var munitions = _aircraftService.GetAircraftMunitions(Convert.ToInt32(mAircraft["Id"]));
                    foreach (var munition in munitions)
                    {
                        var aircraftMunition = new AircraftMunition
                        {
                            Munition = new Munition
                            {
                                Id = Convert.ToInt32(munition["MunitionId"].ToString()),  // Sorgudan gelen MunitionId anahtarı
                                Name = munition["MunitionName"].ToString(),  // Sorgudan gelen MunitionName anahtarı
                                MunitionType = (MunitionType)Enum.Parse(typeof(MunitionType), munition["MunitionType"].ToString()),
                                Weight = Convert.ToDouble(munition["Weight"].ToString()),
                                Speed = Convert.ToDouble(munition["Speed"].ToString()),
                                Range = Convert.ToDouble(munition["Range"].ToString()),
                                Maneuverability = (Maneuverability)Enum.Parse(typeof(Maneuverability), munition["Maneuverability"].ToString()),
                                ExplosivePower = Convert.ToDouble(munition["ExplosivePower"].ToString()),
                                Cost = Convert.ToDouble(munition["Cost"].ToString())
                            },
                            Quantity = Convert.ToInt32(munition["Quantity"])  // Sorgudan gelen Quantity anahtarı
                        };

                        aircraft.Munitions.Add(aircraftMunition);
                    }

                    var radar = _aircraftService.GetAircraftRadar(Convert.ToInt32(mAircraft["Id"]));

                    if (radar != null)
                    {
                        aircraft.Radar = new Radar
                        {
                            Id = Convert.ToInt32(radar["Id"]),
                            Name = radar["RadarName"].ToString(),
                            RadarType = (RadarType)Enum.Parse(typeof(RadarType), radar["RadarType"].ToString()),
                            MinDetectionRange = Convert.ToDouble(radar["MinDetectionRange"]),
                            MaxDetectionRange = Convert.ToDouble(radar["MaxDetectionRange"]),
                            MinAltitude = Convert.ToInt32(radar["MinAltitude"]),
                            MaxAltitude = Convert.ToInt32(radar["MaxAltitude"]),
                            MaxTargetSpeed = Convert.ToInt32(radar["MaxTargetSpeed"]),
                            MaxTargetVelocity = Convert.ToInt32(radar["MaxTargetVelocity"]),
                            RedeploymentTime = Convert.ToInt32(radar["RedeploymentTime"])
                        };
                    }

                    //// Radarları veritabanından çekip ekleyelim
                    //var radars = _aircraftService.GetAircraftRadars(Convert.ToInt32(mAircraft["Id"]));
                    //foreach (var radar in radars)
                    //{
                    //    aircraft.Radar = new Radar
                    //    {
                    //        Id = Convert.ToInt32(radar["Id"]),
                    //        Name = radar["Name"].ToString(),
                    //        RadarType = (RadarType)Enum.Parse(typeof(RadarType), radar["RadarType"].ToString()),
                    //        MinDetectionRange = Convert.ToDouble(radar["MinDetectionRange"]),
                    //        MaxDetectionRange = Convert.ToDouble(radar["MaxDetectionRange"]),
                    //        MinAltitude = Convert.ToInt32(radar["MinAltitude"]),
                    //        MaxAltitude = Convert.ToInt32(radar["MaxAltitude"]),
                    //        MaxTargetSpeed = Convert.ToInt32(radar["MaxTargetSpeed"]),
                    //        MaxTargetVelocity = Convert.ToInt32(radar["MaxTargetVelocity"]),
                    //        RedeploymentTime = Convert.ToInt32(radar["RedeploymentTime"])
                    //    };
                    //}

                    // Uçağı global listeye ekle
                    _aircraftList.Add(aircraft);
                }
            }
        }

        // Veritabanından AirDefenseSystem'leri çekip global listeye ekleyelim
        private void LoadAirDefenseSystems()
        {
            var airDefenseSystems = _airDefenseService.GetAllAirDefenseSystems();
            if (airDefenseSystems != null && airDefenseSystems.Any())
            {
                foreach (var mAirDefense in airDefenseSystems)
                {
                    var airDefense = new AirDefense
                    {
                        Name = mAirDefense["Name"].ToString(),
                        AerodynamicTargetRangeMax = Convert.ToDouble(mAirDefense["AerodynamicTargetRangeMax"]),
                        BallisticTargetRangeMax = Convert.ToDouble(mAirDefense["BallisticTargetRangeMax"]),
                        MaxEngagements = Convert.ToInt32(mAirDefense["MaxEngagements"]),
                        MaxMissilesFired = Convert.ToInt32(mAirDefense["MaxMissilesFired"]),
                        ECMCapability = (ECMCapability)Enum.Parse(typeof(ECMCapability), mAirDefense["ECMCapability"].ToString()),
                        Cost = Convert.ToDouble(mAirDefense["Cost"]),
                        Radars = new List<AirDefenseRadar>(),
                        Munitions = new List<AirDefenseMunition>()
                    };

                    // Radarları veritabanından çekip ekleyelim
                    var radars = _airDefenseService.GetAirDefenseRadars(Convert.ToInt32(mAirDefense["Id"]));
                    foreach (var radar in radars)
                    {
                        var airDefenseRadar = new AirDefenseRadar
                        {
                            Radar = new Radar
                            {
                                Name = radar["RadarName"].ToString(),
                                MinDetectionRange = Convert.ToDouble(radar["MinDetectionRange"]),
                                MaxDetectionRange = Convert.ToDouble(radar["MaxDetectionRange"]),
                                MinAltitude = Convert.ToInt32(radar["MinAltitude"]),
                                MaxAltitude = Convert.ToInt32(radar["MaxAltitude"]),
                                MaxTargetSpeed = Convert.ToInt32(radar["MaxTargetSpeed"]),
                                RadarType = (RadarType)Enum.Parse(typeof(RadarType), radar["RadarType"].ToString()),
                                MaxTargetVelocity = Convert.ToInt32(radar["MaxTargetVelocity"]),
                                RedeploymentTime = Convert.ToInt32(radar["RedeploymentTime"])
                            },
                            Quantity = Convert.ToInt32(radar["Quantity"]) // Radar adedi
                        };

                        airDefense.Radars.Add(airDefenseRadar); // Doğru türde ekleniyor
                    }

                    // Mühimmatları veritabanından çekip ekleyelim
                    var munitions = _airDefenseService.GetAirDefenseMunitions(Convert.ToInt32(mAirDefense["Id"]));
                    foreach (var munition in munitions)
                    {
                        var airDefenseMunition = new AirDefenseMunition
                        {
                            Munition = new Munition
                            {
                                Name = munition["MunitionName"].ToString(),
                                Weight = Convert.ToDouble(munition["Weight"]),
                                Speed = Convert.ToDouble(munition["Speed"]),
                                Range = Convert.ToDouble(munition["Range"]),
                                ExplosivePower = Convert.ToDouble(munition["ExplosivePower"])
                            },
                            Quantity = Convert.ToInt32(munition["Quantity"]) // Mühimmat adedi
                        };

                        airDefense.Munitions.Add(airDefenseMunition); // Doğru türde ekleniyor
                    }

                    // AirDefenseSystem'i global listeye ekle
                    _airDefenseList.Add(airDefense);
                }
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Eklenen Aircraft'ları yazdır
            foreach (Grid threatGrid in ThreatList.Children.OfType<Grid>())
            {
                // Grid içindeki ComboBox'ları ve TextBox'ı bulalım
                var aircraftComboBox = threatGrid.Children.OfType<ComboBox>().FirstOrDefault(c => c.Width == 180);
                var iffComboBox = threatGrid.Children.OfType<ComboBox>().FirstOrDefault(c => c.Width == 100);
                var locationTextBox = threatGrid.Children.OfType<TextBox>().FirstOrDefault();

                if (aircraftComboBox != null && iffComboBox != null && locationTextBox != null)
                {
                    // Seçilen Aircraft ve IFF modunu al
                    string? selectedAircraft = aircraftComboBox.SelectedItem?.ToString();
                    string? selectedIFF = iffComboBox.SelectedItem?.ToString();
                    string location = locationTextBox.Text;

                    // Uçak bilgilerini çekmek ve aynı zamanda mühimmatları da göstermek için
                    if (!string.IsNullOrEmpty(selectedAircraft))
                    {
                        // Aircraft bilgilerini veritabanından çek
                        var _aircraft = _aircraftService.GetAllAircrafts().FirstOrDefault(a => a["Name"].ToString() == selectedAircraft);

                        if (_aircraft != null)
                        {
                            //var radars = _aircraftService.GetAircraftRadars(Convert.ToInt32(_aircraft["Id"]));
                            //var radarsDetails = radars.Select(r => $"Radar Name: {r["RadarName"]}, Quantity: {r["Quantity"]}").ToList();
                            //var radarsDetails = radars.Select(r => $"\n{r["RadarName"]}, Quantity: {r["Quantity"]}").ToList();

                            var radarsDetails = _aircraftService.GetAircraftRadar(Convert.ToInt32(_aircraft["RadarId"]));

                            // Uçağın mühimmatlarını çek
                            var munitions = _aircraftService.GetAircraftMunitions(Convert.ToInt32(_aircraft["Id"]));
                            var munitionsDetails = munitions.Select(m => $"\n{m["MunitionName"]}, Quantity: {m["Quantity"]}").ToList();

                            // Aircraft bilgilerini yazdır
                            MessageBox.Show($"Aircraft: {_aircraft["Name"]}, " +
                                $"\nAircraft Type: {_aircraft["AircraftType"]}, " +
                                $"\nSpeed: {_aircraft["Speed"]}, " +
                                $"\nRange: {_aircraft["Range"]}, " +
                                $"\nMax Altitude: {_aircraft["MaxAltitude"]}, " +
                                $"\nManeuverability: {_aircraft["Maneuverability"]}, " +
                                $"\nPayloadCapacity: {_aircraft["PayloadCapacity"]}, " +
                                $"\nCost: {_aircraft["Cost"]}, " +
                                $"\nRadar: {radarsDetails["RadarName"]}, " +
                                $"\nMunitions: {string.Join(", ", munitionsDetails)}, " +
                                $"\nIFF: {selectedIFF}, " +
                                $"\nLocation: {location}");
                        }
                        else
                        {
                            MessageBox.Show("Aircraft details not found.");
                        }
                    }
                }
            }

            // Eklenen Air Defense Systems'ları yazdır
            foreach (Grid defenseGrid in DefenseList.Children.OfType<Grid>())
            {
                // Grid içindeki ComboBox ve TextBox'ı bulalım
                var defenseComboBox = defenseGrid.Children.OfType<ComboBox>().FirstOrDefault();
                var locationTextBox = defenseGrid.Children.OfType<TextBox>().FirstOrDefault();

                if (defenseComboBox != null && locationTextBox != null)
                {
                    // Seçilen Air Defense System'i al
                    string? selectedAirDefenseSystem = defenseComboBox.SelectedItem?.ToString();
                    string location = locationTextBox.Text;

                    if (!string.IsNullOrEmpty(selectedAirDefenseSystem))
                    {
                        // Air Defense System bilgilerini veritabanından çek
                        var airDefenseDetails = _airDefenseService.GetAllAirDefenseSystems().FirstOrDefault(a => a["Name"].ToString() == selectedAirDefenseSystem);

                        if (airDefenseDetails != null)
                        {
                            // Air Defense System'in radar ve mühimmatlarını çek
                            var radars = _airDefenseService.GetAirDefenseRadars(Convert.ToInt32(airDefenseDetails["Id"]));
                            var radarsDetails = radars.Select(r => $"Radar Name: {r["RadarName"]}, Quantity: {r["Quantity"]}").ToList();

                            var munitions = _airDefenseService.GetAirDefenseMunitions(Convert.ToInt32(airDefenseDetails["Id"]));
                            var munitionsDetails = munitions.Select(m => $"Munition Name: {m["MunitionName"]}, Quantity: {m["Quantity"]}").ToList();

                            // Air Defense System bilgilerini yazdır
                            MessageBox.Show($"Air Defense System: {airDefenseDetails["Name"]}, " +
                                $"\nAerodynamic Range: {airDefenseDetails["AerodynamicTargetRangeMax"]}, " +
                                $"\nBallistic Range: {airDefenseDetails["BallisticTargetRangeMax"]}, " +
                                $"\nMax Engagements: {airDefenseDetails["MaxEngagements"]}, " +
                                $"\nMax Missiles Fired: {airDefenseDetails["MaxMissilesFired"]}, " +
                                $"\nECM Capability: {airDefenseDetails["ECMCapability"]}, " +
                                $"\nCost: {airDefenseDetails["Cost"]}, " +
                                $"\nRadars: {string.Join(", ", radarsDetails)}, " +
                                $"\nMunitions: {string.Join(", ", munitionsDetails)}, " +
                                $"\nLocation: {location}");
                        }
                        else
                        {
                            MessageBox.Show("Air Defense System details not found.");
                        }
                    }
                }
            }
        }
    }
}
