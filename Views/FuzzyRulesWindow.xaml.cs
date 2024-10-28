using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AirDefenseOptimizer.FuzzyRules;

namespace AirDefenseOptimizer.Views
{
    public partial class FuzzyRulesWindow : UserControl
    {
        public FuzzyRulesWindow()
        {
            InitializeComponent();
            LoadFuzzyRules();
        }

        private void LoadFuzzyRules()
        {
            // Air Defense Kuralları
            LoadRules(new AirDefenseRules(), AirDefenseRulesList, "DefenseScore", AirDefenseInfo);

            // Aircraft Kuralları
            LoadRules(new AircraftRules(), AircraftRulesList, "ThreatScore", AircraftInfo);

            // Radar Kuralları
            LoadRules(new RadarRules(), RadarRulesList, "SurveillanceScore", RadarInfo);

            // Munition Kuralları
            LoadRules(new MunitionRules(), MunitionRulesList, "ImpactScore", MunitionInfo);
        }

        private void LoadRules<T>(T ruleSet, ItemsControl rulesList, string consequenceKey, TextBlock infoBlock) where T : class
        {
            List<FuzzyRuleViewModel> ruleViewModels = new List<FuzzyRuleViewModel>();
            dynamic rules = ruleSet.GetType().GetProperty("Rules").GetValue(ruleSet);

            int ruleIndex = 1; // Kurallar için bir sayaç
            int criticalCount = 0, highCount = 0, mediumCount = 0, lowCount = 0, veryLowCount = 0;

            foreach (var rule in rules)
            {
                string score = "Unknown"; // Varsayılan değer
                if (rule.Consequences.ContainsKey(consequenceKey))
                {
                    score = rule.Consequences[consequenceKey].ToString();
                }

                var backgroundColor = GetButtonColor(score);

                // Seviyeye göre sayma
                switch (score)
                {
                    case "Critical":
                        criticalCount++;
                        break;
                    case "High":
                        highCount++;
                        break;
                    case "Medium":
                        mediumCount++;
                        break;
                    case "Low":
                        lowCount++;
                        break;
                    case "Very Low":
                        veryLowCount++;
                        break;
                }

                ruleViewModels.Add(new FuzzyRuleViewModel
                {
                    ButtonLabel = $"Kural {ruleIndex}",
                    RuleDescription = $"Koşullar: \n{string.Join(", \n", rule.Conditions)}, \nSonuç: \n{string.Join(", \n", rule.Consequences)}",
                    ButtonBackgroundColor = backgroundColor
                });
                ruleIndex++;
            }

            rulesList.ItemsSource = ruleViewModels;

            // Bilgi alanını güncelle
            infoBlock.Text = $"Toplam Kural: {ruleIndex - 1}\n" +
                             $"Critical: {criticalCount}, High: {highCount}, Medium: {mediumCount}, Low: {lowCount}, Very Low: {veryLowCount}";
        }

        private void RuleButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedRule = button?.DataContext as FuzzyRuleViewModel;

            if (selectedRule != null)
            {
                MessageBox.Show($"Seçilen Kural Detayı: \n{selectedRule.RuleDescription}", "\nKural Detayı\n", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private Brush GetButtonColor(string score)
        {
            switch (score)
            {
                case "Critical":
                    return Brushes.Red;       // Critical tehdit seviyesi için kırmızı
                case "High":
                    return Brushes.Orange;    // High tehdit seviyesi için turuncu
                case "Medium":
                    return Brushes.Yellow;    // Medium tehdit seviyesi için sarı
                case "Low":
                    return Brushes.LightGreen; // Low tehdit seviyesi için açık yeşil
                case "Very Low":
                    return Brushes.Green;      // Very Low tehdit seviyesi için yeşil
                default:
                    return Brushes.Gray;       // Varsayılan olarak gri
            }
        }
    }

    public class FuzzyRuleViewModel
    {
        public string? ButtonLabel { get; set; }
        public Brush? ButtonBackgroundColor { get; set; }
        public string? RuleDescription { get; set; }
    }
}
