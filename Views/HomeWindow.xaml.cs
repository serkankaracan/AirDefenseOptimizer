using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AirDefenseOptimizer.Enums;
using AirDefenseOptimizer.Fuzzification;
using AirDefenseOptimizer.FuzzyCalculator;
using AirDefenseOptimizer.Helpers;
using AirDefenseOptimizer.Models;
using AirDefenseOptimizer.Services;

namespace AirDefenseOptimizer.Views
{
    public partial class HomeWindow : UserControl
    {
        private readonly AirDefenseService _airDefenseService;
        private readonly AircraftService _aircraftService;

        private List<AircraftInput> _aircraftThreats = new List<AircraftInput>();
        private List<AirDefenseInput> _airDefenseSystems = new List<AirDefenseInput>();
        private List<ThreatDetail> threatDetails = new List<ThreatDetail>();

        private int defenseIndex = 1;

        private static readonly Random rand = new Random();

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
        private void AddLabelsToPanel(StackPanel panel, string[] labels, int[] widths)
        {
            for (int i = 0; i < labels.Length; i++)
            {
                panel.Children.Add(new Label
                {
                    Content = labels[i],
                    Width = widths[i],
                    Margin = new Thickness(0),
                    Padding = new Thickness(5)
                });
            }
        }
        private Grid CreateGridWithColumns(int[] columnWidths)
        {
            var grid = new Grid { Margin = new Thickness(0, 10, 0, 10) };
            foreach (var width in columnWidths)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width) });
            }
            return grid;
        }
        private void PopulateComboBox(ComboBox comboBox, IEnumerable<string> items, string placeholder)
        {
            comboBox.Items.Add(new ComboBoxItem
            {
                Content = placeholder,
                IsEnabled = false,
                IsSelected = true
            });

            foreach (var item in items)
            {
                comboBox.Items.Add(item);
            }
        }
        private void AddAircraftThreat_Click(object sender, RoutedEventArgs e)
        {
            if (ThreatList.Children.Count == 0)
            {
                var labelsPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
                AddLabelsToPanel(labelsPanel,
                    new[] { "", "Aircraft:", "IFF Mode:", "Location (Latitude, Longitude, Altitude)", "Speed" },
                    new[] { 50, 180, 100, 160, 80 });
                ThreatList.Children.Add(labelsPanel);
            }

            var threatGrid = CreateGridWithColumns(new[] { 50, 180, 100, 160, 80, 80 });

            // Index Label
            var indexLabel = new Label
            {
                Width = 50,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            // Aircraft ComboBox
            var aircraftComboBox = new ComboBox { Name = "AircraftComboBox", Width = 180, Height = 30 };
            var aircrafts = _aircraftService.GetAllAircrafts().Select(a => a["Name"].ToString());
            PopulateComboBox(aircraftComboBox, aircrafts, "Select Aircraft");

            // IFF ComboBox
            var iffComboBox = new ComboBox { Name = "IFFComboBox", Width = 100, Height = 30 };
            var iffValues = Enum.GetValues(typeof(IFF)).Cast<IFF>().Select(i => i.ToString());
            PopulateComboBox(iffComboBox, iffValues, "Select IFF");

            // Location TextBox
            var locationTextBox = new TextBox
            {
                Name = "LocationTextBox",
                Width = 160,
                Text = GenerateRandomLocation(),
                MaxLength = 50
            };

            // Speed TextBox
            var speedTextBox = new TextBox
            {
                Name = "SpeedTextBox",
                Width = 80,
                MaxLength = 10
            };

            // Remove Button
            var removeButton = CreateRemoveButton(threatGrid, ThreatList);

            // Add elements to grid
            AddElementsToGrid(threatGrid, new UIElement[] { indexLabel, aircraftComboBox, iffComboBox, locationTextBox, speedTextBox, removeButton });

            // Add grid to list
            ThreatList.Children.Add(threatGrid);
            UpdateIndices(ThreatList);
        }
        private void AddAirDefenseSystem_Click(object sender, RoutedEventArgs e)
        {
            if (DefenseList.Children.Count == 0)
            {
                var labelsPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
                AddLabelsToPanel(labelsPanel,
                    new[] { "Index", "Air Defense System", "Location (Latitude, Longitude, Altitude)" },
                    new[] { 50, 180, 240 });
                DefenseList.Children.Add(labelsPanel);
            }

            var defenseGrid = CreateGridWithColumns(new[] { 50, 180, 160, 80 });

            // Index Label
            var indexLabel = new Label
            {
                Content = (DefenseList.Children.Count).ToString(),
                Width = 50,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            // Defense ComboBox
            var defenseComboBox = new ComboBox { Width = 180, Height = 30 };
            var airDefenseSystems = _airDefenseService.GetAllAirDefenseSystems().Select(d => d["Name"].ToString());
            PopulateComboBox(defenseComboBox, airDefenseSystems, "Air Defense System");

            // Location TextBox
            var locationTextBox = new TextBox
            {
                Width = 160,
                Text = GenerateRandomLocation(),
                MaxLength = 25
            };

            // Remove Button
            var removeButton = CreateRemoveButton(defenseGrid, DefenseList);

            // Add elements to grid
            AddElementsToGrid(defenseGrid, new UIElement[] { indexLabel, defenseComboBox, locationTextBox, removeButton });

            // Add grid to list
            DefenseList.Children.Add(defenseGrid);
            UpdateIndices(DefenseList);
        }
        private Button CreateRemoveButton(Grid grid, StackPanel list)
        {
            var removeButton = new Button
            {
                Content = "Remove",
                Width = 80,
                Height = 30,
                Background = new SolidColorBrush(System.Windows.Media.Colors.IndianRed),
                Foreground = new SolidColorBrush(System.Windows.Media.Colors.White)
            };
            removeButton.Click += (s, e) =>
            {
                list.Children.Remove(grid);
                if (list.Children.Count == 1)
                    list.Children.Clear();
                UpdateIndices(list);
            };
            return removeButton;
        }
        private void AddElementsToGrid(Grid grid, UIElement[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                grid.Children.Add(elements[i]);
                Grid.SetColumn(elements[i], i);
            }
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
            ClearPreviousData();
            ProcessAircraftThreats();
            ProcessAirDefenseSystems();
            List<Radar> detectedRadarList = DetectAircraftByRadars();
            UpdateThreatDetails(detectedRadarList);
            AssignAirDefenseSystemsToThreats();
            ShowThreatDetailsWindow();
        }
        private void ClearPreviousData()
        {
            _aircraftThreats.Clear();
            _airDefenseSystems.Clear();
            threatDetails.Clear();
        }
        private void ProcessAircraftThreats()
        {
            foreach (Grid threatGrid in ThreatList.Children.OfType<Grid>())
            {
                var aircraftComboBox = threatGrid.Children.OfType<ComboBox>().FirstOrDefault(c => c.Name == "AircraftComboBox");
                var iffComboBox = threatGrid.Children.OfType<ComboBox>().FirstOrDefault(c => c.Name == "IFFComboBox");
                var locationTextBox = threatGrid.Children.OfType<TextBox>().FirstOrDefault(c => c.Name == "LocationTextBox");
                var speedTextBox = threatGrid.Children.OfType<TextBox>().FirstOrDefault(c => c.Name == "SpeedTextBox");

                if (aircraftComboBox == null || iffComboBox == null || locationTextBox == null || speedTextBox == null)
                    continue;

                ProcessAircraftInput(aircraftComboBox, iffComboBox, locationTextBox, speedTextBox);
            }
        }
        private void ProcessAircraftInput(ComboBox aircraftComboBox, ComboBox iffComboBox, TextBox locationTextBox, TextBox speedTextBox)
        {
            string? selectedAircraft = aircraftComboBox.SelectedItem?.ToString();
            string location = locationTextBox.Text;
            string stringSpeed = speedTextBox.Text;
            string? stringIFF = iffComboBox.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedAircraft) || string.IsNullOrEmpty(location) || string.IsNullOrEmpty(stringSpeed))
            {
                MessageBox.Show("Tüm alanları doldurun.");
                return;
            }

            if (!Enum.TryParse<IFF>(stringIFF, out var selectedIFF))
                selectedIFF = IFF.Unknown;

            var aircraftData = _aircraftService.GetAllAircrafts().FirstOrDefault(a => a["Name"].ToString() == selectedAircraft);

            if (aircraftData == null)
            {
                MessageBox.Show($"Aircraft details not found for: {selectedAircraft}");
                return;
            }

            AddAircraftToThreatList(aircraftData, selectedAircraft, location, stringSpeed, selectedIFF);
        }
        private void AddAircraftToThreatList(dynamic aircraftData, string selectedAircraft, string location, string stringSpeed, IFF selectedIFF)
        {
            var aircraft = CreateAircraft(aircraftData, selectedAircraft);
            Position? aircraftPosition = ParsePosition(location);

            if (aircraftPosition == null)
            {
                MessageBox.Show("Geçersiz konum formatı.");
                return;
            }

            double distance = Position.CalculateDistance(GetSourcePosition(), aircraftPosition);
            double speedValue = double.TryParse(stringSpeed, out var parsedSpeed) ? parsedSpeed : 0;

            var threatCalculator = new AircraftThreatCalculator();
            double threatScore = threatCalculator.CalculateThreatLevel(aircraft, selectedIFF, distance, speedValue, aircraft.Maneuverability, aircraftPosition.Altitude, aircraft.Cost);
            double threatLevel = threatScore;

            _aircraftThreats.Add(new AircraftInput(aircraft, selectedIFF, speedValue, location, distance, threatLevel, threatScore));
        }
        private void ProcessAirDefenseSystems()
        {
            foreach (Grid defenseGrid in DefenseList.Children.OfType<Grid>())
            {
                var defenseComboBox = defenseGrid.Children.OfType<ComboBox>().FirstOrDefault();
                var locationTextBox = defenseGrid.Children.OfType<TextBox>().FirstOrDefault();

                if (defenseComboBox == null || locationTextBox == null)
                    continue;

                ProcessAirDefenseInput(defenseComboBox, locationTextBox);
            }
        }
        private void ProcessAirDefenseInput(ComboBox defenseComboBox, TextBox locationTextBox)
        {
            string? selectedAirDefenseSystem = defenseComboBox.SelectedItem?.ToString();
            string location = locationTextBox.Text;

            if (string.IsNullOrEmpty(selectedAirDefenseSystem) || string.IsNullOrEmpty(location))
            {
                MessageBox.Show("Tüm alanları doldurun.");
                return;
            }

            var airDefenseData = _airDefenseService.GetAllAirDefenseSystems().FirstOrDefault(a => a["Name"].ToString() == selectedAirDefenseSystem);

            if (airDefenseData == null)
            {
                MessageBox.Show($"Detay bulunamadı: {selectedAirDefenseSystem}");
                return;
            }

            var airDefense = CreateAirDefense(airDefenseData, selectedAirDefenseSystem);
            _airDefenseSystems.Add(new AirDefenseInput(airDefense, location));
        }
        private List<Radar> DetectAircraftByRadars()
        {
            var detectedRadarList = new List<Radar>();

            foreach (var airDefenseInput in _airDefenseSystems)
            {
                Position? defensePosition = ParsePosition(airDefenseInput.Location);
                if (defensePosition == null)
                {
                    MessageBox.Show($"Geçersiz konum formatı: {airDefenseInput.AirDefense.Name}");
                    continue;
                }

                foreach (var aircraftInput in _aircraftThreats)
                {
                    Position? aircraftPosition = ParsePosition(aircraftInput.Location);
                    if (aircraftPosition == null)
                    {
                        MessageBox.Show($"Geçersiz konum formatı: {aircraftInput.Aircraft.Name}");
                        continue;
                    }

                    CheckRadarDetection(airDefenseInput, aircraftInput, defensePosition, aircraftPosition, detectedRadarList);
                }
            }

            return detectedRadarList;
        }
        private void CheckRadarDetection(AirDefenseInput airDefenseInput, AircraftInput aircraftInput, Position defensePosition, Position aircraftPosition, List<Radar> detectedRadarList)
        {
            double distance = Position.CalculateDistance(defensePosition, aircraftPosition);

            foreach (var radar in airDefenseInput.AirDefense.Radars)
            {
                if (radar.Radar.MaxDetectionRange > distance)
                {
                    detectedRadarList.Add(radar.Radar);
                    //MessageBox.Show("detected by radar: " + radar.Radar.Name);
                }
            }
        }
        private void UpdateThreatDetails(List<Radar> detectedRadarList)
        {
            foreach (var aircraftInput in _aircraftThreats)
            {
                var threatDetail = new ThreatDetail
                {
                    Aircraft = aircraftInput.Aircraft,
                    IFFMode = aircraftInput.IFFMode,
                    Speed = aircraftInput.Speed,
                    Location = aircraftInput.Location,
                    Distance = aircraftInput.Distance,
                    Altitude = Convert.ToDouble(aircraftInput.Location.Split(',')[2].Trim()),
                    ThreatLevel = GetThreatLevel(aircraftInput.ThreatLevel),
                    ThreatScore = aircraftInput.ThreatScore
                };

                var detectingRadars = GetRadarsDetectingThreat(threatDetail, _airDefenseSystems);
                threatDetail.DetectedByRadar = detectingRadars.ToArray();

                if (detectingRadars == null || !detectingRadars.Any())
                {
                    // Tespit edilmediği durumda ThreatScore'u sıfırla
                    threatDetail.ThreatScore = 0;
                }

                //MessageBox.Show($"Threat: {threatDetail.Aircraft?.Name}, ThreatLevel: {threatDetail.ThreatLevel}, Score: {threatDetail.ThreatScore}");

                threatDetail.AssignedADS = GetOptimalAirDefenseSystem(threatDetail, _airDefenseSystems);
                if (threatDetail.AssignedADS == null)
                {
                    MessageBox.Show($"No suitable ADS found for Threat: {threatDetail.Aircraft?.Name}");
                }

                threatDetails.Add(threatDetail);
            }
        }
        private AirDefense? GetOptimalAirDefenseSystem(ThreatDetail threat, List<AirDefenseInput> airDefenseSystems)
        {
            AirDefense? optimalADS = null;
            double bestScore = double.MaxValue;

            foreach (var adsInput in airDefenseSystems)
            {
                var airDefense = adsInput.AirDefense;
                var adsPosition = ParsePosition(adsInput.Location);
                var threatPosition = ParsePosition(threat.Location);

                if (adsPosition == null || threatPosition == null)
                    continue;

                double distance = Position.CalculateDistance(threatPosition, adsPosition);

                if (AdsIsWithinRange(distance, threat.Altitude, airDefense))
                {
                    // Özel bir skor hesaplama (örnek: tehdit seviyesi * maliyet * mesafe)
                    double score = Convert.ToDouble(distance * airDefense.Cost / threat.ThreatScore);

                    if (score < bestScore)
                    {
                        bestScore = score;
                        optimalADS = airDefense;
                    }
                }
            }

            return optimalADS;
        }
        private void AssignAirDefenseSystemsToThreats()
        {
            foreach (var threat in threatDetails)
            {
                var optimalADS = GetOptimalAirDefenseSystem(threat, _airDefenseSystems);
                if (optimalADS != null)
                {
                    threat.AssignedADS = optimalADS;
                }
                else
                {
                    MessageBox.Show($"No suitable Air Defense System found for threat: {threat.Aircraft?.Name}");
                }
            }
        }
        public Dictionary<Aircraft, AirDefense?> AssignAirDefenseSystemsToThreats(
        List<Aircraft> threats,
        List<AirDefense> airDefenses,
        Dictionary<AirDefense, Position> airDefensePositions,
        Dictionary<Aircraft, Position> threatPositions)
        {
            var assignment = new Dictionary<Aircraft, AirDefense?>();

            foreach (var threat in threats)
            {
                AirDefense? bestAirDefense = null;
                double bestScore = double.MaxValue;

                foreach (var airDefense in airDefenses)
                {
                    var airDefensePosition = airDefensePositions[airDefense];
                    var threatPosition = threatPositions[threat];

                    if (CanEngageThreat(airDefense, threat, airDefensePosition, threatPosition))
                    {
                        double distance = Position.CalculateDistance(airDefensePosition, threatPosition);
                        double costFactor = airDefense.Cost / 1000000; // Maliyet normalize edilir.

                        double score = distance * costFactor; // Basit bir maliyet-temelli skor.

                        if (score < bestScore)
                        {
                            bestScore = score;
                            bestAirDefense = airDefense;
                        }
                    }
                }

                assignment[threat] = bestAirDefense;
            }

            return assignment;
        }
        private double Normalize(double value, double min, double max)
        {
            return (value - min) / (max - min);
        }
        private List<Radar> GetRadarsDetectingThreat(ThreatDetail threat, List<AirDefenseInput> airDefenseSystems)
        {
            var detectingRadars = new List<Radar>();

            foreach (var adsInput in airDefenseSystems)
            {
                var adsPosition = ParsePosition(adsInput.Location);
                var threatPosition = ParsePosition(threat.Location);

                if (adsPosition == null || threatPosition == null)
                    continue;

                double distance = Position.CalculateDistance(threatPosition, adsPosition);

                foreach (var radar in adsInput.AirDefense.Radars)
                {
                    if (distance <= radar.Radar.MaxDetectionRange)
                    {
                        detectingRadars.Add(radar.Radar);
                        MessageBox.Show($"Radar {radar.Radar.Name} detected threat: {threat.Aircraft?.Name}");
                    }
                }
            }

            return detectingRadars;
        }
        private string GetThreatLevel(double threatLevel)
        {
            return threatLevel >= 0.90 ? "Very High" :
                   threatLevel >= 0.75 ? "High" :
                   threatLevel >= 0.50 ? "Normal" :
                   threatLevel >= 0.25 ? "Low" : "Very Low";
        }
        private void ShowThreatDetailsWindow()
        {
            ThreatDetailsWindow threatDetailsWindow = new ThreatDetailsWindow(threatDetails.Select((detail, index) =>
            {
                detail.Index = index + 1;
                return detail;
            }).ToList());
            threatDetailsWindow.Show();
        }
        private static bool AdsIsWithinRange(double distance, double altitude, AirDefense airDefense)
        {
            return distance >= airDefense.AerodynamicTargetRangeMin &&
                   distance <= airDefense.AerodynamicTargetRangeMax &&
                   altitude <= airDefense.BallisticTargetRangeMax &&
                   //airDefense.CurrentEngagements < airDefense.MaxEngagements &&
                   airDefense.Munitions.Any(m => m.Quantity > 0);
        }
        public bool CanEngageThreat(AirDefense airDefense, Aircraft threat, Position airDefensePosition, Position threatPosition)
        {
            double distance = Position.CalculateDistance(airDefensePosition, threatPosition);

            // Radar yeteneklerini kontrol et
            bool withinRadarRange = airDefense.Radars.Any(radar =>
                radar.Radar.MaxDetectionRange >= distance &&
                threat.Speed <= radar.Radar.MaxTargetSpeed);

            // Mühimmat ve angajman durumu
            bool hasAvailableMunitions = airDefense.Munitions.Any(m => m.Quantity > 0 && m.Munition.Range >= distance);

            // Angajman kriterleri
            return withinRadarRange && hasAvailableMunitions && airDefense.MaxEngagements > 0;
        }
        private Position GetSourcePosition()
        {
            Position sourcePosition = new Position(
              double.Parse(LatitudeTextBox.Text, CultureInfo.InvariantCulture),
              double.Parse(LongitudeTextBox.Text, CultureInfo.InvariantCulture),
              double.Parse(AltitudeTextBox.Text, CultureInfo.InvariantCulture));

            return sourcePosition;
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
        private string GenerateRandomLocation()
        {
            // Enlem (Latitude): 36° ile 42° arasında
            double latitude = 36 + rand.NextDouble() * 6; // 36 ile 42 arasında

            // Boylam (Longitude): 26° ile 45° arasında
            double longitude = 26 + rand.NextDouble() * 19; // 26 ile 45 arasında

            // İrtifa (Altitude): 0 ile 3,000 metre arasında
            double altitude = rand.NextDouble() * 3000;

            // Değerleri formatlayarak string olarak döndür
            return $"{latitude.ToString("F4", CultureInfo.InvariantCulture)}, {longitude.ToString("F4", CultureInfo.InvariantCulture)}, {altitude.ToString("F0", CultureInfo.InvariantCulture)}";
        }
    }
}
