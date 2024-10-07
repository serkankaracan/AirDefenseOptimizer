using AirDefenseOptimizer.Services;
using System.Windows;
using System.Windows.Controls;

namespace AirDefenseOptimizer.Views
{
    public partial class MunitionWindow : UserControl
    {
        private readonly MunitionService _munitionService;

        public MunitionWindow()
        {
            InitializeComponent();
            _munitionService = new MunitionService(App.ConnectionManager!, App.DatabaseHelper!);

            LoadMunitionData();
        }

        private void LoadMunitionData()
        {
            var munitions = _munitionService.GetAllMunitions();

            if (munitions.Count <= 0)
                return;

            MunitionDataGrid.ItemsSource = munitions.Select(munition => new
            {
                Id = munition["Id"],
                Name = munition["Name"],
                MunitionType = munition["MunitionType"],
                Weight = munition["Weight"],
                Speed = munition["Speed"],
                Range = munition["Range"],
                ExplosivePower = munition["ExplosivePower"],
                Cost = munition["Cost"]
            }).ToList();
        }

        private void SearchMunition_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();
            var filteredMunitions = _munitionService.GetAllMunitions()
                .Where(munition => munition["Name"].ToString()!.ToLower().Contains(searchTerm) ||
                                   munition["MunitionType"].ToString()!.ToLower().Contains(searchTerm))
                .Select(munition => new
                {
                    Id = munition["Id"],
                    Name = munition["Name"],
                    MunitionType = munition["MunitionType"],
                    Weight = munition["Weight"],
                    Speed = munition["Speed"],
                    Range = munition["Range"],
                    ExplosivePower = munition["ExplosivePower"],
                    Cost = munition["Cost"]
                }).ToList();

            MunitionDataGrid.ItemsSource = filteredMunitions;
        }

        private void AddNewMunition_Click(object sender, RoutedEventArgs e)
        {
            MunitionEditWindow munitionEditWindow = new MunitionEditWindow(_munitionService, null);
            munitionEditWindow.ShowDialog();
            LoadMunitionData();
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMunition = ((Button)sender).DataContext as dynamic;
            MunitionEditWindow munitionEditWindow = new MunitionEditWindow(_munitionService, selectedMunition, isReadOnly: true);
            munitionEditWindow.ShowDialog();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMunition = ((Button)sender).DataContext as dynamic;
            MunitionEditWindow munitionEditWindow = new MunitionEditWindow(_munitionService, selectedMunition);
            munitionEditWindow.ShowDialog();
            LoadMunitionData();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMunition = ((Button)sender).DataContext as dynamic;
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete Munition {selectedMunition.Name}?",
                                                      "Delete Confirmation",
                                                      MessageBoxButton.YesNo,
                                                      MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _munitionService.DeleteMunition((int)selectedMunition.Id);
                    MessageBox.Show("Munition deleted successfully.");
                    LoadMunitionData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}
