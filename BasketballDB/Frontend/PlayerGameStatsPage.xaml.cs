using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Frontend
{
    public partial class PlayerGameStatsPage : Page
    {
        private readonly Game _game;
        private readonly string _connectionString;
        private List<PlayerGameStats>? _allStats;


        public PlayerGameStatsPage(Game game, string connectionString)
        {
            InitializeComponent();
            _game = game;
            _connectionString = connectionString;
            LoadBoxScore();
        }

        private void LoadBoxScore()
        {
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlPlayerGameStatsRepository(executor);
            _allStats = repo.RetrieveStatsByGame(_game.GameID).ToList();

            // Default to showing Home Team
            ShowTeam(true);
        }

        private void ToggleTeam_Click(object sender, RoutedEventArgs e)
        {
            bool isHome = (sender == HomeBtn);
            ShowTeam(isHome);
        }

        private void ShowTeam(bool isHome)
        {
            int teamId = isHome ? _game.HomeTeamID : _game.AwayTeamID;
            StatsGrid.ItemsSource = _allStats?.Where(s => s.TeamID == teamId).ToList();

            // Update button colors to show which is active
            HomeBtn.Background = isHome ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F05A28")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#242424"));
            AwayBtn.Background = !isHome ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F05A28")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#242424"));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}