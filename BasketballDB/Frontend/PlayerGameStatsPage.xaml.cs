using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Frontend
{
    public partial class PlayerGameStatsPage : Page
    {
        private readonly Game _game;
        private readonly string _connectionString;
        private DispatcherTimer? _debounceTimer;
        private EditablePlayerGameStats? _editingStats;

        public PlayerGameStatsPage(Game game, string connectionString)
        {
            InitializeComponent();
            _game = game;
            _connectionString = connectionString;
            LoadBoxScore();
            LoadTeamNames();
            LoadGameStatsSummary();
            if (Session.IsAdmin)
                AutoFillButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void LoadBoxScore()
        {
            var executor = new SqlCommandExecutor(_connectionString);
            var statsRepo = new SqlPlayerGameStatsRepository(executor);
            var playerRepo = new SqlPlayerRepository(executor);

            var existingStats = statsRepo.RetrieveStatsByGame(_game.GameID)
                .ToDictionary(s => s.PlayerID);

            var homePlayers = playerRepo.RetrievePlayersByTeam(_game.HomeTeamID);
            var awayPlayers = playerRepo.RetrievePlayersByTeam(_game.AwayTeamID);

            HomeStatsGrid.ItemsSource = BuildStatsList(homePlayers, existingStats, _game.HomeTeamID);
            AwayStatsGrid.ItemsSource = BuildStatsList(awayPlayers, existingStats, _game.AwayTeamID);
        }

        private List<EditablePlayerGameStats> BuildStatsList(
            IReadOnlyList<Player> players,
            Dictionary<int, PlayerGameStats> existingStats,
            int teamID)
        {
            return players.Select(p =>
            {
                if (existingStats.TryGetValue(p.PlayerID, out var s))
                    return new EditablePlayerGameStats(s);

                return new EditablePlayerGameStats(new PlayerGameStats(
                    p.PlayerID, _game.GameID, teamID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                { PlayerName = p.PlayerName });
            }).ToList();
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

        private void StatsGrid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Session.IsAdmin) return;
            if (sender is not DataGrid dg) return;

            var clicked = dg.SelectedItem as EditablePlayerGameStats;

            // Close any open row — if it's a different row, stop here so the
            // grid reloads cleanly before the user opens another one
            if (_editingStats != null && _editingStats != clicked)
            {
                CloseEditing(dg);
                return;
            }

            if (clicked == null)
            {
                CloseEditing(dg);
                return;
            }

            if (clicked.IsEditing)
                CloseEditing(dg);
            else
            {
                _editingStats = clicked;
                clicked.PropertyChanged += Stats_PropertyChanged;
                clicked.IsEditing = true;
                dg.SelectedItem = null;
            }
        }

        private void CloseEditing(DataGrid dg)
        {
            if (_editingStats == null) return;
            _debounceTimer?.Stop();
            _debounceTimer = null;
            _editingStats.PropertyChanged -= Stats_PropertyChanged;
            SaveStatsInternal(_editingStats);
            _editingStats.IsEditing = false;
            _editingStats = null;
            dg.SelectedItem = null;
            LoadBoxScore();
            LoadGameStatsSummary();
        }

        private void Stats_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is EditablePlayerGameStats stats && e.PropertyName?.StartsWith("Edit") == true)
                ScheduleSave(stats);
        }

        private void ScheduleSave(EditablePlayerGameStats stats)
        {
            _debounceTimer?.Stop();
            _debounceTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(600) };
            _debounceTimer.Tick += (s, e) =>
            {
                _debounceTimer!.Stop();
                SaveStatsInternal(stats);
            };
            _debounceTimer.Start();
        }

        private void SaveStatsInternal(EditablePlayerGameStats stats)
        {
            try
            {
                int minutes   = Parse(stats.EditMinutes);
                int rebounds  = Parse(stats.EditRebounds);
                int assists   = Parse(stats.EditAssists);
                int turnovers = Parse(stats.EditTurnovers);
                int steals    = Parse(stats.EditSteals);
                int blocks    = Parse(stats.EditBlocks);
                int fgMade    = Parse(stats.EditFGMade);
                int fgTaken   = Parse(stats.EditFGTaken);
                int threeMade = Parse(stats.EditThreeMade);
                int threeTaken= Parse(stats.EditThreeTaken);
                int fouls     = Parse(stats.EditFouls);

                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlPlayerGameStatsRepository(executor);
                repo.UpdatePlayerGameStats(
                    stats.PlayerID, stats.GameID, stats.TeamID,
                    minutes, turnovers, rebounds, assists, steals, blocks,
                    fgMade, fgTaken, threeMade, threeTaken, fouls
                );

                LoadGameStatsSummary();
            }
            catch (FormatException)
            {
                // User is mid-typing an invalid value — skip this save
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving stats: " + ex.Message);
            }
        }

        private void AutoFillStats_Click(object sender, RoutedEventArgs e)
        {
            var rng = new Random();
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlPlayerGameStatsRepository(executor);

            var allRows = (HomeStatsGrid.ItemsSource as List<EditablePlayerGameStats> ?? new())
                .Concat(AwayStatsGrid.ItemsSource as List<EditablePlayerGameStats> ?? new());

            foreach (var row in allRows)
            {
                int fgTaken    = rng.Next(5, 23);
                int fgMade     = rng.Next(0, fgTaken + 1);
                int tpTaken    = rng.Next(1, 11);
                int tpMade     = rng.Next(0, tpTaken + 1);

                repo.UpdatePlayerGameStats(
                    row.PlayerID, row.GameID, row.TeamID,
                    rng.Next(5, 35),  // minutes
                    rng.Next(0, 6),   // turnovers
                    rng.Next(0, 10),  // rebounds
                    rng.Next(0, 8),   // assists
                    rng.Next(0, 4),   // steals
                    rng.Next(0, 3),   // blocks
                    fgMade, fgTaken,
                    tpMade, tpTaken,
                    rng.Next(0, 5)    // fouls
                );
            }

            LoadBoxScore();
            LoadGameStatsSummary();
        }

        private void StatBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
                tb.CaretIndex = tb.Text.Length;
        }

        private static int Parse(string s) =>
            string.IsNullOrWhiteSpace(s) ? 0 : int.Parse(s);
    }
}
