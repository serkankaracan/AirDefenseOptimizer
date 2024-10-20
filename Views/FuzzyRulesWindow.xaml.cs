using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
            LoadRules(new AirDefenseRules(), AirDefenseRulesList);

            // Aircraft Kuralları
            LoadRules(new AircraftRules(), AircraftRulesList);

            // Radar Kuralları
            LoadRules(new RadarRules(), RadarRulesList);

            // Munition Kuralları
            LoadRules(new MunitionRules(), MunitionRulesList);
        }

        private void LoadRules<T>(T ruleSet, ItemsControl rulesList) where T : class
        {
            List<FuzzyRuleViewModel> ruleViewModels = new List<FuzzyRuleViewModel>();
            dynamic rules = ruleSet.GetType().GetProperty("Rules").GetValue(ruleSet);

            int ruleIndex = 1; // Kurallar için bir sayaç

            foreach (var rule in rules)
            {
                ruleViewModels.Add(new FuzzyRuleViewModel
                {
                    ButtonLabel = $"Kural {ruleIndex}", // Buton üzerinde Kural 1, Kural 2 gibi gösterilecek
                    RuleDescription = $"Koşullar: \n{string.Join(", \n", rule.Conditions)}, \nSonuç: \n{string.Join(", \n", rule.Consequences)}"
                });
                ruleIndex++;
            }

            rulesList.ItemsSource = ruleViewModels;
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
    }

    public class FuzzyRuleViewModel
    {
        public string ButtonLabel { get; set; }
        public string RuleDescription { get; set; }
    }
}
