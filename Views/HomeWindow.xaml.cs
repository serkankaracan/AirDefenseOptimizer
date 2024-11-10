using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Fuzzification;
using AirDefenseOptimizer.FuzzyCalculator;
using AirDefenseOptimizer.FuzzyRules;
using AirDefenseOptimizer.Helpers;
using AirDefenseOptimizer.Models;
using AirDefenseOptimizer.Services;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AirDefenseOptimizer.Views
{
    public partial class HomeWindow : UserControl
    {
        private readonly AirDefenseService _airDefenseService;
        private readonly AircraftService _aircraftService;

        private List<AircraftInput> _aircraftThreats = new List<AircraftInput>();
        private List<AirDefenseInput> _airDefenseSystems = new List<AirDefenseInput>();

        List<ThreatDetail> threatDetails = new List<ThreatDetail>();

        public HomeWindow()
        {
            InitializeComponent();

            _airDefenseService = new AirDefenseService(App.ConnectionManager!, App.DatabaseHelper!);
            _aircraftService = new AircraftService(App.ConnectionManager!, App.DatabaseHelper!);

            // Combobox'ları doldurmak için
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
                        Name = mAircraft["Name"].ToString() ?? "",
                        AircraftType = (AircraftType)Enum.Parse(typeof(AircraftType), mAircraft["AircraftType"].ToString() ?? ""),
                        Speed = Convert.ToDouble(mAircraft["Speed"]),
                        Range = Convert.ToDouble(mAircraft["Range"]),
                        MaxAltitude = Convert.ToDouble(mAircraft["MaxAltitude"]),
                        Maneuverability = (Maneuverability)Enum.Parse(typeof(Maneuverability), mAircraft["Maneuverability"].ToString() ?? ""),
                        PayloadCapacity = Convert.ToDouble(mAircraft["PayloadCapacity"]),
                        RadarCrossSection = Convert.ToDouble(mAircraft["RadarCrossSection"]),
                        ECMCapability = (ECMCapability)Enum.Parse(typeof(ECMCapability), mAircraft["ECMCapability"].ToString() ?? ""),
                        Cost = Convert.ToDouble(mAircraft["Cost"]),
                        Munitions = new List<AircraftMunition>(),
                        Radar = null
                    };

                    // Mühimmatları veritabanından çekip ekle
                    var munitions = _aircraftService.GetAircraftMunitions(Convert.ToInt32(mAircraft["Id"]));
                    foreach (var munition in munitions)
                    {
                        var aircraftMunition = new AircraftMunition
                        {
                            Munition = new Munition
                            {
                                Id = Convert.ToInt32(munition["MunitionId"].ToString()),  // Sorgudan gelen MunitionId anahtarı
                                Name = munition["MunitionName"].ToString() ?? "",  // Sorgudan gelen MunitionName anahtarı
                                MunitionType = (MunitionType)Enum.Parse(typeof(MunitionType), munition["MunitionType"].ToString() ?? ""),
                                Weight = Convert.ToDouble(munition["Weight"].ToString()),
                                Speed = Convert.ToDouble(munition["Speed"].ToString()),
                                Range = Convert.ToDouble(munition["Range"].ToString()),
                                Maneuverability = (Maneuverability)Enum.Parse(typeof(Maneuverability), munition["Maneuverability"].ToString() ?? ""),
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
                            Name = radar["RadarName"].ToString() ?? "",
                            RadarType = (RadarType)Enum.Parse(typeof(RadarType), radar["RadarType"].ToString() ?? ""),
                            MaxDetectionTargets = Convert.ToInt32(radar["MaxDetectionTargets"]),
                            MaxTrackingTargets = Convert.ToInt32(radar["MaxTrackingTargets"]),
                            MinDetectionRange = Convert.ToDouble(radar["MinDetectionRange"]),
                            MaxDetectionRange = Convert.ToDouble(radar["MaxDetectionRange"]),
                            MinAltitude = Convert.ToInt32(radar["MinAltitude"]),
                            MaxAltitude = Convert.ToInt32(radar["MaxAltitude"]),
                            MaxTargetSpeed = Convert.ToInt32(radar["MaxTargetSpeed"]),
                            MaxTargetVelocity = Convert.ToInt32(radar["MaxTargetVelocity"]),
                            RedeploymentTime = Convert.ToInt32(radar["RedeploymentTime"])
                        };
                    }
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
                        Id = Convert.ToInt32(mAirDefense["Id"].ToString()),
                        Name = mAirDefense["Name"].ToString() ?? "",
                        AerodynamicTargetRangeMax = Convert.ToDouble(mAirDefense["AerodynamicTargetRangeMax"]),
                        BallisticTargetRangeMax = Convert.ToDouble(mAirDefense["BallisticTargetRangeMax"]),
                        MaxEngagements = Convert.ToInt32(mAirDefense["MaxEngagements"]),
                        MaxMissilesFired = Convert.ToInt32(mAirDefense["MaxMissilesFired"]),
                        ECMCapability = (ECMCapability)Enum.Parse(typeof(ECMCapability), mAirDefense["ECMCapability"].ToString() ?? ""),
                        Cost = Convert.ToDouble(mAirDefense["Cost"]),
                        Radars = new List<AirDefenseRadar>(),
                        Munitions = new List<AirDefenseMunition>()
                    };

                    // Radarları veritabanından çekip ekle
                    var radars = _airDefenseService.GetAirDefenseRadars(Convert.ToInt32(mAirDefense["Id"]));
                    foreach (var radar in radars)
                    {
                        var airDefenseRadar = new AirDefenseRadar
                        {
                            Radar = new Radar
                            {
                                Id = Convert.ToInt32(radar["RadarId"]),
                                Name = radar["RadarName"].ToString() ?? "",
                                RadarType = (RadarType)Enum.Parse(typeof(RadarType), radar["RadarType"].ToString() ?? ""),
                                MaxDetectionTargets = Convert.ToInt32(radar["MaxDetectionTargets"]),
                                MaxTrackingTargets = Convert.ToInt32(radar["MaxTrackingTargets"]),
                                MinDetectionRange = Convert.ToDouble(radar["MinDetectionRange"]),
                                MaxDetectionRange = Convert.ToDouble(radar["MaxDetectionRange"]),
                                MinAltitude = Convert.ToInt32(radar["MinAltitude"]),
                                MaxAltitude = Convert.ToInt32(radar["MaxAltitude"]),
                                MaxTargetSpeed = Convert.ToInt32(radar["MaxTargetSpeed"]),
                                MaxTargetVelocity = Convert.ToInt32(radar["MaxTargetVelocity"]),
                                RedeploymentTime = Convert.ToInt32(radar["RedeploymentTime"])
                            },
                            Quantity = Convert.ToInt32(radar["Quantity"]) // Radar adedi
                        };

                        airDefense.Radars.Add(airDefenseRadar); // Doğru türde ekleniyor
                    }

                    // Mühimmatları veritabanından çekip ekle
                    var munitions = _airDefenseService.GetAirDefenseMunitions(Convert.ToInt32(mAirDefense["Id"]));
                    foreach (var munition in munitions)
                    {
                        var airDefenseMunition = new AirDefenseMunition
                        {
                            Munition = new Munition
                            {
                                Id = Convert.ToInt32(munition["MunitionId"].ToString()),  // Sorgudan gelen MunitionId anahtarı
                                Name = munition["MunitionName"].ToString() ?? "",  // Sorgudan gelen MunitionName anahtarı
                                MunitionType = (MunitionType)Enum.Parse(typeof(MunitionType), munition["MunitionType"].ToString() ?? ""),
                                Weight = Convert.ToDouble(munition["Weight"].ToString()),
                                Speed = Convert.ToDouble(munition["Speed"].ToString()),
                                Range = Convert.ToDouble(munition["Range"].ToString()),
                                Maneuverability = (Maneuverability)Enum.Parse(typeof(Maneuverability), munition["Maneuverability"].ToString() ?? ""),
                                ExplosivePower = Convert.ToDouble(munition["ExplosivePower"].ToString()),
                                Cost = Convert.ToDouble(munition["Cost"].ToString())
                            },
                            Quantity = Convert.ToInt32(munition["Quantity"]) // Mühimmat adedi
                        };

                        airDefense.Munitions.Add(airDefenseMunition); // Doğru türde ekleniyor
                    }

                    // AirDefenseSystem'i global listeye ekle
                    //_airDefenseList.Add(airDefense);
                }
            }
        }

        // Uçak Tehdidi Ekleme Butonuna Tıklanınca Çalışacak
        private void AddAircraftThreat_Click(object sender, RoutedEventArgs e)
        {
            // Eğer daha önce satır eklenmediyse, üst kısma label'lar ekleyelim.
            if (ThreatList.Children.Count == 0)
            {
                StackPanel labelsPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 5, 0, 5)
                };

                labelsPanel.Children.Add(new Label
                {
                    Content = "",
                    Width = 50,
                    Margin = new Thickness(0),
                    Padding = new Thickness(5)
                });

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
                    Width = 160,
                    Margin = new Thickness(0),
                    Padding = new Thickness(5)
                });

                labelsPanel.Children.Add(new Label
                {
                    Content = "Speed",
                    Width = 80,
                    Margin = new Thickness(0),
                    Padding = new Thickness(5)
                });

                ThreatList.Children.Add(labelsPanel);
            }

            // Grid oluşturuyoruz
            Grid threatGrid = new Grid
            {
                Margin = new Thickness(0, 10, 0, 10)
            };

            // Grid sütunlarını düzenliyoruz, genişlikleri sabitliyoruz
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });  // Index için sütun
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(180) });
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(160) });
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
            threatGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

            // Index Label
            Label indexLabel = new Label
            {
                Width = 50,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            // Uçak Seçimi
            ComboBox aircraftComboBox = new ComboBox
            {
                Name = "AircraftComboBox",
                Width = 180,
                Height = 30,
                Margin = new Thickness(0),
                Padding = new Thickness(5)
            };
            aircraftComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Select Aircraft",
                IsEnabled = false,
                IsSelected = true
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
                Name = "IFFComboBox",
                Width = 100,
                Height = 30,
                Margin = new Thickness(0),
                Padding = new Thickness(5)
            };
            iffComboBox.Items.Add(new ComboBoxItem
            {
                Content = "Select IFF",
                IsEnabled = false,
                IsSelected = true
            });

            var iffValues = Enum.GetValues(typeof(IFF));
            foreach (var iff in iffValues)
            {
                iffComboBox.Items.Add(iff);
            }

            // Konum Girdisi (Latitude, Longitude, Altitude)
            TextBox locationTextBox = new TextBox
            {
                Name = "LocationTextBox",
                Width = 160,
                Margin = new Thickness(0),
                Padding = new Thickness(5),
                MaxLength = 25
            };

            // Speed TextBox'ı
            TextBox speedTextBox = new TextBox
            {
                Name = "SpeedTextBox",
                Width = 80,
                Margin = new Thickness(0),
                Padding = new Thickness(5),
                MaxLength = 10
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
                if (ThreatList.Children.Count == 1)
                {
                    ThreatList.Children.Clear();
                }
                UpdateIndices(ThreatList); // Kaldırma işleminden sonra indeksleri güncelle
            };

            // Grid'e elemanları ekliyoruz
            threatGrid.Children.Add(indexLabel);
            Grid.SetColumn(indexLabel, 0);

            threatGrid.Children.Add(aircraftComboBox);
            Grid.SetColumn(aircraftComboBox, 1);

            threatGrid.Children.Add(iffComboBox);
            Grid.SetColumn(iffComboBox, 2);

            threatGrid.Children.Add(locationTextBox);
            Grid.SetColumn(locationTextBox, 3);

            threatGrid.Children.Add(speedTextBox);
            Grid.SetColumn(speedTextBox, 4);

            threatGrid.Children.Add(removeButton);
            Grid.SetColumn(removeButton, 5);

            // Tehdit Listesine ekliyoruz
            ThreatList.Children.Add(threatGrid);

            UpdateIndices(ThreatList); // Ekledikten sonra tüm indeksleri güncelle
        }

        // İndeksleri güncelleyen metot
        private void UpdateIndices(Panel list)
        {
            int index = 1;
            foreach (var child in list.Children)
            {
                if (child is Grid grid && grid.Children[0] is Label indexLabel)
                {
                    indexLabel.Content = index.ToString();
                    index++;
                }
            }
        }

        // Hava Savunma Sistemi Ekleme Butonuna Tıklanınca Çalışacak
        private int defenseIndex = 1; // Sayaç ekleyelim

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
                    Content = "Index",
                    Width = 50,
                    Margin = new Thickness(0),
                    Padding = new Thickness(5)
                });

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

            // Grid sütunları: Index, Air Defense System, Location, Remove
            defenseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
            defenseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(180) });
            defenseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(160) });
            defenseGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

            // Index Label
            Label indexLabel = new Label
            {
                Content = defenseIndex.ToString(), // Sayacı Label içeriğine ekle
                Width = 50,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            // Hava Savunma Seçimi
            ComboBox defenseComboBox = new ComboBox
            {
                Width = 180,
                Height = 30,
                Margin = new Thickness(0),
                Padding = new Thickness(5)
            };
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
                UpdateIndices(DefenseList); // Kaldırma işleminden sonra indeksleri güncelle
            };

            // Grid'e elemanları ekliyoruz
            defenseGrid.Children.Add(indexLabel);
            Grid.SetColumn(indexLabel, 0);

            defenseGrid.Children.Add(defenseComboBox);
            Grid.SetColumn(defenseComboBox, 1);

            defenseGrid.Children.Add(locationTextBox);
            Grid.SetColumn(locationTextBox, 2);

            defenseGrid.Children.Add(removeButton);
            Grid.SetColumn(removeButton, 3);

            // Savunma Listesine ekliyoruz
            DefenseList.Children.Add(defenseGrid);

            UpdateIndices(DefenseList); // Ekledikten sonra tüm indeksleri güncelle
        }

        private Position? ParsePosition(string location)
        {
            var locationParts = location.Split(',');
            if (locationParts.Length == 3 &&
                double.TryParse(locationParts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude) &&
                double.TryParse(locationParts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude) &&
                double.TryParse(locationParts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double altitude))
            {
                return new Position(latitude, longitude, altitude);
            }
            return null;
        }

        private Aircraft CreateAircraft(Dictionary<string, object> aircraftData, string selectedAircraft)
        {
            var aircraft = new Aircraft
            {
                Id = Convert.ToInt32(aircraftData["Id"].ToString()),
                Name = selectedAircraft,
                AircraftType = (AircraftType)Enum.Parse(typeof(AircraftType), aircraftData["AircraftType"].ToString() ?? ""),
                Speed = Convert.ToDouble(aircraftData["Speed"]),
                Range = Convert.ToDouble(aircraftData["Range"]),
                MaxAltitude = Convert.ToDouble(aircraftData["MaxAltitude"]),
                Maneuverability = (Maneuverability)Enum.Parse(typeof(Maneuverability), aircraftData["Maneuverability"].ToString() ?? ""),
                PayloadCapacity = Convert.ToDouble(aircraftData["PayloadCapacity"]),
                RadarCrossSection = Convert.ToDouble(aircraftData["RadarCrossSection"]),
                ECMCapability = (ECMCapability)Enum.Parse(typeof(ECMCapability), aircraftData["ECMCapability"].ToString() ?? ""),
                Cost = Convert.ToDouble(aircraftData["Cost"]),
                Munitions = _aircraftService.GetAircraftMunitions(Convert.ToInt32(aircraftData["Id"])).Select(m => new AircraftMunition
                {
                    Munition = new Munition
                    {
                        Id = Convert.ToInt32(m["MunitionId"]),
                        Name = m["MunitionName"].ToString() ?? "",
                        MunitionType = (MunitionType)Enum.Parse(typeof(MunitionType), m["MunitionType"].ToString() ?? ""),
                        Weight = Convert.ToDouble(m["Weight"]),
                        Speed = Convert.ToDouble(m["Speed"]),
                        Range = Convert.ToDouble(m["Range"]),
                        Maneuverability = (Maneuverability)Enum.Parse(typeof(Maneuverability), m["Maneuverability"].ToString() ?? ""),
                        ExplosivePower = Convert.ToDouble(m["ExplosivePower"]),
                        Cost = Convert.ToDouble(m["Cost"])
                    },
                    Quantity = Convert.ToInt32(m["Quantity"])
                }).ToList()
            };

            // Radar bilgisini al ve ata
            var radarData = _aircraftService.GetAircraftRadar(Convert.ToInt32(aircraftData["Id"]));
            if (radarData != null)
            {
                aircraft.Radar = new Radar
                {
                    Id = Convert.ToInt32(radarData["Id"]),
                    Name = radarData["RadarName"].ToString() ?? "",
                    RadarType = (RadarType)Enum.Parse(typeof(RadarType), radarData["RadarType"].ToString() ?? ""),
                    MaxDetectionTargets = Convert.ToInt32(radarData["MaxDetectionTargets"]),
                    MaxTrackingTargets = Convert.ToInt32(radarData["MaxTrackingTargets"]),
                    MinDetectionRange = Convert.ToDouble(radarData["MinDetectionRange"]),
                    MaxDetectionRange = Convert.ToDouble(radarData["MaxDetectionRange"]),
                    MinAltitude = Convert.ToInt32(radarData["MinAltitude"]),
                    MaxAltitude = Convert.ToInt32(radarData["MaxAltitude"]),
                    MaxTargetSpeed = Convert.ToInt32(radarData["MaxTargetSpeed"]),
                    MaxTargetVelocity = Convert.ToInt32(radarData["MaxTargetVelocity"]),
                    RedeploymentTime = Convert.ToInt32(radarData["RedeploymentTime"])
                };
            }

            return aircraft;
        }

        private AirDefense CreateAirDefense(Dictionary<string, object> airDefenseData, string selectedAirDefenseSystem)
        {
            return new AirDefense
            {
                Id = Convert.ToInt32(airDefenseData["Id"].ToString()),
                Name = selectedAirDefenseSystem,
                AerodynamicTargetRangeMax = Convert.ToDouble(airDefenseData["AerodynamicTargetRangeMax"]),
                BallisticTargetRangeMax = Convert.ToDouble(airDefenseData["BallisticTargetRangeMax"]),
                MaxEngagements = Convert.ToInt32(airDefenseData["MaxEngagements"]),
                MaxMissilesFired = Convert.ToInt32(airDefenseData["MaxMissilesFired"]),
                ECMCapability = (ECMCapability)Enum.Parse(typeof(ECMCapability), airDefenseData["ECMCapability"].ToString() ?? ""),
                Cost = Convert.ToDouble(airDefenseData["Cost"]),

                Radars = _airDefenseService.GetAirDefenseRadars(Convert.ToInt32(airDefenseData["Id"])).Select(r => new AirDefenseRadar
                {
                    Radar = new Radar
                    {
                        Id = Convert.ToInt32(r["RadarId"]),
                        Name = r["RadarName"].ToString() ?? "",
                        RadarType = (RadarType)Enum.Parse(typeof(RadarType), r["RadarType"].ToString() ?? ""),
                        MaxDetectionTargets = Convert.ToInt32(r["MaxDetectionTargets"]),
                        MaxTrackingTargets = Convert.ToInt32(r["MaxTrackingTargets"]),
                        MinDetectionRange = Convert.ToDouble(r["MinDetectionRange"]),
                        MaxDetectionRange = Convert.ToDouble(r["MaxDetectionRange"]),
                        MinAltitude = Convert.ToInt32(r["MinAltitude"]),
                        MaxAltitude = Convert.ToInt32(r["MaxAltitude"]),
                        MaxTargetSpeed = Convert.ToInt32(r["MaxTargetSpeed"]),
                        MaxTargetVelocity = Convert.ToInt32(r["MaxTargetVelocity"]),
                        RedeploymentTime = Convert.ToInt32(r["RedeploymentTime"])
                    },
                    Quantity = Convert.ToInt32(r["Quantity"])
                }).ToList(),

                Munitions = _airDefenseService.GetAirDefenseMunitions(Convert.ToInt32(airDefenseData["Id"])).Select(m => new AirDefenseMunition
                {
                    Munition = new Munition
                    {
                        Id = Convert.ToInt32(m["MunitionId"]),
                        Name = m["MunitionName"].ToString() ?? "",
                        MunitionType = (MunitionType)Enum.Parse(typeof(MunitionType), m["MunitionType"].ToString() ?? ""),
                        Weight = Convert.ToDouble(m["Weight"]),
                        Speed = Convert.ToDouble(m["Speed"]),
                        Range = Convert.ToDouble(m["Range"]),
                        Maneuverability = (Maneuverability)Enum.Parse(typeof(Maneuverability), m["Maneuverability"].ToString() ?? ""),
                        ExplosivePower = Convert.ToDouble(m["ExplosivePower"]),
                        Cost = Convert.ToDouble(m["Cost"])
                    },
                    Quantity = Convert.ToInt32(m["Quantity"])
                }).ToList()
            };
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            var aircraft = new Aircraft
            {
                Speed = 1200, // Hız (örneğin 1200 km/h)
                Range = 2000, // Menzil (örneğin 2000 km)
                MaxAltitude = 15000, // İrtifa (örneğin 15.000 m)
                Maneuverability = Maneuverability.High, // Manevra kabiliyeti
                ECMCapability = ECMCapability.Basic, // Elektronik karşı önlem kabiliyeti
                PayloadCapacity = 1000, // Yük kapasitesi
                RadarCrossSection = 3, // Radar kesit alanı
                Cost = 150000 // Maliyet
            };

            // 2. FuzzyAircraft nesnesi oluşturun
            var fuzzyAircraft = new FuzzyAircraft();

            // 3. Tehdit seviyesini hesaplayın
            double threatLevel = fuzzyAircraft.CalculateThreatLevel(aircraft);

            // 4. Sonucu gösterin
            MessageBox.Show($"Calculated Threat Level: {threatLevel}");
        }

        private void ShowThreatLevelButton_Click(object sender, RoutedEventArgs e)
        {
            AircraftRules aircraftRules = new AircraftRules();

            // Örnek konum tanımla
            Position sourcePosition = new Position(
                double.Parse(LatitudeTextBox.Text, CultureInfo.InvariantCulture),
                double.Parse(LongitudeTextBox.Text, CultureInfo.InvariantCulture),
                double.Parse(AltitudeTextBox.Text, CultureInfo.InvariantCulture));

            // Aircraft verilerini listeye ekleyelim
            _aircraftThreats.Clear(); // Önceki verileri temizleyelim
            foreach (Grid threatGrid in ThreatList.Children.OfType<Grid>())
            {
                // Grid içindeki ComboBox'ları ve TextBox'ı bulalım
                var aircraftComboBox = threatGrid.Children.OfType<ComboBox>().FirstOrDefault(c => c.Name == "AircraftComboBox");
                var iffComboBox = threatGrid.Children.OfType<ComboBox>().FirstOrDefault(c => c.Name == "IFFComboBox");
                var locationTextBox = threatGrid.Children.OfType<TextBox>().FirstOrDefault(c => c.Name == "LocationTextBox");
                var speedTextBox = threatGrid.Children.OfType<TextBox>().FirstOrDefault(c => c.Name == "SpeedTextBox");

                IFF selectedIFF = IFF.Unknown;

                if (aircraftComboBox != null && iffComboBox != null && locationTextBox != null && speedTextBox != null)
                {
                    // Seçilen Aircraft ve IFF modunu al
                    string? selectedAircraft = aircraftComboBox.SelectedItem?.ToString();
                    string? stringIFF = iffComboBox.SelectedItem?.ToString();
                    string stringSpeed = speedTextBox.Text;

                    if (stringIFF != null && Enum.TryParse<IFF>(stringIFF, out var parsedIFF))
                        selectedIFF = parsedIFF;

                    string location = locationTextBox.Text;
                    string speed = speedTextBox.Text;

                    if (!string.IsNullOrEmpty(selectedAircraft) &&
                        !string.IsNullOrEmpty(location) &&
                        !string.IsNullOrEmpty(speed))
                    {
                        // Aircraft bilgilerini veritabanından çek
                        var aircraftData = _aircraftService.GetAllAircrafts().FirstOrDefault(a => a["Name"].ToString() == selectedAircraft);

                        if (aircraftData != null)
                        {
                            // Aircraft nesnesini oluştur
                            var aircraft = CreateAircraft(aircraftData, selectedAircraft);

                            Position? userPosition = ParsePosition(location);
                            if (userPosition != null)
                            {
                                // Mesafeyi hesapla
                                double distance = Position.CalculateDistance(sourcePosition, userPosition);

                                // Tehdit seviyesini hesapla
                                var threatCalculator = new AircraftThreatCalculator();
                                double speedValue = double.TryParse(speed, out var parsedSpeed) ? parsedSpeed : 0;
                                double threatLevel = threatCalculator.CalculateThreatLevel(aircraft, selectedIFF, distance, speedValue);

                                // Mesaj göster
                                MessageBox.Show($"Tehdit: {aircraft.Name}\n" +
                                                $"Latitude: {userPosition.Latitude}, Longitude: {userPosition.Longitude}, Altitude: {userPosition.Altitude}\n" +
                                                $"Hedefe olan mesafe: {distance:F2} km\n" +
                                                $"IFF mod: {selectedIFF}\n" +
                                                $"Tehdit Seviyesi: {threatLevel}");

                                // AircraftInput nesnesini oluştur ve listeye ekle
                                _aircraftThreats.Add(new AircraftInput(aircraft, selectedIFF, speedValue, location, distance));

                                // Tehdit detaylarını güncelle
                                threatDetails.Add(new ThreatDetail
                                {
                                    Aircraft = aircraft,
                                    IFFMode = selectedIFF,
                                    Speed = speedValue,
                                    Location = location,
                                    Distance = distance,
                                    ThreatLevel = threatLevel.ToString()
                                });
                            }
                            else
                            {
                                MessageBox.Show("Geçersiz konum formatı.");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Aircraft details not found for: {selectedAircraft}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tüm alanları doldurun.");
                        return;
                    }
                }
            }

            // Air Defense verilerini listeye ekleyelim
            _airDefenseSystems.Clear(); // Önceki verileri temizleyelim
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

                    if (!string.IsNullOrEmpty(selectedAirDefenseSystem) && !string.IsNullOrEmpty(location))
                    {
                        // Air Defense bilgilerini veritabanından çek
                        var airDefenseData = _airDefenseService.GetAllAirDefenseSystems().FirstOrDefault(a => a["Name"].ToString() == selectedAirDefenseSystem);

                        if (airDefenseData != null)
                        {
                            // AirDefense nesnesini oluştur
                            var airDefense = CreateAirDefense(airDefenseData, selectedAirDefenseSystem);

                            // AirDefenseInput nesnesini oluştur ve listeye ekle
                            _airDefenseSystems.Add(new AirDefenseInput(airDefense, location));
                        }
                        else
                        {
                            MessageBox.Show($"Air Defense System details not found for: {selectedAirDefenseSystem}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tüm alanları doldurun.");
                        return;
                    }
                }
            }

            threatDetails.Clear();

            // Listeye eklenen Aircraft ve Air Defense System'ler üzerinden işlem yapalım
            foreach (var aircraftInput in _aircraftThreats)
            {
                // Tehdit skorunu hesapla
                var (threatLevel, totalScore) = aircraftRules.CalculateThreatScore(aircraftInput.Aircraft);

                threatDetails.Add(new ThreatDetail
                {
                    Aircraft = aircraftInput.Aircraft,
                    IFFMode = aircraftInput.IFFMode,
                    Speed = aircraftInput.Speed,
                    Location = aircraftInput.Location,
                    Distance = aircraftInput.Distance,
                    ThreatScore = totalScore,
                    ThreatLevel = threatLevel
                });

            }

            foreach (var airDefenseInput in _airDefenseSystems)
            {
                // Hava Savunma Sistemi Konum Bilgisi
                Position? defensePosition = ParsePosition(airDefenseInput.Location);
                if (defensePosition != null)
                {
                    foreach (var aircraftInput in _aircraftThreats)
                    {
                        Position? aircraftPosition = ParsePosition(aircraftInput.Location);
                        if (aircraftPosition != null)
                        {
                            // Mesafeyi hesapla
                            double distance = Position.CalculateDistance(defensePosition, aircraftPosition);

                            // Mesaj göster
                            MessageBox.Show(
                                $"Air Defense System: {airDefenseInput.AirDefense.Name}\n" +
                                $"Location: {defensePosition.Latitude}, {defensePosition.Longitude}, {defensePosition.Altitude}\n" +
                                $"Target Aircraft: {aircraftInput.Aircraft.Name}\n" +
                                $"Distance to Aircraft: {distance:F2} km");
                        }
                        else
                        {
                            MessageBox.Show($"Invalid location format for aircraft: {aircraftInput.Aircraft.Name}");
                        }
                    }
                }
                else
                {
                    MessageBox.Show($"Invalid location format for air defense system: {airDefenseInput.AirDefense.Name}");
                }
            }

            ThreatDetailsWindow threatDetailsWindow = new ThreatDetailsWindow(threatDetails.Select((detail, index) =>
            {
                detail.Index = index + 1;
                return detail;
            }).ToList());
            threatDetailsWindow.Show();
        }
    }
}
