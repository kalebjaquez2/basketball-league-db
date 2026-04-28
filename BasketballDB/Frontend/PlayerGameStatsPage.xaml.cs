using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            LoadTeamNames();
        }

        private void LoadBoxScore()
        {
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlPlayerGameStatsRepository(executor);

            _allStats = repo.RetrieveStatsByGame(_game.GameID).ToList();

            // Split into teams
            var homeStats = _allStats
                .Where(s => s.TeamID == _game.HomeTeamID)
                .ToList();

            var awayStats = _allStats
                .Where(s => s.TeamID == _game.AwayTeamID)
                .ToList();

            HomeStatsGrid.ItemsSource = homeStats;
            AwayStatsGrid.ItemsSource = awayStats;
        }

        private void LoadTeamNames()
        {
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlTeamRepository(executor);

            var homeTeam = repo.FetchTeam(_game.HomeTeamID);
            var awayTeam = repo.FetchTeam(_game.AwayTeamID);

            HomeTeamLabel.Text = homeTeam.TeamName;
            AwayTeamLabel.Text = awayTeam.TeamName;

            GameHeader.Text = $"{homeTeam.TeamName} vs {awayTeam.TeamName}";
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}