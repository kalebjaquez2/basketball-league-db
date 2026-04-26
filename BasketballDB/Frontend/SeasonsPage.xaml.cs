using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System.Windows;
using System.Windows.Controls;

namespace Frontend
{
    public partial class SeasonsPage : Page
    {
        private readonly League _league;
        private readonly string _connectionString;

        public SeasonsPage(League league, string connectionString)
        {
            InitializeComponent();
            _league = league;
            _connectionString = connectionString;
            LeagueHeader.Text = league.LeagueName;
            LoadSeasons();
        }

        private void LoadSeasons()
        {
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlSeasonRepository(executor);
            SeasonsList.ItemsSource = repo.RetrieveSeasonsByLeague(_league.LeagueID);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                // Switch visibility back to the tiles
                mainWindow.MainFrame.Visibility = Visibility.Collapsed;
                mainWindow.HomeScreen.Visibility = Visibility.Visible;
            }
        }

        private void AddSeason_Click(object sender, RoutedEventArgs e)
        {
            // 1. Create the dialog (you'll likely build this next)
            var dialog = new AddSeasonDialog(_league, _connectionString);
            dialog.Owner = Window.GetWindow(this);

            // 2. Show the dialog and check if the user saved
            if (dialog.ShowDialog() == true)
            {
                // 3. Refresh the list so the new season appears immediately
                LoadSeasons();
            }
        }

        private void SeasonTile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Season selectedSeason)
            {
                this.NavigationService.Navigate(new TeamsPage(selectedSeason, _connectionString));
            }
        }
    }
}
