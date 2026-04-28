

using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System;
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
            LoadGameStatsSummary();
        }

        private void LoadBoxScore()
        {
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlPlayerGameStatsRepository(executor);
            _allStats = repo.RetrieveStatsByGame(_game.GameID).ToList();

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

        private void LoadGameStatsSummary()
        {
            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlStatsRepository(executor);

                // Use the game date as both start and end date
                // to get stats just for this specific game
                var gameDate = DateOnly.FromDateTime(_game.Date.ToDateTime(TimeOnly.MinValue));
                var summary = repo.RetrieveGameStatsSummary(gameDate, gameDate)
                    .FirstOrDefault(s => s.GameID == _game.GameID);

                if (summary != null)
                {
                    AvgPointsText.Text = summary.AveragePoints.ToString("F1");
                    AvgReboundsText.Text = summary.AverageRebounds.ToString("F1");
                    AvgAssistsText.Text = summary.AverageAssists.ToString("F1");
                    AvgTurnoversText.Text = summary.AverageTurnovers.ToString("F1");
                }
                else
                {
                    AvgPointsText.Text = "N/A";
                    AvgReboundsText.Text = "N/A";
                    AvgAssistsText.Text = "N/A";
                    AvgTurnoversText.Text = "N/A";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading game stats summary: " + ex.Message);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}