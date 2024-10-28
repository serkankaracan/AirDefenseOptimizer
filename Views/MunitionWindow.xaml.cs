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

            try
            {
                _munitionService = new MunitionService(App.ConnectionManager!, App.DatabaseHelper!);
                LoadMunitionData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Munition Window: {ex.Message}");
            }
        }

        private void LoadMunitionData()
        {
            try
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
                    Maneuverability = munition["Maneuverability"],
                    ExplosivePower = munition["ExplosivePower"],
                    Cost = munition["Cost"]
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading munitions data: {ex.Message}");
            }
        }

        private void SearchMunition_Click(object sender, RoutedEventArgs e)
        {
            try
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
                        Maneuverability = munition["Maneuverability"],
                        ExplosivePower = munition["ExplosivePower"],
                        Cost = munition["Cost"]
                    }).ToList();

                MunitionDataGrid.ItemsSource = filteredMunitions;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during search: {ex.Message}");
            }
        }

        private void AddNewMunition_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MunitionEditWindow munitionEditWindow = new MunitionEditWindow(_munitionService, null);
                munitionEditWindow.ShowDialog();
                LoadMunitionData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding new munition: {ex.Message}");
            }
        }

        private void PreviewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedMunition = ((Button)sender).DataContext as dynamic;
                MunitionEditWindow munitionEditWindow = new MunitionEditWindow(_munitionService, selectedMunition, isReadOnly: true);
                munitionEditWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error previewing munition: {ex.Message}");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedMunition = ((Button)sender).DataContext as dynamic;
                MunitionEditWindow munitionEditWindow = new MunitionEditWindow(_munitionService, selectedMunition);
                munitionEditWindow.ShowDialog();
                LoadMunitionData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing munition: {ex.Message}");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
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
                        MessageBox.Show($"Error deleting munition: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during delete operation: {ex.Message}");
            }
        }
    }
}
