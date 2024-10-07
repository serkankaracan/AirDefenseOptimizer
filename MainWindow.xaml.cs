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
            MainContent.Content = new Views.HomeWindow();
        }

        private void btnAdsPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.AirDefenseWindow();
        }

        private void btnAircratfPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.AircraftWindow();
        }

        private void btnMunitionPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.MunitionWindow();
        }

        private void btnRadarPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.RadarWindow();
        }

        private void btnAboutPage_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Views.AboutWindow();
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