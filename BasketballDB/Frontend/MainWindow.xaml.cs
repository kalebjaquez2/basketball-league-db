using System.Windows;
using System.Windows.Controls;

namespace Frontend
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadLeagues();
        }

        private void LoadLeagues()
        {
            // TODO: Replace with real data from Backend
            // Example: LeagueTilesControl.ItemsSource = LeagueRepository.GetAllLeagues();
            var sampleLeagues = new[]
            {
                new { LeagueID = 1, LeagueName = "Adult League", Location = "Downtown Rec Center" },
                new { LeagueID = 2, LeagueName = "Youth League", Location = "Downtown Rec Center" },
                new { LeagueID = 3, LeagueName = "3v3 Summer League", Location = "Downtown Rec Center" }
            };
            LeagueTilesControl.ItemsSource = sampleLeagues;
        }

        private void ShowHomeScreen()
        {
            HomeScreen.Visibility = Visibility.Visible;
            MainFrame.Visibility = Visibility.Collapsed;
        }

        private void ShowFrame()
        {
            HomeScreen.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
        }

        private void AddLeague_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Open Add League dialog/page
            // Example: new AddLeagueWindow().ShowDialog();
            // Then reload leagues after adding
            // LoadLeagues();
        }

        private void LeagueTile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int leagueId)
            {
                ShowFrame();
                // TODO: MainFrame.Navigate(new SeasonsView(leagueId));
            }
        }
    }
}
