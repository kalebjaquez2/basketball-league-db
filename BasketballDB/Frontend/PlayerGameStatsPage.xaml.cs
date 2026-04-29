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

        public PlayerGameStatsPage(Game game, string connectionString)
        {
            InitializeComponent();
            _game = game;
            _connectionString = connectionString;
            LoadBoxScore();
            LoadTeamNames();
            LoadGameStatsSummary();

            if (!Session.IsAdmin)
            {
                HomeStatsGrid.Columns[HomeStatsGrid.Columns.Count - 1].Visibility = Visibility.Collapsed;
                AwayStatsGrid.Columns[AwayStatsGrid.Columns.Count - 1].Visibility = Visibility.Collapsed;
            }
        }

        private void LoadBoxScore()
        {
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlPlayerGameStatsRepository(executor);
            var allStats = repo.RetrieveStatsByGame(_game.GameID)
                .Select(s => new EditablePlayerGameStats(s))
                .ToList();

            HomeStatsGrid.ItemsSource = allStats.Where(s => s.TeamID == _game.HomeTeamID).ToList();
            AwayStatsGrid.ItemsSource = allStats.Where(s => s.TeamID == _game.AwayTeamID).ToList();
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

        private void StatsOptions_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        private void EditStats_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi &&
                mi.Parent is ContextMenu cm &&
                cm.PlacementTarget is Button btn &&
                btn.Tag is EditablePlayerGameStats stats)
            {
                stats.IsEditing = true;
            }
        }

        private void SaveStats_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EditablePlayerGameStats stats)
            {
                try
                {
                    int minutes  = Parse(stats.EditMinutes);
                    int rebounds = Parse(stats.EditRebounds);
                    int assists  = Parse(stats.EditAssists);
                    int turnovers= Parse(stats.EditTurnovers);
                    int steals   = Parse(stats.EditSteals);
                    int blocks   = Parse(stats.EditBlocks);
                    int fgMade   = Parse(stats.EditFGMade);
                    int fgTaken  = Parse(stats.EditFGTaken);
                    int threeMade= Parse(stats.EditThreeMade);
                    int threeTaken=Parse(stats.EditThreeTaken);
                    int fouls    = Parse(stats.EditFouls);

                    var executor = new SqlCommandExecutor(_connectionString);
                    var repo = new SqlPlayerGameStatsRepository(executor);
                    repo.UpdatePlayerGameStats(
                        stats.PlayerID, stats.GameID, stats.TeamID,
                        minutes, turnovers, rebounds, assists, steals, blocks,
                        fgMade, fgTaken, threeMade, threeTaken, fouls
                    );

                    LoadBoxScore();
                    LoadGameStatsSummary();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving stats: " + ex.Message);
                    stats.IsEditing = false;
                }
            }
        }

        private void CancelStatsEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EditablePlayerGameStats stats)
                stats.IsEditing = false;
        }

        private static int Parse(string s) =>
            string.IsNullOrWhiteSpace(s) ? 0 : int.Parse(s);
    }
}
