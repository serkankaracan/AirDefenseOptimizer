using AirDefenseOptimizer.Views;
using System.Windows;

namespace AirDefenseOptimizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnHomePage_Click(null!, null!);
        }

        private void btnHomePage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HomeWindow();
        }

        private void btnFuzzyRulesPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new FuzzyRulesWindow();
        }

        private void btnAdsPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AirDefenseWindow();
        }

        private void btnAircratfPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AircraftWindow();
        }

        private void btnMunitionPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new MunitionWindow();
        }

        private void btnRadarPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new RadarWindow();
        }

        private void btnAboutPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new AboutWindow();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
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
}