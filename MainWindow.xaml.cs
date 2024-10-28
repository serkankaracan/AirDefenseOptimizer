using AirDefenseOptimizer.Views;
using System.Windows;
using System.Windows.Controls;

namespace AirDefenseOptimizer
{
    public partial class MainWindow : Window
    {
        private Button? _selectedButton;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                MenuButton_Click(btnHomePage, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing the main window: {ex.Message}");
            }
        }

        private void MenuButton_Click(object sender, RoutedEventArgs? e)
        {
            try
            {
                var clickedButton = sender as Button;
                if (clickedButton == null) return;

                // Tıklanan butonu seçili yap
                if (clickedButton != btnExit) SelectButton(clickedButton);

                // Tıklanan butona göre içerik değiştir
                if (clickedButton == btnHomePage)
                {
                    MainContent.Content = new HomeWindow();
                }
                else if (clickedButton == btnFuzzyRulesPage)
                {
                    MainContent.Content = new FuzzyRulesWindow();
                }
                else if (clickedButton == btnAdsPage)
                {
                    MainContent.Content = new AirDefenseWindow();
                }
                else if (clickedButton == btnAircratfPage)
                {
                    MainContent.Content = new AircraftWindow();
                }
                else if (clickedButton == btnMunitionPage)
                {
                    MainContent.Content = new MunitionWindow();
                }
                else if (clickedButton == btnRadarPage)
                {
                    MainContent.Content = new RadarWindow();
                }
                else if (clickedButton == btnAboutPage)
                {
                    MainContent.Content = new AboutWindow();
                }
                else if (clickedButton == btnExit)
                {
                    MessageBoxResult result = MessageBox.Show("Uygulamadan çıkmak istediğinizden emin misiniz?",
                                                              "Çıkış Onayı",
                                                              MessageBoxButton.YesNo,
                                                              MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        Application.Current.Shutdown();  // Evet seçildiyse uygulama kapanır
                    }
                    // Hayır seçildiyse hiçbir şey yapılmaz
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while processing the menu button click: {ex.Message}");
            }
        }

        private void SelectButton(Button button)
        {
            try
            {
                // Eğer önceden bir buton seçildiyse onun "Tag" değerini temizle
                if (_selectedButton != null)
                {
                    _selectedButton.Tag = null;
                }

                // Yeni seçilen butona "Selected" Tag'ini ekle
                button.Tag = "Selected";
                _selectedButton = button;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while selecting the button: {ex.Message}");
            }
        }
    }
}
